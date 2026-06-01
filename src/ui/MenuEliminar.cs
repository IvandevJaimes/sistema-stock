using Spectre.Console;
public class MenuEliminar
{
    private ProductoController productoCtrl = new ProductoController();
    public async Task EjecutarEliminar(string nombreSucursal, Producto productoSeleccionado)
    {
        AnsiConsole.Clear();
        var titulo = new FigletText($"Sucursal: {nombreSucursal}")
            .LeftJustified()
            .Color(Color.Cyan);
        var panel = new Panel("[bold red]Eliminar Producto[/]")
            .Border(BoxBorder.Rounded)
            .BorderStyle(new Style(Color.Red));

        var tabla = new Table();
        tabla.Border(TableBorder.Rounded);
        tabla.BorderColor(Color.Grey);
        tabla.Expand();
        tabla.AddColumn("[bold yellow]ID[/]");
        tabla.AddColumn("[bold yellow]Nombre[/]");
        tabla.AddColumn("[bold yellow]Tipo[/]");
        tabla.AddColumn("[bold yellow]Precio[/]");
        tabla.AddColumn("[bold yellow]Cantidad[/]");
        tabla.AddColumn("[bold yellow]Detalles[/]");
        tabla.AddRow(
            $"[cyan]{productoSeleccionado.id}[/]",
            $"[green]{productoSeleccionado.nombre}[/]",
            $"[magenta]{productoSeleccionado.GetType().Name}[/]",
            $"[yellow]{productoSeleccionado.precio:C}[/]",
            $"[red]{productoSeleccionado.cantidad}[/]",
            $"[grey]{productoSeleccionado.ObtenerDetalles()}[/]"
        );
        AnsiConsole.Write(titulo);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[cyan]Producto seleccionado:[/]");
        AnsiConsole.Write(tabla);
        AnsiConsole.Write(panel);

        var opcion = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle("bold")
                .AddChoices("Eliminar este producto", "[grey]↩ Volver[/]")
        );
        switch (opcion)
        {
            case "Eliminar este producto":
                var confirmar = AnsiConsole.Prompt(
                    new ConfirmationPrompt($"¿Estás seguro de eliminar '{productoSeleccionado.nombre}'?")
                );
                if (!confirmar)
                {
                    Alerta.Error("Operación cancelada");
                    return; 
                }
                var resultado = await productoCtrl.EliminarProducto(nombreSucursal, productoSeleccionado.id);
                if (resultado.success)
                {
                    Alerta.Exito($"Producto '{productoSeleccionado.nombre}' eliminado");
                }
                else
                {
                    Alerta.Error(resultado.mensaje!);
                }
                break;
            case "[grey]↩ Volver[/]":
                AnsiConsole.Clear();
                break;
        }
    }
}