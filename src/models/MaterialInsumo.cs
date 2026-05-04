public class MaterialInsumo : Producto
{
    public MaterialInsumo(int Id, string Nombre, decimal Precio, int Cantidad, string UnidadMedida)
        : base(Id, Nombre, Precio, Cantidad) 
    {
        unidadMedida = UnidadMedida;
    }
    public string unidadMedida { get; protected set; }

    public override string ObtenerDetalles()
    {
        return $"Medida: {unidadMedida}";
    }
}