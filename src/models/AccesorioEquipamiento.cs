public class AccesorioEquipamiento : Producto
{
    public AccesorioEquipamiento(int Id, string Nombre, decimal Precio, int Cantidad, string Uso)
        : base(Id, Nombre, Precio, Cantidad)
    {
        uso = Uso;
    }
    public string uso { get; protected set; }

    public override string ObtenerDetalles()
    {
        return $"Uso: {uso}";
    }
}