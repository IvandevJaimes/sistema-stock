
public class SucursalController
{
    private SucursalService sucursalService = new SucursalService();

    public async Task<Result<List<Sucursal>?>> ListarSucursales()  
    {
        try
        {
            var sucursales = await sucursalService.VerSucursales();  
            if (sucursales == null) return Result<List<Sucursal>?>.Error("No se encontraron sucursales.");
            return Result<List<Sucursal>?>.Ok(sucursales);
        }
        catch (Exception ex)
        {
            return Result<List<Sucursal>?>.Error(ex.Message);
        }
    }

    public async Task<Result<Sucursal?>> ObtenerSucursal(string sucursalNombre)
    {
        try
        {
            if (string.IsNullOrEmpty(sucursalNombre)) return Result<Sucursal?>.Error("El nombre de la sucursal no puede estar vacío.");

            var sucursal = await sucursalService.VerSucursal(sucursalNombre);

            if (sucursal == null) return Result<Sucursal?>.Error("Sucursal no encontrada.");
            return Result<Sucursal?>.Ok(sucursal);
        }
        catch (Exception ex)
        {
            return Result<Sucursal?>.Error(ex.Message);
        }
    }

    public async Task<Result<List<VentaDetalle>?>> MostrarVentas(string sucursalNombre)
    {
        try
        {
            if (string.IsNullOrEmpty(sucursalNombre)) return Result<List<VentaDetalle>?>.Error("El nombre de la sucursal no puede estar vacío.");
            var ventas = await sucursalService.MostrarVentas(sucursalNombre);
            return Result<List<VentaDetalle>?>.Ok(ventas);
        }
        catch (Exception ex)
        {
            return Result<List<VentaDetalle>?>.Error(ex.Message);
        }
    }
}