public static class Data
{
    public static List<Sucursal> getData()
    {
        return new List<Sucursal>
        {
            new Sucursal("Centro", "25 de Mayo, San Miguel de Tucumán", new List<Producto>
            {
                new Herramienta("p001", "Martillo", 9000, 25, "manual", "golpe"),
                new Herramienta("p002", "Pico", 34000.99m, 15, "manual", "excavacion"),
                new Herramienta("p003", "Sierra Circular", 15500.50m, 10, "electrico", "corte"),
                new MaterialInsumo("p006", "Caja de Clavos 1kg", 1200.50m, 50, "caja"),
                new AccesorioEquipamiento("p009", "Pincel Professional", 850.50m, 40, "pintura")
            }),
            new Sucursal("Norte", "av. sarmiento 550, San Miguel de Tucumán", new List<Producto>
            {
                new Herramienta("p001", "Martillo", 9000, 30, "manual", "golpe"),
                new Herramienta("p013", "taladro 750 wats", 39000, 5, "electrico", "perforacion"),
                new MaterialInsumo("p012", "Cemento Holcim 50kg", 450.50m, 80, "kg"),
                new AccesorioEquipamiento("p010", "Escalera Aluminio 5m", 12500, 10, "acceso")
            })
        };
    }
}
