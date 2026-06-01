public abstract class Producto
{
    public Producto(int Id, int Codigo, string Nombre, decimal Precio, int Cantidad)
    {
        id = Id;
        codigo = Codigo;
        nombre = Nombre;
        precio = Precio;
        cantidad = Cantidad;
    }
    public int id { get; private set; }
    public int codigo { get; private set; }
    public string nombre { get; private set; }
    public decimal precio { get; private set; }
    public int cantidad { get; private set; }


    public abstract string ObtenerDetalles();
    public decimal calcularValor()
    {
        return precio * cantidad;
    }
    public decimal calcularIngresos(int cantidadVendida)
    {
        return cantidadVendida * precio;
    }

}
