var sucursalCtrl = new SucursalController();
var productoCtrl = new ProductoController();

Console.WriteLine("=== PRUEBAS EXHAUSTIVAS DEL SISTEMA DE STOCK ===\n");

// ==========================================
// SECCIÓN 1: SUCURSALES
// ==========================================
Console.WriteLine("=== SECCIÓN 1: SUCURSALES ===\n");

Console.WriteLine("1.1. Listar todas las sucursales:");
var sucursales = sucursalCtrl.ListarSucursales();
foreach (var s in sucursales)
{
    Console.WriteLine($"   ✅ {s.nombre} | {s.direccion} | Productos: {s.stock.Count}");
}
Console.WriteLine();

Console.WriteLine("1.2. Obtener sucursal existente (Norte):");
var sucursalNorte = sucursalCtrl.ObtenerSucursal("Norte");
if (sucursalNorte != null)
    Console.WriteLine($"   ✅ Encontrada: {sucursalNorte.nombre} - {sucursalNorte.direccion}");
else
    Console.WriteLine($"   ❌ Error: Sucursal no encontrada");
Console.WriteLine();

Console.WriteLine("1.3. Obtener sucursal INEXISTENTE (Sur):");
var sucursalSur = sucursalCtrl.ObtenerSucursal("Sur");
if (sucursalSur != null)
    Console.WriteLine($"   ✅ Encontrada: {sucursalSur.nombre}");
else
    Console.WriteLine($"   ❌ Esperado: Sucursal 'Sur' no encontrada (null)");
Console.WriteLine();

// ==========================================
// SECCIÓN 2: CREAR PRODUCTOS (TODOS LOS TIPOS)
// ==========================================
Console.WriteLine("=== SECCIÓN 2: CREAR PRODUCTOS ===\n");

Console.WriteLine("2.1. Crear HERRAMIENTA (Éxito):");
var h1 = productoCtrl.CrearProducto("Norte", "herramienta", "Taladro Bosch", 25000m, 10, "electrico", "perforacion");
if (h1.success)
    Console.WriteLine($"   ✅ Creado: {h1.data!.nombre} | ID: {h1.data.id} | {h1.data.ObtenerDetalles()}");
else
    Console.WriteLine($"   ❌ Error: {h1.mensaje}");
Console.WriteLine();

Console.WriteLine("2.2. Crear MATERIAL INSUMO (Éxito):");
var m1 = productoCtrl.CrearProducto("Norte", "materialInsumo", "Pegamento 50ml", 800m, 30, "tubo", "");
if (m1.success)
    Console.WriteLine($"   ✅ Creado: {m1.data!.nombre} | ID: {m1.data.id} | {m1.data.ObtenerDetalles()}");
else
    Console.WriteLine($"   ❌ Error: {m1.mensaje}");
Console.WriteLine();

Console.WriteLine("2.3. Crear ACCESORIO EQUIPAMIENTO (Éxito):");
var a1 = productoCtrl.CrearProducto("Norte", "accesorioEquipamiento", "Casco Seguridad", 4500m, 20, "proteccion", "");
if (a1.success)
    Console.WriteLine($"   ✅ Creado: {a1.data!.nombre} | ID: {a1.data.id} | {a1.data.ObtenerDetalles()}");
else
    Console.WriteLine($"   ❌ Error: {a1.mensaje}");
Console.WriteLine();

Console.WriteLine("2.4. Crear producto en sucursal INEXISTENTE:");
var errSucursal = productoCtrl.CrearProducto("Este", "herramienta", "Martillo", 9000m, 5, "manual", "golpe");
if (errSucursal.success)
    Console.WriteLine($"   ✅ Creado: {errSucursal.data!.nombre}");
else
    Console.WriteLine($"   ❌ Esperado: {errSucursal.mensaje}");
Console.WriteLine();

Console.WriteLine("2.5. Crear producto con TIPO INVÁLIDO:");
var errTipo = productoCtrl.CrearProducto("Norte", "invalido", "Producto Raro", 100m, 1, "", "");
if (errTipo.success)
    Console.WriteLine($"   ✅ Creado: {errTipo.data!.nombre}");
