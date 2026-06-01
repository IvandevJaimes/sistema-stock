using MySqlConnector;
public class VentasQuerys
{

    public async Task<List<VentaDetalle>> GetVentas(int idSucursal)
    {
        var consulta = @"
        SELECT v.IdVenta, v.Fecha, p.Nombre AS NombreProducto,
        dv.IdProducto, dv.Cantidad, dv.PrecioUnitario
        FROM Venta v
        JOIN DetalleVenta dv ON v.IdVenta = dv.IdVenta
        JOIN Producto p ON dv.IdProducto = p.IdProducto
        WHERE v.IdSucursal = @IdSucursal";
        //crear la consulta usando join uniendo las tres tablas necesarias para obtener toda la informacion de la venta

        var db = await ConexionDB.AbrirConexion();
        var ventas = new List<VentaDetalle>(); // variable vacia para ir guardando los resultados de la consulta

        try
        {
            var commando = new MySqlCommand(consulta, db);
            commando.Parameters.AddWithValue("@IdSucursal", idSucursal);
            var reader = await commando.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                ventas.Add(new VentaDetalle //usar la clase ventaDetalle para amoldar los resultados
                {
                    idVenta = reader.GetInt32("IdVenta"),
                    fecha = reader.GetDateTime("Fecha"),
                    nombreProducto = reader.GetString("NombreProducto"),
                    cantidad = reader.GetInt32("Cantidad"),
                    precioUnitario = reader.GetDecimal("PrecioUnitario")
                });
            }
            reader.Close();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener las ventas: " + ex.Message);
        }
        finally
        {
            db.Close();
        }
        return ventas;
    }

    public async Task RegistrarVenta(int idSucursal, List<CarritoItem> items )
    //pasar items como lista de CarritoItem para poder registrar varias ventas
    {
        var db = await ConexionDB.AbrirConexion();
        await using var transaction = await db.BeginTransactionAsync();  // declarar la transaccion
        // una transaccion es un codigo que se ejecuta evitando que se guarden cambios hasta que se cumpla la primera query. si falla, revierte todos los cambios
        try
        {
            var comandoVenta = new MySqlCommand(
                "INSERT INTO Venta (IdSucursal) VALUES (@IdSucursal); SELECT LAST_INSERT_ID();",
                db, transaction); //pasar como parametro la transaccion para que se ejecute dentro de ella
            comandoVenta.Parameters.AddWithValue("@IdSucursal", idSucursal);
            var idVenta = Convert.ToInt32(await comandoVenta.ExecuteScalarAsync());
            foreach (var item in items) //iterar sobre los items del carrito para guardar uno por uno 
            {
                var comandoDetalle = new MySqlCommand(
                    @"INSERT INTO DetalleVenta (IdVenta, IdProducto, Cantidad, PrecioUnitario)
                  VALUES (@IdVenta, @IdProducto, @Cantidad, @Precio);
                  UPDATE Producto SET Stock = Stock - @Cantidad WHERE IdProducto = @IdProducto",
                    db, transaction); //pasar la transaccion para que se ejecute dentro de ella y restar el stock del producto vendido correctamente
                comandoDetalle.Parameters.AddWithValue("@IdVenta", idVenta);
                comandoDetalle.Parameters.AddWithValue("@IdProducto", item.id);
                comandoDetalle.Parameters.AddWithValue("@Cantidad", item.cantidad);
                comandoDetalle.Parameters.AddWithValue("@Precio", item.precio);
                await comandoDetalle.ExecuteNonQueryAsync();
            }
            await transaction.CommitAsync();  //esperar a que se ejecuten las querys y confirmar la transaccion
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();  
            throw new Exception("Error al registrar la venta: " + ex.Message);
        }
        finally
        {
            db.Close();
        }

        //usar esta capa en la capa de servicios 
    }
}