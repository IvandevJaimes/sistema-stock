public class Lavarropas : Producto
{
    public Lavarropas(int Id, int Codigo, string Nombre, decimal Precio, int Cantidad, double Carga, string Tipo)
        : base(Id, Codigo, Nombre, Precio, Cantidad)
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