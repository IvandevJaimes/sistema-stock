using Spectre.Console;

public class MenuCrear
{
    private ProductoController productoCtrl = new ProductoController();
    public void EjecutarCrear(string nombreSucursal)
    {
        string extra1 = "", extra2 = "";
        string tipoProducto = "";
        AnsiConsole.Clear();
        var titulo = new FigletText($"Sucursal: {nombreSucursal}")
            .LeftJustified()
            .Color(Color.Cyan);

        AnsiConsole.Write(titulo);

        var panel = new Panel("[bold green]Crear nuevo producto[/]")
            .Border(BoxBorder.Rounded)
            .BorderStyle(new Style(Color.Green));
        AnsiConsole.Write(panel);

        var tipo = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el tipo de producto:[/]")
                .HighlightStyle("bold")
                .AddChoices("Herramienta", "Material / Insumo", "Accesorio / Equipamiento", "[grey]↩ Volver[/]")
        );

        if (tipo == "[grey]↩ Volver[/]")
        {
            AnsiConsole.Clear();
            return;
        }

        var nombre = AnsiConsole.Prompt(
            new TextPrompt<string>("Nombre del producto:")
                .Validate(n => !string.IsNullOrWhiteSpace(n) && n.Length <= 50
                    ? ValidationResult.Success()
                    : ValidationResult.Error("Nombre inválido"))
        );

        var precio = AnsiConsole.Prompt(
            new TextPrompt<decimal>("Precio:")
                .Validate(p => p > 0 ? ValidationResult.Success() : ValidationResult.Error("Debe ser > 0"))
        );

        var cantidad = AnsiConsole.Prompt(
            new TextPrompt<int>("Cantidad:")
                .Validate(c => c >= 0 ? ValidationResult.Success() : ValidationResult.Error("Debe ser >= 0"))
        );

        switch (tipo)
        {
            case "Herramienta":
                tipoProducto = "herramienta";
                extra1 = AnsiConsole.Ask<string>("Tipo de alimentación:");
                extra2 = AnsiConsole.Ask<string>("Tipo de trabajo:");
                break;
            case "Material / Insumo":
                tipoProducto = "materialInsumo";
                extra1 = AnsiConsole.Ask<string>("Unidad de medida:");
                break;
            case "Accesorio / Equipamiento":
                tipoProducto = "accesorioEquipamiento";
                extra1 = AnsiConsole.Ask<string>("Uso:");
                break;

            case "[grey]↩ Volver[/]":
                AnsiConsole.Clear();
                break;


        }

        var resultado = productoCtrl.CrearProducto(nombreSucursal, tipoProducto, nombre, precio, cantidad, extra1, extra2);

        if (resultado.success)
            Alerta.Exito("Producto creado correctamente");
        else
            Alerta.Error(resultado.mensaje!);
    }
}
