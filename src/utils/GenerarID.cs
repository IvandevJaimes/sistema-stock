public static class GenerarID
{
    private static Random random = new Random();
    
    public static int RandomID()
    {
        return random.Next(10000, 99999);
    }
}