using MySqlConnector;
public class VentasQuerys
{
    private ConexionDB conexionDB = new ConexionDB();

    public async Task<List<VentaDetalle>> GetVentas(int idSucursal)
    {
        var consulta = @"
        SELECT v.IdVenta, v.Fecha, p.Nombre AS NombreProducto,
        dv.IdProducto, dv.Cantidad, dv.PrecioUnitario
        FROM Venta v
        JOIN DetalleVenta dv ON v.IdVenta = dv.IdVenta
        JOIN Producto p ON dv.IdProducto = p.IdProducto
        WHERE v.IdSucursal = @IdSucursal";

        var db = conexionDB.AbrirConexion();
        var ventas = new List<VentaDetalle>();

        try
        {
            var commando = new MySqlCommand(consulta, db);
            commando.Parameters.AddWithValue("@IdSucursal", idSucursal);
            var reader = await commando.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                ventas.Add(new VentaDetalle
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

    public async Task RegistrarVenta(int idSucursal, List<CarritoItem> items)
    {
        var db = conexionDB.AbrirConexion();
        await using var transaction = await db.BeginTransactionAsync(); 
        try
        {
            var comandoVenta = new MySqlCommand(
                "INSERT INTO Venta (IdSucursal) VALUES (@IdSucursal); SELECT LAST_INSERT_ID();",
                db, transaction);
            comandoVenta.Parameters.AddWithValue("@IdSucursal", idSucursal);
            var idVenta = Convert.ToInt32(await comandoVenta.ExecuteScalarAsync());
            foreach (var item in items)
            {
                var comandoDetalle = new MySqlCommand(
                    @"INSERT INTO DetalleVenta (IdVenta, IdProducto, Cantidad, PrecioUnitario)
                  VALUES (@IdVenta, @IdProducto, @Cantidad, @Precio);
                  UPDATE Producto SET Stock = Stock - @Cantidad WHERE IdProducto = @IdProducto",
                    db, transaction);
                comandoDetalle.Parameters.AddWithValue("@IdVenta", idVenta);
                comandoDetalle.Parameters.AddWithValue("@IdProducto", item.id);
                comandoDetalle.Parameters.AddWithValue("@Cantidad", item.cantidad);
                comandoDetalle.Parameters.AddWithValue("@Precio", item.precio);
                await comandoDetalle.ExecuteNonQueryAsync();
            }
            await transaction.CommitAsync();  
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
    }
}