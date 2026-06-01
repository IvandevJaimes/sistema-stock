using MySqlConnector;
public class SucursalQuerys
{
    private ConexionDB conexionDB = new ConexionDB();

    public async Task<List<Sucursal>> GetSucursales()
    {
        var consulta = "SELECT * FROM Sucursal";
        var db = conexionDB.AbrirConexion();
        var sucursales = new List<Sucursal>();
        try
        {
           var commando = new MySqlCommand(consulta, db);
           var reader = await commando.ExecuteReaderAsync();
           while (await reader.ReadAsync())
           {
               var id = reader.GetInt32("IdSucursal");
               var nombre = reader.GetString("Nombre");
               sucursales.Add(new Sucursal(id, nombre, "", new List<Producto>(), new List<VentaDetalle>()));
           }
           reader.Close();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener las sucursales: " + ex.Message);
        }
        finally
        {
            db.Close();
        }
        return sucursales;
    }   

}