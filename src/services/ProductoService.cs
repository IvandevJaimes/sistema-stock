//Capa de servicios para productos, encargada de manejar solo la logica de negocio usando las querys
public class ProductoService
{
    private SucursalService sucursalService = new SucursalService(); // instanciar el servicio de sucursales para usarlo en esta clase y evitar acoplamiento con la capa de querys de sucursales

    private ProductosQuerys productosQuerys = new ProductosQuerys(); // instanciar las querys de productos para usarla solo en esta clase

    private VentasQuerys ventas = new VentasQuerys();
    public async Task<List<Producto>> GetProductos(string sucursalNombre)
    {
        List<Producto> productos = new List<Producto>(); // variable vacia para ir guardando los resultados de la consulta
        try
        {
            var sucursal = await sucursalService.VerSucursal(sucursalNombre);
            if (sucursal == null) throw new Exception("Sucursal no encontrada"); // verificar que se haya encontrado la sucursal, si no lanzar una excepcion
            productos = await productosQuerys.GetProductos(sucursal.idSucursal); // obtener los productos de la sucursal encontrada usando su idSucursal
            return productos;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Producto> PostProducto(int codigo, string sucursalNombre, string tipo, string nombre, decimal precio, int cantidad, double extra1, string extra2 = "")
    {
        //recibir los datos necesarios para crear un producto, incluyendo su tipo y los atributos extra segun el tipo, y devolver el producto creado con su id asignado por la base de datos
        try
        {
            var sucursal = await sucursalService.VerSucursal(sucursalNombre);
            if (sucursal == null) throw new Exception("Sucursal no encontrada");

            var productos = await GetProductos(sucursalNombre);
            var productoExistente = productos.Find(p => p.codigo == codigo);

            if (productoExistente != null) throw new Exception("Ya existe un producto con el mismo código en esta sucursal");
            //verificar que no exista otro producto con el mismo codigo en la misma sucursal, si existe lanzar una excepcion ya que el codigo es unico por sucursal (UNIQUE)

            //crear el producto correcto segun el tipo usando switch y pasarle los datos polimorficos recibidos. el id se asigna despues de insertarlo en la base de datos
            Producto nuevo = tipo.ToLower() switch
            {
                "televisor" => new Televisor(0, codigo, nombre, precio, cantidad, extra1, extra2),
                "heladera" => new Heladera(0, codigo, nombre, precio, cantidad, extra1, extra2),
                "lavarropas" => new Lavarropas(0, codigo, nombre, precio, cantidad, extra1, extra2),
                _ => throw new Exception("Tipo de producto inválido")
            };

            var idProducto = await productosQuerys.CrearProducto(nuevo, sucursal.idSucursal);
            nuevo.id = idProducto; // asignar el id generado por la base de datos al producto creado para devolverlo completo con su id asignado

            return nuevo;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    public async Task<Producto> PutProducto(int idProducto, string sucursalNombre, string? nombre = null, decimal? precio = null, int? cantidad = null, double? extra1 = null, string? extra2 = null)
    {
        //se reciben todos los datos del producto como opcionales excepto el idProducto y el nombre de la sucursal para identificar el producto a actualizar. esto permite actualizacion parcial
        // se denomina nullable a los parametros opcionales, esto quiere decir que pueden recibir un valor o null. si reciben un valor se actualiza ese campo, si reciben null se mantiene el valor actual del producto sin actualizarlo
        try
        {
            var productos = await GetProductos(sucursalNombre);

            var producto = productos.Find(p => p.id == idProducto);
            if (producto == null) throw new Exception("Producto no encontrado"); //primero verificar por id si el producto existe en la db y en la sucursal, si no existe lanzar una excepcion

            var tipo = producto.GetType().Name; //obtener el tipo del producto encontrado

            Producto productoActualizado = tipo switch
            {
                //actuaizar segun el tipo del producto y verificar cada campo si recibio un valor o no
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
            await productosQuerys.ActualizarProducto(idProducto, productoActualizado, tipo); //enviar el producto actualizado a la capa querys

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
            if (producto == null) throw new Exception("Producto no encontrado"); //verificar que el producto exista antes de intentar eliminarlo, si no existe lanzar una excepcion

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

            foreach (var item in items) //verificar que cada producto del carrito exista en la sucursal y que tenga stock suficiente para la cantidad vendida 
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
            await ventas.RegistrarVenta(sucursal.idSucursal, items); //enviar el idSucursal y la lista de items del carrito a la capa de querys

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
            //verificar que se hayan encontrado productos con ese nombre, si no lanzar una excepcion
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
            //verificar que se haya encontrado un producto con ese codigo, si no lanzar una excepcion. el codigo es unico por sucursal, por lo que solo puede haber un producto con ese codigo en la sucursal
            return new List<Producto> { producto };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
}
