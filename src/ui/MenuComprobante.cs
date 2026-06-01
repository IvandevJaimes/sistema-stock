using Spectre.Console;
public class MenuComprobante
{

    private SucursalController sucursalCtrl = new SucursalController();
    public async Task Generar(string nombreSucursal, List<CarritoItem> carrito, decimal total)
    {
        var numero = $"V-{DateTime.Now:yyyyMMddHHmmss}";
        var fecha = DateTime.Now;

        await EjecutarComprobante(nombreSucursal, carrito, total, numero, fecha);
    }

    private async Task EjecutarComprobante(string sucursal, List<CarritoItem> carrito, decimal total, string numero, DateTime fecha)
    {
        var sucursalObj = await sucursalCtrl.ObtenerSucursal(sucursal);
        if (!sucursalObj.success || sucursalObj.data == null)
        {
            Alerta.Error("No se pudo generar el comprobante.");
            return;
        }
        AnsiConsole.Clear();

        AnsiConsole.Write(new Rule("[yellow]Comprobante de Venta[/]").RuleStyle("grey"));

        AnsiConsole.MarkupLine($"[bold]N°:[/] {numero}");
        AnsiConsole.MarkupLine($"[bold]Sucursal:[/] {sucursalObj?.data.nombre}");

        AnsiConsole.MarkupLine($"[bold]Fecha:[/] {fecha:dd/MM/yyyy HH:mm}");

        AnsiConsole.Write(new Rule().RuleStyle("grey"));
        AnsiConsole.WriteLine();

        var tabla = new Table().Border(TableBorder.Rounded).Expand();

        tabla.AddColumn("Producto");
        tabla.AddColumn("Cant");
        tabla.AddColumn("Precio");
        tabla.AddColumn("Subtotal");

        foreach (var p in carrito)
        {
            tabla.AddRow(
                p.nombre,
                p.cantidad.ToString(),
                p.precio.ToString("C"),
                (p.precio * p.cantidad).ToString("C")
            );
        }

        AnsiConsole.Write(tabla);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold green]✔ Venta realizada[/]");
        AnsiConsole.MarkupLine($"[bold white]TOTAL: {total:C}[/]");

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[grey]Presione una tecla para continuar...[/]");
        Console.ReadKey(true);
    }
}