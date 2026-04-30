public class SucurSalService
{   
    public Sucursal ReadSucursal(string sucursal = "")
    {
        var sucursales = Data.GetData();

        foreach (var s in sucursales)
        {
            if (sucursal == s.nombre)
            {
                return s;
            }
        }
        throw new Exception("No se encontro la sucursal");
    }
}

