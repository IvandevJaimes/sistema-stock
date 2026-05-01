public class SucurSalService
{
    public Sucursal ReadSucursal(string sucursal)
    {
        var sucursales = Data.GetData();

        foreach (var s in sucursales)
        {
            if (sucursal == s.nombre)
            {
                return s;
            }
        }
        throw new Exception("No se encontro la sucursal");
    }
}

public class ProductoService
{
    public List<Producto> GetProductos(Sucursal sucursal)
    {
        foreach (var producto in sucursal.stock)
        {
            Console.WriteLine(producto.nombre);
        }
        return sucursal.stock;
    }
}
