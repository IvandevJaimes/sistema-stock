public class Herramienta : Producto
{
    public Herramienta(string Id, string Nombre, decimal Precio, int Cantidad, string TipoAlimentacion, string TipoTrabajo)
        : base(Id, Nombre, Precio, Cantidad)
    {
        tipoAlimentacion = TipoAlimentacion;
        tipoTrabajo = TipoTrabajo;
    }
    public string tipoAlimentacion { get; set; }
    public string tipoTrabajo { get; set; }
}
