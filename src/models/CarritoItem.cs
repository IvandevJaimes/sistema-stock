//modelo para representar un item del carrito de compras, con su id, nombre, cantidad y precio
public class CarritoItem
{

    public int id { get; set; }
    public string nombre { get; set; } = "";
    public int cantidad { get; set; }
    public decimal precio { get; set; }

}