using MySqlConnector;

// clase encargada de manejar las consultas relacionadas con los productos en la base de datos 
public class ProductosQuerys
{

    public async Task<List<Producto>> GetProductos(int IdSucursal)
    {
        var consulta = @"
        SELECT 
            p.IdProducto, p.Codigo, p.Nombre, p.Precio, p.Stock, p.TipoProducto,
            t.Pulgadas, t.TipoPantalla,
            h.CapacidadLitros, h.Tipo AS TipoHeladera,
            l.CargaKg, l.Tipo AS TipoLavarropas
        FROM Producto p
        JOIN Sucursal s ON p.IdSucursal = s.IdSucursal
        LEFT JOIN Televisor t ON p.IdProducto = t.IdProducto
        LEFT JOIN Heladera h ON p.IdProducto = h.IdProducto
        LEFT JOIN Lavarropas l ON p.IdProducto = l.IdProducto
        WHERE p.IdSucursal = @IdSucursal;";

        var db = await ConexionDB.AbrirConexion();
        var productos = new List<Producto>();

        try
        {
            var commando = new MySqlCommand(consulta, db);
            commando.Parameters.AddWithValue("@IdSucursal", IdSucursal); // previene inyeccion SQL
            var reader = await commando.ExecuteReaderAsync(); // ejecutar la consulta de forma asincrona
            while (await reader.ReadAsync())
            {
                var id = reader.GetInt32("IdProducto");
                var codigo = reader.GetInt32("Codigo");
                var nombre = reader.GetString("Nombre");
                var precio = reader.GetDecimal("Precio");
                var stock = reader.GetInt32("Stock");
                var tipo = reader.GetString("TipoProducto");
                Producto producto = tipo switch // usar el operador switch para crear la instancia del producto correcto segun su tipo
                {
                    "Televisor" => new Televisor(
                        id, codigo, nombre, precio, stock,
                        reader.GetInt32("Pulgadas"),
                        reader.GetString("TipoPantalla")),
                    "Heladera" => new Heladera(
                        id, codigo, nombre, precio, stock,
                        reader.GetInt32("CapacidadLitros"),
                        reader.GetString("TipoHeladera")),
                    "Lavarropas" => new Lavarropas(
                        id, codigo, nombre, precio, stock,
                        reader.GetInt32("CargaKg"),
                        reader.GetString("TipoLavarropas")),
                    _ => throw new Exception($"Tipo de producto desconocido: {tipo}")
                };
                productos.Add(producto);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los productos: " + ex.Message);
        }
        finally
        {
            db.Close(); // cerrar siempre la conexion a la db. el finally se ejecuta si o si sin importar si hubo error o no
        }
        return productos;
    }

    public async Task<int> CrearProducto(Producto producto, int sucursalID)
    {
        var db = await ConexionDB.AbrirConexion();
        var tipo = producto.GetType().Name;

        try
        {

            var sqlProducto = @"
            INSERT INTO Producto (Codigo, Nombre, Precio, Stock, TipoProducto, IdSucursal)
            VALUES (@Codigo, @Nombre, @Precio, @Stock, @TipoProducto, @IdSucursal);
            SELECT LAST_INSERT_ID();"; 
            // obtener el id del producto recien insertado para luego usarlo en la tabla hija correspondiente ya que es autoincremental y no lo conocemos antes de insertarlo

            var comando = new MySqlCommand(sqlProducto, db);
            //usar @ para prevenir inyeccion SQL y agregar los parametros necesarios para la consulta
            comando.Parameters.AddWithValue("@Codigo", producto.codigo);
            comando.Parameters.AddWithValue("@Nombre", producto.nombre);
            comando.Parameters.AddWithValue("@Precio", producto.precio);
            comando.Parameters.AddWithValue("@Stock", producto.cantidad);
            comando.Parameters.AddWithValue("@TipoProducto", tipo);
            comando.Parameters.AddWithValue("@IdSucursal", sucursalID);
            var idProducto = Convert.ToInt32(await comando.ExecuteScalarAsync());

            string sqlHija = tipo switch
            {
                "Televisor" => "INSERT INTO Televisor (IdProducto, Pulgadas, TipoPantalla) VALUES (@Id, @Extra1, @Extra2)",
                "Heladera" => "INSERT INTO Heladera (IdProducto, CapacidadLitros, Tipo) VALUES (@Id, @Extra1, @Extra2)",
                "Lavarropas" => "INSERT INTO Lavarropas (IdProducto, CargaKg, Tipo) VALUES (@Id, @Extra1, @Extra2)",
                _ => throw new Exception("Tipo inválido")
            }; // aqui se define el tipo de producto correcto y la consulta correcta para insertar los datos polimorficos a cada tabla hija

            var comandoHija = new MySqlCommand(sqlHija, db);
            comandoHija.Parameters.AddWithValue("@Id", idProducto);

            if (producto is Televisor t) //se usa is para verificar el tipo del producto y agregar los parametros correspondientes para cada tipo de producto
            {
                comandoHija.Parameters.AddWithValue("@Extra1", t.pulgadas);
                comandoHija.Parameters.AddWithValue("@Extra2", t.tipoPantalla);
            }
            else if (producto is Heladera h)
            {
                comandoHija.Parameters.AddWithValue("@Extra1", h.capacidad);
                comandoHija.Parameters.AddWithValue("@Extra2", h.tipo);
            }
            else if (producto is Lavarropas l)
            {
                comandoHija.Parameters.AddWithValue("@Extra1", l.carga);
                comandoHija.Parameters.AddWithValue("@Extra2", l.tipo);
            }
            await comandoHija.ExecuteNonQueryAsync();

            return idProducto; //devolver el id del producto recien creado 
        }
        catch (Exception ex)
        {
            if (ex is MySqlException mysqlEx && mysqlEx.Number == 1062) // verificar si el error es por clave duplicada ya que en la tabla Producto el campo Codigo es unico por sucursal (UNIQUE)
                throw new Exception("Ya existe un producto con el mismo código en esta sucursal");
            throw new Exception("Error al crear el producto: " + ex.Message); 
        }
        finally
        {
            db.Close();
        }
    }

    public async Task EliminarProducto(int idProducto)
    // usa task sin generico ya que no devuelve nada. es void pero asincrono
    {
        var db = await ConexionDB.AbrirConexion();
        try
        {
            //eliminar el producto por su id y desde la tabla padre. se eliminaran tambian las hijas gracias a (ON DELETE CASCADE) para que no queden datos huerfanos
            var sql = "DELETE FROM Producto WHERE IdProducto = @Id"; 
            var comando = new MySqlCommand(sql, db);
            comando.Parameters.AddWithValue("@Id", idProducto);
            await comando.ExecuteNonQueryAsync();

        }
        catch (Exception ex)
        {
            throw new Exception("Error al eliminar el producto: " + ex.Message);

        }
        finally
        {
            db.Close();
        }
    }

    public async Task ActualizarProducto(int idProducto, Producto producto, string tipo)
    {
        var db = await ConexionDB.AbrirConexion();
        try
        {
            var sqlProducto = @"
            UPDATE Producto 
            SET Nombre = @Nombre, Precio = @Precio, Stock = @Stock
            WHERE IdProducto = @Id;";

            var comando = new MySqlCommand(sqlProducto, db);
            comando.Parameters.AddWithValue("@Id", idProducto);
            comando.Parameters.AddWithValue("@Nombre", producto.nombre);
            comando.Parameters.AddWithValue("@Precio", producto.precio);
            comando.Parameters.AddWithValue("@Stock", producto.cantidad);
            await comando.ExecuteNonQueryAsync();

            string sqlHija = tipo switch //crear la query correcta segun tipo para actualizar los datos extras
            {
                "Televisor" => "UPDATE Televisor SET Pulgadas = @Extra1, TipoPantalla = @Extra2 WHERE IdProducto = @Id",
                "Heladera" => "UPDATE Heladera SET CapacidadLitros = @Extra1, Tipo = @Extra2 WHERE IdProducto = @Id",
                "Lavarropas" => "UPDATE Lavarropas SET CargaKg = @Extra1, Tipo = @Extra2 WHERE IdProducto = @Id",
                _ => throw new Exception("Tipo inválido")
            };
            var comandoHija = new MySqlCommand(sqlHija, db);
            comandoHija.Parameters.AddWithValue("@Id", idProducto);
            if (producto is Televisor Televisor)
            {
                comandoHija.Parameters.AddWithValue("@Extra1", Televisor.pulgadas);
                comandoHija.Parameters.AddWithValue("@Extra2", Televisor.tipoPantalla);
            }
            else if (producto is Heladera heladera)
            {
                comandoHija.Parameters.AddWithValue("@Extra1", heladera.capacidad);
                comandoHija.Parameters.AddWithValue("@Extra2", heladera.tipo);
            }
            else if (producto is Lavarropas lavarropa)
            {
                comandoHija.Parameters.AddWithValue("@Extra1", lavarropa.carga);
                comandoHija.Parameters.AddWithValue("@Extra2", lavarropa.tipo);
            }
            await comandoHija.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar el producto: " + ex.Message);
        }
        finally
        {
            db.Close();
        }
    }

    // primera capa. luego pasa ser usada unicamente por la capa de services o la capa de negocio. manteniendo una unica responsabilidad
}