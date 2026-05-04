public class Herramienta : Producto
{
    public Herramienta(int Id, string Nombre, decimal Precio, int Cantidad, string TipoAlimentacion, string TipoTrabajo)
        : base(Id, Nombre, Precio, Cantidad)
    {
        tipoAlimentacion = TipoAlimentacion;
        tipoTrabajo = TipoTrabajo;
    }
    public string tipoAlimentacion { get; private set; }
    public string tipoTrabajo { get; private set; }
    public override string ObtenerDetalles()
    {
        return $"Alimentación: {tipoAlimentacion} | Trabajo: {tipoTrabajo}";
    }
}