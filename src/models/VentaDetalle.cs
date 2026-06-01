//modelo para representar el detalle de una venta
public class VentaDetalle
{
    public int idVenta { get; set; }
    public DateTime fecha { get; set; }
    public string nombreProducto { get; set; } = "";
    public int cantidad { get; set; }
    public decimal precioUnitario { get; set; }
}