public static class Data
{
    private static List<Sucursal> _cache = null!;
    public static List<Sucursal> GetData()
    {
        if (_cache == null)
        {
            _cache = new List<Sucursal>
            {
                new Sucursal("Centro", "25 de Mayo, San Miguel de Tucumán", new List<Producto>
                {
                    new Herramienta("Martillo", 9000, 25, "manual", "golpe", "herramienta"),
                    new Herramienta("Pico", 34000.99m, 15, "manual", "excavacion", "herramienta"),
                    new Herramienta("Sierra Circular", 15500.50m, 10, "electrico", "corte", "herramienta"),
                    new MaterialInsumo("Caja de Clavos 1kg", 1200.50m, 50, "caja", "materialInsumo"),
                    new AccesorioEquipamiento("Pincel Professional", 850.50m, 40, "pintura", "accesorioEquipamiento")
                }),
                new Sucursal("Norte", "av. sarmiento 550, San Miguel de Tucumán", new List<Producto>
                {
                    new Herramienta("Martillo", 9000, 30, "manual", "golpe", "herramienta"),
                    new Herramienta("taladro 750 wats", 39000, 5, "electrico", "perforacion", "herramienta"),
                    new MaterialInsumo("Cemento Holcim 50kg", 450.50m, 80, "kg", "materialInsumo"),
                    new AccesorioEquipamiento("Escalera Aluminio 5m", 12500, 10, "acceso", "accesorioEquipamiento")
                })
            };
        }
        return _cache;
    }
}