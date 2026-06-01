public class SucursalService
{
    private SucursalQuerys sucursalQuerys = new SucursalQuerys();
    private VentasQuerys ventas = new VentasQuerys();
    public async Task<List<Sucursal>> VerSucursales()
    {
        var sucursales = new List<Sucursal>();
        try
        {
            var resultados = sucursalQuerys.GetSucursales();
            sucursales = await resultados;
            if (sucursales == null || sucursales.Count == 0)
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
            return sucursal;
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
            return await ventas.GetVentas(sucursal.idSucursal);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
      
    }

}
