public class Heladera : Producto
{
    public Heladera(int Id, string Nombre, decimal Precio, int Cantidad, double Capacidad, string Tipo)
        : base(Id, Nombre, Precio, Cantidad) 
    {
        capacidad = Capacidad;
        tipo = Tipo;
    }
    public double capacidad { get; protected set; }
    public string tipo { get; protected set; }

    public override string ObtenerDetalles()
    {
        return $"Capacidad: {capacidad}L | Tipo: {tipo}";
    }
}