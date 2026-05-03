public abstract class Producto
{
    public Producto(int Id, string Nombre, decimal Precio, int Cantidad)
    {
        id = Id;
        nombre = Nombre;
        precio = Precio;
        cantidad = Cantidad;
    }
    public int id { get; private set; }
    public string nombre { get; private set; }
    public decimal precio { get; private set; }
    public int cantidad { get; private set; }


    public abstract string ObtenerDetalles();

    public int vender (int cantidadVendida)
    {
        return cantidad -= cantidadVendida;
    }
    public decimal calcularValor()
    {
        return precio * cantidad;
    }
    public decimal calcularIngresos(int cantidadVendida)
    {
        return cantidadVendida * precio;
    }

    public void ActualizarProducto(string? Nombre = null, decimal? Precio = null, int? Cantidad = null)
    {
        if (Nombre != null) nombre = Nombre;
        if (Precio != null) precio = Precio ?? precio;
        if (Cantidad != null) cantidad = Cantidad ?? cantidad;
    }
}
