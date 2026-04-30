public class AccesorioEquipamiento : Producto
{
    public AccesorioEquipamiento(string Id, string Nombre, decimal Precio, int Cantidad, string Uso)
        : base(Id, Nombre, Precio, Cantidad)
    {
        uso = Uso;
    }
    public string uso { get; set; }
}
