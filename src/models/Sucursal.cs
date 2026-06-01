//modelo para representar una sucursal 
public class Sucursal
{
    public Sucursal(
        int IdSucursal = 0,
        string Nombre = "",
        string Direccion = "",
        List<Producto>? Stock = null,
        List<VentaDetalle>? Ventas = null 
    )
    {
        nombre = Nombre;
        idSucursal = IdSucursal;
        direccion = Direccion;
        stock = Stock ?? new List<Producto>();
        ventas = Ventas ?? new List<VentaDetalle>();
    }
    public string nombre { get; } 
    public int idSucursal { get; }
    public string direccion { get; }
    public List<Producto> stock { get; }
    public List<VentaDetalle> ventas { get;}
    //atributos solo con getter para que no puedan ser modificados desde otra parte 
    
    public List<VentaDetalle> verVentas()
    {
        return ventas;
    }

}
