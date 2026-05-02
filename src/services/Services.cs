public class SucursalService
{
    private List<Sucursal> sucursales = Data.GetData();

    public List<Sucursal> VerSucursales()
    {
        return sucursales;
    }
    public Sucursal? VerSucursal(string sucursalNombre)
    {
        var sucursal = sucursales.Find(s => s.nombre == sucursalNombre);
        if (sucursal == null) return null;
        return sucursal;
    }

}

public class ProductoService
{
    public List<Producto> GetProductos(Sucursal sucursal)
    {
        return new List<Producto>(sucursal.stock);
    }

    public Result<Producto> PostProducto(string sucursalNombre, string tipo, string nombre, decimal precio, int cantidad, string extra1 = "", string extra2 = "")
    {
        var sucursal = new SucursalService().VerSucursal(sucursalNombre);
        if (sucursal == null) return Result<Producto>.Error("Sucursal no encontrada");

        var id = GenerarID.RandomID();

        Producto? nuevo = tipo switch
        {
            "herramienta" => new Herramienta(id, nombre, precio, cantidad, extra1, extra2, tipo),
            "materialInsumo" => new MaterialInsumo(id, nombre, precio, cantidad, extra1, tipo),
            "accesorioEquipamiento" => new AccesorioEquipamiento(id, nombre, precio, cantidad, extra1, tipo),
            _ => null
        };
        if (nuevo == null) return Result<Producto>.Error("Tipo de producto inválido");

        sucursal.AgregarProducto(nuevo);
        return Result<Producto>.Ok(nuevo);
    }
    public Result<Producto> PutProducto(string sucursalNombre, int id, string nombre, decimal precio, int cantidad)
    {
        var sucursal = new SucursalService().VerSucursal(sucursalNombre);
        if (sucursal == null) return Result<Producto>.Error("Sucursal no encontrada");
        var producto = sucursal.stock.Find(p => p.id == id);
        if (producto == null) return Result<Producto>.Error("Producto no encontrado en la sucursal");

        producto.ActualizarProducto(nombre, precio, cantidad);
        return Result<Producto>.Ok(producto);

    }

    public Result<Producto> DeleteProducto(string sucursalNombre, int id)
    {
        var sucursal = new SucursalService().VerSucursal(sucursalNombre);
        if (sucursal == null) return Result<Producto>.Error("Sucursal no encontrada");
        var producto = sucursal.stock.Find(p => p.id == id);
        if (producto == null) return Result<Producto>.Error("Producto no encontrado");

        sucursal.BorrarProducto(producto);
        return Result<Producto>.Ok(producto);
    }

    public Result<decimal> VenderProducto(string sucursalNombre, int id, int cantidad)
    {
        var sucursal = new SucursalService().VerSucursal(sucursalNombre);
        if (sucursal == null) return Result<decimal>.Error("Sucursal no encontrada");

        var producto = sucursal.stock.Find(p => p.id == id);
        if (producto == null) return Result<decimal>.Error("Producto no encontrado");

        if (producto.cantidad < cantidad) return Result<decimal>.Error("Stock insuficiente");

        producto.vender(cantidad);
        var ingresos = producto.calcularIngresos(cantidad);
        return Result<decimal>.Ok(ingresos);
    }
}
