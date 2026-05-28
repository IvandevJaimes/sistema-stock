using Spectre.Console;
public class MenuVentas
{
    private SucursalController sucursalController = new SucursalController();

    public void Ejecutar(string nombreSucursal)
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

            var ventas = sucursalController.MostrarVentas(nombreSucursal);

            var tabla = new Table();
            tabla.Border(TableBorder.Rounded);
            tabla.BorderColor(Color.Grey);
            tabla.Expand();

            tabla.AddColumn("[bold yellow]Producto[/]");
            tabla.AddColumn("[bold yellow]Cantidad[/]");
            tabla.AddColumn("[bold yellow]Precio unitario[/]");
            tabla.AddColumn("[bold yellow]Subtotal[/]");

            if (ventas != null && ventas.Count > 0)
            {
                foreach (var v in ventas)
                {
                    tabla.AddRow(
                        $"[green]{v.nombre}[/]",
                        $"[cyan]{v.cantidad}[/]",
                        $"[yellow]{v.precio:C}[/]",
                        $"[magenta]{v.cantidad * v.precio:C}[/]"
                    );
                }
                AnsiConsole.Write(tabla);

                decimal gananciasTotales = ventas.Sum(v => v.cantidad * v.precio);
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[bold green]Ganancias totales: {gananciasTotales:C}[/]");
            }
            else
            {
                tabla.AddRow("[grey]-[/]", "[grey]-[/]", "[grey]-[/]", "[grey]-[/]");
                AnsiConsole.Write(tabla);
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]No hay ventas registradas[/]");
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
