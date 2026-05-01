// var sucursales = Data.GetData();

// foreach (var sucursal in sucursales)
// {
//     Console.WriteLine($"Sucursal: {sucursal.nombre}");
//     Console.WriteLine($"Direccion: {sucursal.direccion}");
//     Console.WriteLine("Stock:");

//     foreach (var producto in sucursal.stock)
//     {
//         var linea = $"- {producto.nombre} (ID: {producto.id}, Precio: {producto.precio}, Cantidad: {producto.cantidad})";
        
//         if (producto is Herramienta h)
//             linea += $" [Herramienta - Alimentación: {h.tipoAlimentacion}, Trabajo: {h.tipoTrabajo}]";
//         else if (producto is MaterialInsumo m)
//             linea += $" [Material/Insumo - Unidad: {m.unidadMedida}]";
//         else if (producto is AccesorioEquipamiento a)
//             linea += $" [Accesorio/Equipamiento - Uso: {a.uso}]";
        
//         Console.WriteLine(linea);
//     }
//     Console.WriteLine();
// }

var suc = new SucurSalService();
var prod = new ProductoService();
try
{
    var result = suc.ReadSucursal("awd");
    prod.GetProductos(result);
}
catch (Exception ex)
{
    Console.WriteLine($"Error 404: {ex.Message}");
}