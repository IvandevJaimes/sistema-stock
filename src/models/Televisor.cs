public class Televisor : Producto
{
    public Televisor(int Id, string Nombre, decimal Precio, int Cantidad, double Pulgadas, string TipoPantalla)
        : base(Id, Nombre, Precio, Cantidad)
    {
        pulgadas = Pulgadas;
        tipoPantalla = TipoPantalla;
    }
    public double pulgadas{ get; protected set; }
    public string tipoPantalla{ get; protected set; }
    public override string ObtenerDetalles()
    {
        return $"Pulgadas: {pulgadas} | Tipo de pantalla: {tipoPantalla}" ;
    }
}