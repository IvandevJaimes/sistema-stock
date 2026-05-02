public class AccesorioEquipamiento : Producto
{
    public AccesorioEquipamiento(int Id, string Nombre, decimal Precio, int Cantidad, string Uso, string Tipo)
        : base(Id, Nombre, Precio, Cantidad, Tipo)
    {
        uso = Uso;
    }
    public string uso { get; set; }
}
