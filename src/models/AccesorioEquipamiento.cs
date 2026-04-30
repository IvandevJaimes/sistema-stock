public class AccesorioEquipamiento : Producto
{
    public AccesorioEquipamiento(string Item_id, string Nombre, decimal Precio, int Cantidad, string Uso)
        : base(Item_id, Nombre, Precio, Cantidad)
    {
        uso = Uso;
    }
    public string uso { get; set; }
}
