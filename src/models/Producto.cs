public abstract class Producto
{
    public Producto(string Id, string Nombre, decimal Precio, int Cantidad)
    {
        id = Id;
        nombre = Nombre;
        precio = Precio;
        cantidad = Cantidad;
    }
    public string id { get; private set; }
    public string nombre { get; private set; }
    public decimal precio { get; private set; }
    public int cantidad { get; private set; }

    public void vender(int cantidadVendida)
    {
        if (cantidadVendida <= 0) throw new Exception("Cantidad inválida");
        if (cantidad < cantidadVendida) throw new Exception("Stock insuficiente");
        cantidad -= cantidadVendida;
    }

    public decimal calcularValor()
    {
        return precio * cantidad;
    }
    public decimal calcularIngresos(int cantidadVendida)
    {
        return cantidadVendida * precio;
    }

}
