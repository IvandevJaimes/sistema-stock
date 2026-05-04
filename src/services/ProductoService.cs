public class ProductoService
{
    private SucursalService sucursalService = new SucursalService();

    public List<Producto> GetProductos(Sucursal sucursal)
    {
        return new List<Producto>(sucursal.stock);
    }

    public Result<Producto> PostProducto(string sucursalNombre, string tipo, string nombre, decimal precio, int cantidad, string extra1 = "", string extra2 = "")
    {
        var sucursal = sucursalService.VerSucursal(sucursalNombre);
        if (sucursal == null) return Result<Producto>.Error("Sucursal no encontrada");

        var id = GenerarID.RandomID();

        Producto? nuevo = tipo switch
        {
            "herramienta" => new Herramienta(id, nombre, precio, cantidad, extra1, extra2),
            "materialInsumo" => new MaterialInsumo(id, nombre, precio, cantidad, extra1),
            "accesorioEquipamiento" => new AccesorioEquipamiento(id, nombre, precio, cantidad, extra1),
            _ => null
        };
        if (nuevo == null) return Result<Producto>.Error("Tipo de producto inválido");

        sucursal.AgregarProducto(nuevo);
        return Result<Producto>.Ok(nuevo);
    }
    public Result<Producto> PutProducto(string sucursalNombre, int id, string nombre, decimal precio, int cantidad)
    {
        var sucursal = sucursalService.VerSucursal(sucursalNombre);
        if (sucursal == null) return Result<Producto>.Error("Sucursal no encontrada");
        var producto = sucursal.stock.Find(p => p.id == id);
        if (producto == null) return Result<Producto>.Error("Producto no encontrado en la sucursal");

        producto.ActualizarProducto(nombre, precio, cantidad);
        return Result<Producto>.Ok(producto);

    }

    public Result<Producto> DeleteProducto(string sucursalNombre, int id)
    {
        var sucursal = sucursalService.VerSucursal(sucursalNombre);
        if (sucursal == null) return Result<Producto>.Error("Sucursal no encontrada");
        var producto = sucursal.stock.Find(p => p.id == id);
        if (producto == null) return Result<Producto>.Error("Producto no encontrado");

        sucursal.BorrarProducto(producto);
        return Result<Producto>.Ok(producto);
    }

    public Result<decimal> VenderProducto(string sucursalNombre, int id, int cantidad)
    {
        var sucursal = sucursalService.VerSucursal(sucursalNombre);
        if (sucursal == null) return Result<decimal>.Error("Sucursal no encontrada");

        var producto = sucursal.stock.Find(p => p.id == id);
        if (producto == null) return Result<decimal>.Error("Producto no encontrado");

        if (producto.cantidad < cantidad) return Result<decimal>.Error("Stock insuficiente");

        producto.vender(cantidad);
        var ingresos = producto.calcularIngresos(cantidad);
        return Result<decimal>.Ok(ingresos);
    }

    public Result<List<Producto>> BuscarProducto(string sucursalNombre, string nombre)
    {
        var sucursal = sucursalService.VerSucursal(sucursalNombre);
        if (sucursal == null)
            return Result<List<Producto>>.Error("Sucursal no encontrada");

        var productos = sucursal.stock
            .Where(p => p.nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (!productos.Any())
            return Result<List<Producto>>.Error("Producto no encontrado");
        return Result<List<Producto>>.Ok(productos);
    }
}
