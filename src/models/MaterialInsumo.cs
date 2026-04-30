public class MaterialInsumo : Producto
{
    public MaterialInsumo(string Item_id, string Nombre, decimal Precio, int Cantidad, string UnidadMedida)
        : base(Item_id, Nombre, Precio, Cantidad)
    {
        unidadMedida = UnidadMedida;
    }
    public string unidadMedida { get; set; }
}
