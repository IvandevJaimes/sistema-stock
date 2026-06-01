public class ProductoController
{
    private ProductoService productoService = new ProductoService();

    private SucursalController sucursalController = new SucursalController();

    public async Task<Result<Producto>> CrearProducto(int codigo, string sucursalNombre, string tipo, string nombre, decimal precio, int cantidad, double extra1, string extra2 = "")
    {
        try
        {
            if (codigo <= 0) return Result<Producto>.Error("El código del producto debe ser mayor a cero.");
            if (codigo > 9999) return Result<Producto>.Error("El código del producto no puede ser mayor a 9999.");

            if (string.IsNullOrEmpty(sucursalNombre)) return Result<Producto>.Error("El nombre de la sucursal no puede estar vacío.");

            if (string.IsNullOrEmpty(tipo)) return Result<Producto>.Error("El tipo de producto no puede estar vacío.");

            if (string.IsNullOrEmpty(nombre)) return Result<Producto>.Error("El nombre del producto no puede estar vacío.");

            if (precio <= 0) return Result<Producto>.Error("El precio debe ser mayor a cero.");
            if (cantidad < 0) return Result<Producto>.Error("La cantidad no puede ser negativa.");

            var nuevoProducto = await productoService.PostProducto(codigo, sucursalNombre, tipo, nombre, precio, cantidad, extra1, extra2);
            return Result<Producto>.Ok(nuevoProducto);
        }
        catch (Exception ex)
        {
            return Result<Producto>.Error(ex.Message);
        }


    }

    public async Task<Result<Producto>> ActualizarProducto(int idProducto, string sucursalNombre, string? nombre = null, decimal? precio = null, int? cantidad = null, double? extra1 = null, string? extra2 = null)
    {
        try
        {
            if (idProducto <= 0) return Result<Producto>.Error("El ID del producto debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(sucursalNombre)) return Result<Producto>.Error("El nombre de la sucursal no puede estar vacío.");

            if (nombre != null && string.IsNullOrWhiteSpace(nombre)) return Result<Producto>.Error("El nombre no puede estar vacío.");

            if (precio != null && precio <= 0) return Result<Producto>.Error("El precio debe ser mayor a cero.");
            if (cantidad != null && cantidad < 0) return Result<Producto>.Error("La cantidad no puede ser negativa.");

            var productoActualizar = await productoService.PutProducto(idProducto, sucursalNombre, nombre, precio, cantidad, extra1, extra2);
            return Result<Producto>.Ok(productoActualizar);
        }
        catch (Exception ex)
        {
            return Result<Producto>.Error(ex.Message);
        }
    }

    public async Task<Result<Producto>> EliminarProducto(string sucursalNombre, int id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(sucursalNombre)) return Result<Producto>.Error("El nombre de la sucursal no puede estar vacío.");
            if (id <= 0) return Result<Producto>.Error("El ID del producto debe ser mayor a cero.");
            var productoEliminar = await productoService.DeleteProducto(sucursalNombre, id);
            return Result<Producto>.Ok(productoEliminar);

        }
        catch (Exception ex)
        {
            return Result<Producto>.Error(ex.Message);
        }

    }

    public async Task<Result<decimal>> VenderProducto(string sucursalNombre, List<CarritoItem> items)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(sucursalNombre)) return Result<decimal>.Error("El nombre de la sucursal no puede estar vacío.");
            if (items == null || items.Count == 0) return Result<decimal>.Error("No se han agregado productos al carrito");

            await productoService.RegistrarVenta(sucursalNombre, items);

            return Result<decimal>.Ok(items.Sum(i => i.cantidad * i.precio));
        }
        catch (Exception ex)
        {
            return Result<decimal>.Error(ex.Message);
        }
    }

    public async Task<Result<List<Producto>>> ListarProductos(string sucursalNombre)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(sucursalNombre)) return Result<List<Producto>>.Error("El nombre de la sucursal no puede estar vacío.");
            var productosObtenidos = await productoService.GetProductos(sucursalNombre);
            return Result<List<Producto>>.Ok(productosObtenidos);
        }
        catch (Exception ex)
        {
            return Result<List<Producto>>.Error(ex.Message);
        }

    }

    public async Task<Result<List<Producto>>> BuscarProductoPorNombre(string sucursalNombre, string productoNombre)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(sucursalNombre)) return Result<List<Producto>>.Error("El nombre de la sucursal no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(productoNombre)) return Result<List<Producto>>.Error("El nombre del producto no puede estar vacío.");

            var productos = await productoService.BuscarProductoPorNombre(sucursalNombre, productoNombre);
            return Result<List<Producto>>.Ok(productos);
        }
        catch (Exception ex)
        {
            return Result<List<Producto>>.Error(ex.Message);
        }
    }

    public async Task<Result<List<Producto>>> BuscarProductoPorCodigo(string sucursalNombre, int productoCodigo)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(sucursalNombre)) return Result<List<Producto>>.Error("El nombre de la sucursal no puede estar vacío.");
            if (productoCodigo <= 0) return Result<List<Producto>>.Error("El código del producto debe ser mayor a cero.");

            var productos = await productoService.BuscarProductoPorCodigo(sucursalNombre, productoCodigo);
            return Result<List<Producto>>.Ok(productos);
        }
        catch (Exception ex)
        {
            return Result<List<Producto>>.Error(ex.Message);
        }
    }
}