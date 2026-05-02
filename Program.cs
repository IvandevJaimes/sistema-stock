var suc = new SucursalService();
var prod = new ProductoService();
// 1. CREAR PRODUCTO
Console.WriteLine("=== CREANDO PRODUCTO ===");
var nuevo = prod.PostProducto("Norte", "herramienta", "Destornillador", 2500m, 20, "manual", "giro");
if (nuevo != null) // ✅ Verificamos null UNA vez
{
    Console.WriteLine($"Creado: {nuevo.nombre} (ID: {nuevo.id})");
    // 2. MOSTRAR ANTES DEL UPDATE
    Console.WriteLine("\n=== ANTES DEL UPDATE ===");
    var result = suc.VerSucursal("Norte");
    if (result != null)
    {
        var productos = prod.GetProductos(result);
        foreach (var p in productos)
        {
            if (p.id == nuevo.id)
                Console.WriteLine($"ID: {p.id} - {p.nombre} - ${p.precio}");
        }
    }
    // 3. ACTUALIZAR (Adentro del null check)
    Console.WriteLine("\n=== ACTUALIZANDO ===");
    var actualizado = prod.PutProducto("Norte", nuevo.id, "Destornillador Pro", 3200m, 18);
    
    if (actualizado != null)
    {
        Console.WriteLine($"Actualizado: {actualizado.nombre} {actualizado.precio} (ID: {actualizado.id})");
    }
    // 4. MOSTRAR DESPUÉS DEL UPDATE
    Console.WriteLine("\n=== DESPUÉS DEL UPDATE ===");
    if (result != null)
    {
        var productosActualizados = prod.GetProductos(result);
        foreach (var p in productosActualizados)
        {
            if (p.id == nuevo.id)
                Console.WriteLine($"ID: {p.id} - {p.nombre} - ${p.precio}");
        }
    }
}
else
{
    Console.WriteLine("ERROR: No se pudo crear el producto");
}