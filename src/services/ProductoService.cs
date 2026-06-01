public class ProductoService
{
    private SucursalService sucursalService = new SucursalService();

    private ProductosQuerys productosQuerys = new ProductosQuerys();

    private VentasQuerys ventas = new VentasQuerys();
    public async Task<List<Producto>> GetProductos(string sucursalNombre)
    {
        List<Producto> productos = new List<Producto>();
        try
        {
            var sucursal = await sucursalService.VerSucursal(sucursalNombre);
            if (sucursal == null) throw new Exception("Sucursal no encontrada");
            productos = await productosQuerys.GetProductos(sucursal.idSucursal);
            return productos;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Producto> PostProducto(int codigo, string sucursalNombre, string tipo, string nombre, decimal precio, int cantidad, double extra1, string extra2 = "")
    {

        try
        {
            var sucursal = await sucursalService.VerSucursal(sucursalNombre);
            if (sucursal == null) throw new Exception("Sucursal no encontrada");

            var productos = await GetProductos(sucursalNombre);
            var productoExistente = productos.Find(p => p.codigo == codigo);

            if (productoExistente != null) throw new Exception("Ya existe un producto con el mismo código en esta sucursal");

            var id = GenerarID.RandomID();

            Producto nuevo = tipo.ToLower() switch
            {
                "televisor" => new Televisor(id, codigo, nombre, precio, cantidad, extra1, extra2),
                "heladera" => new Heladera(id, codigo, nombre, precio, cantidad, extra1, extra2),
                "lavarropas" => new Lavarropas(id, codigo, nombre, precio, cantidad, extra1, extra2),
                _ => throw new Exception("Tipo de producto inválido")
            };

            await productosQuerys.CrearProducto(nuevo, sucursal.idSucursal);

            return nuevo;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    public async Task<Producto> PutProducto(int idProducto, string sucursalNombre, string? nombre = null, decimal? precio = null, int? cantidad = null, double? extra1 = null, string? extra2 = null)
    {
        try
        {
            var productos = await GetProductos(sucursalNombre);

            var producto = productos.Find(p => p.id == idProducto);
            if (producto == null) throw new Exception("Producto no encontrado");

            var tipo = producto.GetType().Name;
            Producto productoActualizado = tipo switch
            {
                "Televisor" => new Televisor(idProducto, producto.codigo,
                    nombre ?? producto.nombre,
                    precio ?? producto.precio,
                    cantidad ?? producto.cantidad,
                    extra1 ?? ((Televisor)producto).pulgadas,
                    extra2 ?? ((Televisor)producto).tipoPantalla),
                "Heladera" => new Heladera(idProducto, producto.codigo,
                    nombre ?? producto.nombre,
                    precio ?? producto.precio,
                    cantidad ?? producto.cantidad,
                    extra1 ?? ((Heladera)producto).capacidad,
                    extra2 ?? ((Heladera)producto).tipo),
                "Lavarropas" => new Lavarropas(idProducto, producto.codigo,
                    nombre ?? producto.nombre,
                    precio ?? producto.precio,
                    cantidad ?? producto.cantidad,
                    extra1 ?? ((Lavarropas)producto).carga,
                    extra2 ?? ((Lavarropas)producto).tipo),
                _ => throw new Exception("Tipo de producto inválido")
            };
            await productosQuerys.ActualizarProducto(idProducto, productoActualizado, tipo);

            return productoActualizado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public async Task<Producto> DeleteProducto(string sucursalNombre, int idProducto)
    {
        try
        {
            var productos = await GetProductos(sucursalNombre);

            var producto = productos.Find(p => p.id == idProducto);
            if (producto == null) throw new Exception("Producto no encontrado");

            await productosQuerys.EliminarProducto(idProducto);
            return producto;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task RegistrarVenta(string sucursalNombre, List<CarritoItem> items)
    {
        try
        {
            var sucursal = await sucursalService.VerSucursal(sucursalNombre);
            if (sucursal == null) throw new Exception("Sucursal no encontrada");

            var productos = await GetProductos(sucursalNombre);

            foreach (var item in items)
            {
                var producto = productos.Find(p => p.id == item.id);
                if (producto == null)
                {
                    throw new Exception($"Producto con ID {item.id} no encontrado en la sucursal");
                }
                if (producto.cantidad < item.cantidad)
                {
                    throw new Exception($"Stock insuficiente para el producto {producto.nombre}");
                }
            }
            await ventas.RegistrarVenta(sucursal.idSucursal, items);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public async Task<List<Producto>> BuscarProductoPorNombre(string sucursalNombre, string nombre)
    {
        try
        {
            var productos = await GetProductos(sucursalNombre);

            var productosEncontrados = productos
                .Where(p => p.nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (productosEncontrados == null || productosEncontrados.Count == 0) throw new Exception("Producto no encontrado");

            return new List<Producto>(productosEncontrados);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<Producto>> BuscarProductoPorCodigo(string sucursalNombre, int codigo)
    {
        try
        {
            var productos = await GetProductos(sucursalNombre);

            var producto = productos.Find(p => p.codigo == codigo);
            if (producto == null || producto.codigo != codigo) throw new Exception("Producto no encontrado");

            return new List<Producto> { producto };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
}
