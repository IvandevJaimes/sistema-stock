public class SucursalController
{
    private SucursalService sucursalService = new SucursalService();

    public List<Sucursal> ListarSucursales()
    {
        var sucursales = sucursalService.VerSucursales();
        return sucursales;
    }

    public Sucursal? ObtenerSucursal(string sucursalNombre)
    {
        var sucursal = sucursalService.VerSucursal(sucursalNombre);
        return sucursal;
    }

    public void RegistrarVenta(string sucursalNombre, List<CarritoItem> item)
    {
        sucursalService.RegistrarVenta(sucursalNombre, item);
    }
    public List<CarritoItem>? MostrarVentas(string sucursalNombre)
    {
        var ventas = sucursalService.MostrarVentas(sucursalNombre);
        return ventas;
    }
}