else
    Console.WriteLine($"   ❌ Esperado: {errTipo.mensaje}");
Console.WriteLine();

// ==========================================
// SECCIÓN 3: LISTAR PRODUCTOS
// ==========================================
Console.WriteLine("=== SECCIÓN 3: LISTAR PRODUCTOS ===\n");

Console.WriteLine("3.1. Listar productos en 'Norte' (debería tener los 3 nuevos + los 4 originales = 7):");
var productosNorte = productoCtrl.ListarProductos("Norte");
Console.WriteLine($"   Total: {productosNorte.Count} productos");
foreach (var p in productosNorte)
{
    Console.WriteLine($"   - ID: {p.id} | {p.nombre} | ${p.precio} | Stock: {p.cantidad} | {p.ObtenerDetalles()}");
}
Console.WriteLine();

Console.WriteLine("3.2. Listar productos en sucursal INEXISTENTE:");
var productosSur = productoCtrl.ListarProductos("Sur");
Console.WriteLine($"   Total: {productosSur.Count} productos (esperado: 0)");
Console.WriteLine();

// ==========================================
// SECCIÓN 4: VENDER PRODUCTOS
// ==========================================
Console.WriteLine("=== SECCIÓN 4: VENDER PRODUCTOS ===\n");

if (h1.success)
{
    Console.WriteLine($"4.1. Vender 3 unidades de '{h1.data!.nombre}' (Stock actual: {h1.data.cantidad}):");
    var v1 = productoCtrl.VenderProducto("Norte", h1.data.id, 3);
    if (v1.success)
        Console.WriteLine($"   ✅ Venta exitosa. Ingresos: ${v1.data} | Stock restante: {h1.data.cantidad - 3}");
    else
        Console.WriteLine($"   ❌ Error: {v1.mensaje}");
}
Console.WriteLine();

if (h1.success)
{
    Console.WriteLine($"4.2. Vender MÁS de lo disponible (intentar vender 100 de '{h1.data!.nombre}'):");
    var v2 = productoCtrl.VenderProducto("Norte", h1.data.id, 100);
    if (v2.success)
        Console.WriteLine($"   ✅ Venta exitosa. Ingresos: ${v2.data}");
    else
        Console.WriteLine($"   ❌ Esperado: {v2.mensaje}");
}
Console.WriteLine();

Console.WriteLine("4.3. Vender producto INEXISTENTE (ID: 99999):");
var v3 = productoCtrl.VenderProducto("Norte", 99999, 1);
if (v3.success)
    Console.WriteLine($"   ✅ Venta exitosa.");
else
    Console.WriteLine($"   ❌ Esperado: {v3.mensaje}");
Console.WriteLine();

Console.WriteLine("4.4. Vender en sucursal INEXISTENTE:");
var v4 = productoCtrl.VenderProducto("Sur", 1, 1);
if (v4.success)
    Console.WriteLine($"   ✅ Venta exitosa.");
else
    Console.WriteLine($"   ❌ Esperado: {v4.mensaje}");
Console.WriteLine();

// ==========================================
// SECCIÓN 5: CALCULAR VALOR Y INGRESOS
// ==========================================
Console.WriteLine("=== SECCIÓN 5: CÁLCULOS (VALOR E INGRESOS) ===\n");

if (h1.success)
{
    Console.WriteLine($"5.1. Calcular VALOR de stock actual de '{h1.data!.nombre}':");
    decimal valor = h1.data.calcularValor();
    Console.WriteLine($"   ✅ Precio: ${h1.data.precio} x Cantidad: {h1.data.cantidad} = ${valor}");
}
Console.WriteLine();

if (h1.success)
{
    Console.WriteLine($"5.2. Calcular INGRESOS por venta de 2 unidades de '{h1.data!.nombre}':");
    decimal ingresos = h1.data.calcularIngresos(2);
    Console.WriteLine($"   ✅ ${h1.data.precio} x 2 = ${ingresos}");
}
Console.WriteLine();

