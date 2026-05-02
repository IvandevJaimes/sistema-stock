public static class GenerarID
{
    private static readonly Random _random = new Random();
    
    public static int RandomID()
    {
        return _random.Next(10000, 99999);
    }
}