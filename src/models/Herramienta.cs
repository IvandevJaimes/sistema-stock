public class Herramienta : Producto
{
    public Herramienta(string Item_id, string Nombre, decimal Precio, int Cantidad, string TipoAlimentacion, string TipoTrabajo)
        : base(Item_id, Nombre, Precio, Cantidad)
    {
        tipoAlimentacion = TipoAlimentacion;
        tipoTrabajo = TipoTrabajo;
    }
    public string tipoAlimentacion { get; set; }
    public string tipoTrabajo { get; set; }
}
