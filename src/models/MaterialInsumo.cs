public class MaterialInsumo : Producto
{
    public MaterialInsumo(string Id, string Nombre, decimal Precio, int Cantidad, string UnidadMedida)
        : base(Id, Nombre, Precio, Cantidad)
    {
        unidadMedida = UnidadMedida;
    }
    public string unidadMedida { get; set; }
}
