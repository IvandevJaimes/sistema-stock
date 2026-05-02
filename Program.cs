var suc = new SucursalService();
var prod = new ProductoService();
// 1. CREAR PRODUCTO (Verificamos null)
Console.WriteLine("=== CREANDO PRODUCTO ===");
var nuevo = prod.PostProducto("Norte", "herramienta", "Destornillador", 2500m, 20, "manual", "giro");
if (nuevo != null) // ✅ Verificamos
{
    Console.WriteLine($"Creado: {nuevo.nombre} (ID: {nuevo.id})");
}
else
{
    Console.WriteLine("ERROR: No se pudo crear el producto");
}
// 2. MOSTRAR TODOS LOS PRODUCTOS
Console.WriteLine("\n=== LISTA DESPUÉS DE CREAR ===");
var result = suc.VerSucursal("Norte");
if (result != null) // ✅ Verificamos
{
    var productos = prod.GetProductos(result);
    foreach (var p in productos)
    {
        Console.WriteLine($"ID: {p.id} - {p.tipo} - {p.nombre} - ${p.precio}");
    }
}
// 3. ACTUALIZAR EL PRODUCTO CREADO
Console.WriteLine("\n=== ACTUALIZANDO PRODUCTO ===");
if (nuevo != null) // ✅ Necesitamos el ID
{
    var actualizado = prod.PutProducto("Norte", nuevo.id, "Destornillador Pro", 3200m, 18);
    
    if (actualizado != null) // ✅ Verificamos
    {
        Console.WriteLine($"Actualizado: {actualizado.nombre} (ID: {actualizado.id})");
    }
}
// 4. MOSTRAR NUEVAMENTE
Console.WriteLine("\n=== LISTA DESPUÉS DE ACTUALIZAR ===");
if (result != null)
{
    var productosActualizados = prod.GetProductos(result);
    foreach (var p in productosActualizados)
    {
        Console.WriteLine($"ID: {p.id} - {p.tipo} - {p.nombre} - ${p.precio}");
    }
}