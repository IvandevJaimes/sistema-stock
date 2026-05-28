public class Lavarropa : Producto
{
    public Lavarropa(int Id, string Nombre, decimal Precio, int Cantidad, double Carga, string Tipo)
        : base(Id, Nombre, Precio, Cantidad)
    {
        carga = Carga;
        tipo = Tipo;
    }
    public double carga { get; private set; }
    public string tipo { get; private set; }
    public override string ObtenerDetalles()
    {
        return $"Carga: {carga} | Tipo: {tipo}";
    }
}