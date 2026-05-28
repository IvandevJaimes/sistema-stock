public class SucursalService
{
    private List<Sucursal> sucursales = Data.GetData();

    public List<Sucursal> VerSucursales()
    {
        return sucursales;
    }
    public Sucursal? VerSucursal(string sucursalNombre)
    {
        var sucursal = sucursales.Find(s => s.nombre == sucursalNombre);
        if (sucursal == null) return null;
        return sucursal;
    }

    public void RegistrarVenta(string sucursalNombre, List<CarritoItem> item)
    {
        var sucursal = sucursales.Find(s => s.nombre == sucursalNombre);
        if (sucursal != null)
        {
            sucursal.RegistrarVentas(item);
        }
    }
    public List<CarritoItem>? MostrarVentas(string sucursalNombre)
    {
        var sucursal = sucursales.Find(s => s.nombre == sucursalNombre);
        return sucursal?.ventas;
    }

}
