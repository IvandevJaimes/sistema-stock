var sucursales = Data.getData();

foreach (var sucursal in sucursales)
{
    Console.WriteLine($"Sucursal: {sucursal.nombre}");
    Console.WriteLine($"Direccion: {sucursal.direccion}");
    Console.WriteLine("Stock:");

    foreach (var producto in sucursal.stock)
    {
        var linea = $"- {producto.nombre} (ID: {producto.item_id}, Precio: {producto.precio}, Cantidad: {producto.cantidad})";
        
        if (producto is Herramienta h)
            linea += $" [Herramienta - Alimentación: {h.tipoAlimentacion}, Trabajo: {h.tipoTrabajo}]";
        else if (producto is MaterialInsumo m)
            linea += $" [Material/Insumo - Unidad: {m.unidadMedida}]";
        else if (producto is AccesorioEquipamiento a)
            linea += $" [Accesorio/Equipamiento - Uso: {a.uso}]";
        
        Console.WriteLine(linea);
    }
    Console.WriteLine();
}
