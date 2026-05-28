public class ProductoController
{
    private ProductoService productoService = new ProductoService();

    private SucursalService sucursalService = new SucursalService();

    public Result<Producto> CrearProducto(string sucursalNombre, string tipo, string nombre, decimal precio, int cantidad, double extra1, string extra2 = "")
    {
        var nuevoProducto = productoService.PostProducto(sucursalNombre, tipo, nombre, precio, cantidad, extra1, extra2);
        return nuevoProducto;
    }

    public Result<Producto> ActualizarProducto(string sucursalNombre, int id, string nombre, decimal precio, int cantidad)
    {
        var productoActualizar = productoService.PutProducto(sucursalNombre, id, nombre, precio, cantidad);
        return productoActualizar;
    }

    public Result<Producto> EliminarProducto(string sucursalNombre, int id)
    {
        var productoEliminar = productoService.DeleteProducto(sucursalNombre, id);
        return productoEliminar;
    }

    public Result<decimal> VenderProducto(string sucursalNombre, int id, int cantidad)
    {
        var productoVendido = productoService.VenderProducto(sucursalNombre, id, cantidad);
        return productoVendido;
    }

    public List<Producto> ListarProductos(string sucursalNombre)
    {
        var sucursal = sucursalService.VerSucursal(sucursalNombre);
        if (sucursal == null) return new List<Producto>();

        var productosObtenidos = productoService.GetProductos(sucursal);
        return productosObtenidos;
    }

    public Result<List<Producto>> BuscarProducto(string sucursalNombre, string productoNombre)
    {
        var productos = productoService.BuscarProducto(sucursalNombre, productoNombre);
        return productos;
    }

} 