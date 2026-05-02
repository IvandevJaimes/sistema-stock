public class AccesorioEquipamiento : Producto
{
    public AccesorioEquipamiento(string Nombre, decimal Precio, int Cantidad, string Uso, string Tipo)
        : base(Nombre, Precio, Cantidad, Tipo)
    {
        uso = Uso;
    }
    public string uso { get; set; }
}
