public abstract class Producto
{
    public Producto(string Nombre, decimal Precio, int Cantidad, string Tipo)
    {
        nombre = Nombre;
        precio = Precio;
        cantidad = Cantidad;
        tipo = Tipo;

    }
    public int id { get; private set; } = GenerarID.RandomID(); 
    public string nombre { get; private set; }
    public decimal precio { get; private set; }
    public int cantidad { get; private set; }
    public string tipo { get; private set; }
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
