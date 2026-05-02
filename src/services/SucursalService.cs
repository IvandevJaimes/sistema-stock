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

}
