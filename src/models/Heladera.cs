public class Heladera : Producto
{
    public Heladera(int Id, int Codigo, string Nombre, decimal Precio, int Cantidad, double Capacidad, string Tipo)
        : base(Id, Codigo, Nombre, Precio, Cantidad) 
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