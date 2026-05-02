using System.Linq;

public class SucursalService
{
    private List<Sucursal> sucursales = Data.GetData();
    public Sucursal VerSucursal(string sucursal)
    {
        
        foreach (var s in sucursales)
        {
            if (sucursal == s.nombre)
            {
                return s;
            }
        }
        return null!;
    }
}

public class ProductoService
{
    public List<Producto> GetProductos(Sucursal sucursal)
    {
        return new List<Producto>(sucursal.stock);
    }

    public Producto? PostProducto(string sucursalNombre, string tipo, string nombre, decimal precio, int cantidad, string extra1 = "", string extra2 = "")
    {
        var sucursalService = new SucursalService();
        var sucursal = sucursalService.VerSucursal(sucursalNombre);
        
        var id = GenerarID.RandomID();

        Producto nuevo = tipo switch
        {
            "herramienta" => new Herramienta(id, nombre, precio, cantidad, extra1, extra2, tipo),
            "materialInsumo" => new MaterialInsumo(id, nombre, precio, cantidad, extra1, tipo),
            "accesorioEquipamiento" => new AccesorioEquipamiento(id, nombre, precio, cantidad, extra1, tipo),
            _ => null!
        };
        
        sucursal.AgregarProducto(nuevo);
        return nuevo;
    }

    public Producto? PutProducto(string sucursalNombre, int id, string nombre, decimal precio, int cantidad)
    {
        var sucursalService = new SucursalService();
        var sucursal = sucursalService.VerSucursal(sucursalNombre);
        var producto = sucursal.stock.FirstOrDefault(p => p.id == id);
        if (producto == null) return null;
        producto.ActualizarProducto(nombre, precio, cantidad);
        return producto;
    }
}
