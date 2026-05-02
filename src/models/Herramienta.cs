public class Herramienta : Producto
{
    public Herramienta(string Nombre, decimal Precio, int Cantidad, string TipoAlimentacion, string TipoTrabajo, string Tipo)
        : base(Nombre, Precio, Cantidad, Tipo)
    {
        tipoAlimentacion = TipoAlimentacion;
        tipoTrabajo = TipoTrabajo;
    }
    public string tipoAlimentacion { get; set; }
    public string tipoTrabajo { get; set; }
}
