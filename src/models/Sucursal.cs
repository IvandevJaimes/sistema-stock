public class Sucursal
{
    public Sucursal(
        string Nombre = "",
        string Direccion = "",
        List<Producto>? Stock = null,
        List<CarritoItem>? Ventas = null 
    )
    {
        nombre = Nombre;
        direccion = Direccion;
        stock = Stock ?? new List<Producto>();
        ventas = Ventas ?? new List<CarritoItem>();
    }
    public string nombre { get; }
    public string direccion { get; }

    public List<CarritoItem> ventas { get; private set; }
    public List<Producto> stock { get; private set; }

    public void AgregarProducto(Producto producto)
    {
        stock.Add(producto);
    }
    public void BorrarProducto(Producto producto)
    {
        stock.Remove(producto);
    }

    public void RegistrarVentas(List<CarritoItem> items)
    {
        foreach (var item in items)
            ventas.Add(item);
    }
}
