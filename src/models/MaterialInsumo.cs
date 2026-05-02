public class MaterialInsumo : Producto
{
    public MaterialInsumo(int Id, string Nombre, decimal Precio, int Cantidad, string UnidadMedida, string Tipo)
        : base(Id, Nombre, Precio, Cantidad, Tipo)
    {
        unidadMedida = UnidadMedida;
    }
    public string unidadMedida { get; set; }
}
