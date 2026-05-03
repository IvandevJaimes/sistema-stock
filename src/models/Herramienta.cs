public class Herramienta : Producto
{
    public Herramienta(int Id, string Nombre, decimal Precio, int Cantidad, string TipoAlimentacion, string TipoTrabajo)
        : base(Id, Nombre, Precio, Cantidad)
    {
        tipoAlimentacion = TipoAlimentacion;
        tipoTrabajo = TipoTrabajo;
    }
    public string tipoAlimentacion { get; protected set; }
    public string tipoTrabajo { get; protected set; }
    public override string ObtenerDetalles()
    {
        return $"Tipo: Herramienta | Alimentación: {tipoAlimentacion} | Trabajo: {tipoTrabajo}";
    }
}