using MySqlConnector;
public class ProductosQuerys
{
    private ConexionDB conexionDB = new ConexionDB();

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

        var db = conexionDB.AbrirConexion();
        var productos = new List<Producto>();

        try
        {
            var commando = new MySqlCommand(consulta, db);
            commando.Parameters.AddWithValue("@IdSucursal", IdSucursal);
            var reader = await commando.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var id = reader.GetInt32("IdProducto");
                var codigo = reader.GetInt32("Codigo");
                var nombre = reader.GetString("Nombre");
                var precio = reader.GetDecimal("Precio");
                var stock = reader.GetInt32("Stock");
                var tipo = reader.GetString("TipoProducto");
                Producto producto = tipo switch
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
            db.Close();
        }
        return productos;
    }

    public async Task CrearProducto(Producto producto, int sucursalID)
    {
        var db = conexionDB.AbrirConexion();
        var tipo = producto.GetType().Name;

        try
        {

            var sqlProducto = @"
            INSERT INTO Producto (Codigo, Nombre, Precio, Stock, TipoProducto, IdSucursal)
            VALUES (@Codigo, @Nombre, @Precio, @Stock, @TipoProducto, @IdSucursal);
            SELECT LAST_INSERT_ID();";

            var comando = new MySqlCommand(sqlProducto, db);
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
            };
            var comandoHija = new MySqlCommand(sqlHija, db);
            comandoHija.Parameters.AddWithValue("@Id", idProducto);
            if (producto is Televisor t)
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

        }
        catch (Exception ex)
        {
            if (ex is MySqlException mysqlEx && mysqlEx.Number == 1062)
                throw new Exception("Ya existe un producto con el mismo código en esta sucursal");
            throw new Exception("Error al crear el producto: " + ex.Message);
        }
        finally
        {
            db.Close();
        }
    }

    public async Task EliminarProducto(int idProducto)
    {
        var db = conexionDB.AbrirConexion();
        try
        {
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
        var db = conexionDB.AbrirConexion();
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

            string sqlHija = tipo switch
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
}