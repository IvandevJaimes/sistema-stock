public class MaterialInsumo : Producto
{
    public MaterialInsumo(string Nombre, decimal Precio, int Cantidad, string UnidadMedida, string Tipo)
        : base(Nombre, Precio, Cantidad, Tipo)
    {
        unidadMedida = UnidadMedida;
    }
    public string unidadMedida { get; set; }
}
