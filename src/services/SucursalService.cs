//Capa de servicios para sucursales, encargada de manejar solo la logica de negocio usando las querys
public class SucursalService
{
    private SucursalQuerys sucursalQuerys = new SucursalQuerys(); // instanciar querys para usarla solo en esta clase
    private VentasQuerys ventas = new VentasQuerys();
    public async Task<List<Sucursal>> VerSucursales()
    {
        var sucursales = new List<Sucursal>(); //devuelve lista de sucursales
        try
        {
            var resultados = sucursalQuerys.GetSucursales();
            sucursales = await resultados;
            if (sucursales == null || sucursales.Count == 0) //verificar que se hayan obtenido sucursales, si no lanzar una excepcion
            {
                throw new Exception("No se encontraron sucursales");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return sucursales;
    }
    public async Task<Sucursal?> VerSucursal(string sucursalNombre)
    {
        try
        {
            var sucursales = await VerSucursales();
            var sucursal = sucursales.Find(s => s.nombre == sucursalNombre);
            if (sucursal == null) throw new Exception("Sucursal no encontrada");
            return sucursal; //devuelve la sucursal encontrada o null si no se encuentra
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

   
    public async Task<List<VentaDetalle>?> MostrarVentas(string sucursalNombre)
    {
        try
        {
            var sucursal = await VerSucursal(sucursalNombre);
            if (sucursal == null)
            {
                throw new Exception("Sucursal no encontrada");
            }
            return await ventas.GetVentas(sucursal.idSucursal); //devuelve la lista de ventas de la sucursal encontrada o null si no se encuentra
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
      
    }

}
