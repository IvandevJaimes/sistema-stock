public static class Data
{
    private static List<Sucursal> cache = null!;
    public static List<Sucursal> GetData()
    {
        if (cache == null)
        {
            cache = new List<Sucursal>
            {
                new Sucursal("Centro", "25 de Mayo, San Miguel de Tucumán", new List<Producto>
                {
                    new Televisor(88365, "Televisor Phillips", 180000, 25, 22, "LCD"),
                    new Televisor(26792, "Televisor Samsung", 130000, 10, 58, "oled"),
                    new Televisor(89547, "Televisor LG", 2000000, 10, 40, "Amoled"),
                    new Heladera(91871, "Heladera Samsung", 180000, 5 , 319, "No frost"),
                    new Lavarropa(59738, "Lavarropa Dream", 800000, 40, 10, "automatico")
                }),
                new Sucursal("Norte", "av. sarmiento 550, San Miguel de Tucumán", new List<Producto>
                {
                    new Televisor(67349, "Televisor Philco", 500000, 30, 40, "LCD"),
                    new Televisor(71398, "Televisor Noblex", 620000, 5, 55, "LCD"),
                    new Heladera(87824, "Heladera Gafa", 900000, 3, 282, "No frost"),
                    new Lavarropa(90023, "Lavarropa Gadnic", 514000, 8, 75, "Automatico")
                })
            };
        }
        return cache;
    }
}