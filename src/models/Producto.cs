// clase padre para representar un producto. es abstracta para evitar su instanciación directa y forzar a usar las clases hijas que representan cada tipo de producto con sus atributos específicos
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
    public int id { get; set; }
    public int codigo { get; private set; }
    public string nombre { get; private set; }
    public decimal precio { get; private set; }
    public int cantidad { get; private set; }


    public abstract string ObtenerDetalles(); // metodo abstracto para que las clases hijas lo implementen segun sus atributos especificos 
    public decimal calcularValor() // metodo para calcular el valor total del producto segun su precio y cantidad
    {
        return precio * cantidad;
    }
    public decimal calcularIngresos(int cantidadVendida) // metodo para calcular los ingresos generados por la venta 
    {
        return cantidadVendida * precio;
    }

}
