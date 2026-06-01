//capa controller, se encarga de recibir las solicitudes de la capa interfaz, procesar los datos necesarios y enviar la respuesta correspondiente
public class SucursalController
{
    private SucursalService sucursalService = new SucursalService();

    public async Task<Result<List<Sucursal>?>> ListarSucursales()
    {
        try
        {
            var sucursales = await sucursalService.VerSucursales();
            if (sucursales == null) return Result<List<Sucursal>?>.Error("No se encontraron sucursales."); //verificar que se hayan obtenido sucursales, si no devolver el resultado con error

            return Result<List<Sucursal>?>.Ok(sucursales);// devolver el resultado con la lista de sucursales obtenida
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
            // verificar que el nombre de la sucursal no esté vacío, si lo está devolver el resultado con error
            var sucursal = await sucursalService.VerSucursal(sucursalNombre);

            if (sucursal == null) return Result<Sucursal?>.Error("Sucursal no encontrada."); //verificar que se haya encontrado la sucursal, si no devolver el resultado con error
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
            
            return Result<List<VentaDetalle>?>.Ok(ventas); //devolver el resultado con la lista de ventas obtenida, puede ser null si no se encontraron ventas para esa sucursal, pero no es un error ya que la sucursal existe y se pudo obtener su lista de ventas aunque esta esté vacía
        }
        catch (Exception ex)
        {
            return Result<List<VentaDetalle>?>.Error(ex.Message);
        }
    }
}