// ==========================================
// SECCIÓN 6: ACTUALIZAR PRODUCTOS
// ==========================================
Console.WriteLine("=== SECCIÓN 6: ACTUALIZAR PRODUCTOS ===\n");

if (m1.success)
{
    Console.WriteLine($"6.1. Actualizar '{m1.data!.nombre}' (cambiar nombre y precio):");
    var up1 = productoCtrl.ActualizarProducto("Norte", m1.data.id, "Pegamento 100ml", 1500m, m1.data.cantidad);
    if (up1.success)
        Console.WriteLine($"   ✅ Actualizado: {up1.data!.nombre} | Nuevo precio: ${up1.data.precio}");
    else
        Console.WriteLine($"   ❌ Error: {up1.mensaje}");
}
Console.WriteLine();

Console.WriteLine("6.2. Actualizar producto INEXISTENTE (ID: 99999):");
var up2 = productoCtrl.ActualizarProducto("Norte", 99999, "No existe", 100m, 1);
if (up2.success)
    Console.WriteLine($"   ✅ Actualizado.");
else
    Console.WriteLine($"   ❌ Esperado: {up2.mensaje}");
Console.WriteLine();

// ==========================================
// SECCIÓN 7: ELIMINAR PRODUCTOS
// ==========================================
Console.WriteLine("=== SECCIÓN 7: ELIMINAR PRODUCTOS ===\n");

if (a1.success)
{
    Console.WriteLine($"7.1. Eliminar '{a1.data!.nombre}' (ID: {a1.data.id}):");
    var del1 = productoCtrl.EliminarProducto("Norte", a1.data.id);
    if (del1.success)
        Console.WriteLine($"   ✅ Eliminado: {del1.data!.nombre}");
    else
        Console.WriteLine($"   ❌ Error: {del1.mensaje}");
}
Console.WriteLine();

Console.WriteLine("7.2. Eliminar producto YA ELIMINADO (ID de arriba):");
if (a1.success)
{
    var del2 = productoCtrl.EliminarProducto("Norte", a1.data!.id);
    if (del2.success)
        Console.WriteLine($"   ✅ Eliminado.");
    else
        Console.WriteLine($"   ❌ Esperado: {del2.mensaje}");
}
Console.WriteLine();

Console.WriteLine("7.3. Eliminar producto INEXISTENTE (ID: 99999):");
var del3 = productoCtrl.EliminarProducto("Norte", 99999);
if (del3.success)
    Console.WriteLine($"   ✅ Eliminado.");
else
    Console.WriteLine($"   ❌ Esperado: {del3.mensaje}");
Console.WriteLine();

// ==========================================
// SECCIÓN 8: VERIFICACIÓN FINAL
// ==========================================
Console.WriteLine("=== SECCIÓN 8: VERIFICACIÓN FINAL ===\n");

Console.WriteLine("8.1. Lista final de productos en 'Norte':");
var finalNorte = productoCtrl.ListarProductos("Norte");
Console.WriteLine($"   Total: {finalNorte.Count} productos (debería ser 6 - se eliminó 1)");
foreach (var p in finalNorte)
{
    Console.WriteLine($"   - ID: {p.id} | {p.nombre} | Stock: {p.cantidad}");
}
Console.WriteLine();

Console.WriteLine("8.2. Verificar calcularValor() de toda la sucursal:");
decimal totalSucursal = 0;
foreach (var p in finalNorte)
{
    totalSucursal += p.calcularValor();
}
Console.WriteLine($"   ✅ Valor total del stock en Norte: ${totalSucursal}");
Console.WriteLine();

Console.WriteLine("8.3. Verificar sucursal 'Centro' (no debe haberse modificado):");
var centro = productoCtrl.ListarProductos("Centro");
Console.WriteLine($"   Total productos en Centro: {centro.Count} (esperado: 5 - datos estáticos)");
foreach (var p in centro)
{
    Console.WriteLine($"   - {p.nombre} | Stock: {p.cantidad}");
}

Console.WriteLine("\n=== ✅ TODAS LAS PRUEBAS COMPLETADAS ===");
