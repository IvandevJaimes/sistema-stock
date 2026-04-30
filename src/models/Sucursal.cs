public class Sucursal
{
    public Sucursal(
        string Nombre = "",
        string Direccion = "",
        List<Producto>? Stock = null
    )
    {
        nombre = Nombre;
        direccion = Direccion;
        stock = Stock ?? new List<Producto>();
    }

    public string nombre { get;}
    public string direccion { get;} 
    public List<Producto> stock { get; set; } 
}
