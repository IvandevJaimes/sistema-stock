
using Spectre.Console;
public class MenuVentas
{
    private SucursalController sucursalController = new SucursalController();

    public async Task Ejecutar(string nombreSucursal)
    {
        var titulo = new FigletText($"Sucursal: {nombreSucursal}")
            .LeftJustified()
            .Color(Color.Cyan);

        var panel = new Panel("[bold white]Historial de ventas[/]")
            .Border(BoxBorder.Rounded)
            .BorderStyle(new Style(Color.Purple));

        var salir = false;
        while (!salir)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(titulo);
            AnsiConsole.Write(panel);
            AnsiConsole.WriteLine();

            var ventas = await sucursalController.MostrarVentas(nombreSucursal);

            if (ventas.success == false)
            {
                AnsiConsole.MarkupLine($"[red]{ventas.mensaje}[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]Presiona cualquier tecla para volver...[/]");
                Console.ReadKey();
                return;
            }   

            var tabla = new Table();
            tabla.Border(TableBorder.Rounded);
            tabla.BorderColor(Color.Grey);
            tabla.Expand();

            tabla.AddColumn("[bold yellow]Producto[/]");
            tabla.AddColumn("[bold yellow]Cantidad[/]");
            tabla.AddColumn("[bold yellow]Precio unitario[/]");
            tabla.AddColumn("[bold yellow]Subtotal[/]");

            if (ventas.data == null || !ventas.data.Any())
            {
                tabla.AddRow("[grey]-[/]", "[grey]-[/]", "[grey]-[/]", "[grey]-[/]");
                AnsiConsole.Write(tabla);
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]No hay ventas registradas[/]");
            }
            else
            {
                foreach (var v in ventas.data)
                {
                    tabla.AddRow(
                        $"[green]{v.nombreProducto}[/]",
                        $"[cyan]{v.cantidad}[/]",
                        $"[yellow]{v.precioUnitario:C}[/]",
                        $"[magenta]{v.cantidad * v.precioUnitario:C}[/]"
                    );
                }
                AnsiConsole.Write(tabla);

                decimal gananciasTotales = ventas.data.Sum(v => v.cantidad * v.precioUnitario);
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[bold green]Ganancias totales: {gananciasTotales:C}[/]");
            }

            AnsiConsole.WriteLine();
            var opcion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Elegir opción: [/]")
                    .HighlightStyle("bold")
                    .AddChoices("[grey]↩ Volver[/]")
            );

            switch (opcion)
            {
                case "[grey]↩ Volver[/]":
                    salir = true;
                    AnsiConsole.Clear();
                    break;
            }
        }
    }
}
