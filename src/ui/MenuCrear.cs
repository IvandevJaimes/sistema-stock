using Spectre.Console;

public class MenuCrear
{
    private ProductoController productoCtrl = new ProductoController();
    public async Task EjecutarCrear(string nombreSucursal)
    {
        double extra1 = 0;
        string extra2 = "";
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
                .AddChoices("Heladera", "Lavarropas", "Televisor", "[grey]↩ Volver[/]")
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
        var codigo = AnsiConsole.Prompt(
            new TextPrompt<int>("Código del producto:")
                .Validate(c => c > 0 && c.ToString().Length <= 4
                    ? ValidationResult.Success()
                    : ValidationResult.Error("Código inválido"))
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
            case "Heladera":
                tipoProducto = "Heladera";
                extra1 = AnsiConsole.Prompt(
                    new TextPrompt<double>("Capacidad (Litros):")
                        .Validate(v => v > 0 ? ValidationResult.Success() : ValidationResult.Error("Debe ser > 0")));
                extra2 = AnsiConsole.Prompt(
                    new TextPrompt<string>("Tipo:")
                        .Validate(v => !string.IsNullOrWhiteSpace(v) ? ValidationResult.Success() : ValidationResult.Error("No puede estar vacío")));
                break;
            case "Lavarropas":
                tipoProducto = "Lavarropas";
                extra1 = AnsiConsole.Prompt(
                    new TextPrompt<double>("Capacidad (Kg):")
                        .Validate(v => v > 0 ? ValidationResult.Success() : ValidationResult.Error("Debe ser > 0")));
                extra2 = AnsiConsole.Prompt(
                    new TextPrompt<string>("Tipo:")
                        .Validate(v => !string.IsNullOrWhiteSpace(v) ? ValidationResult.Success() : ValidationResult.Error("No puede estar vacío")));
                break;
            case "Televisor":
                tipoProducto = "Televisor";
                extra1 = AnsiConsole.Prompt(
                    new TextPrompt<double>("Pulgadas:")
                        .Validate(v => v > 0 ? ValidationResult.Success() : ValidationResult.Error("Debe ser > 0")));
                extra2 = AnsiConsole.Prompt(
                    new TextPrompt<string>("Tipo de Pantalla:")
                        .Validate(v => !string.IsNullOrWhiteSpace(v) ? ValidationResult.Success() : ValidationResult.Error("No puede estar vacío")));
                break;
        }

        var resultado = await productoCtrl.CrearProducto(codigo, nombreSucursal, tipoProducto, nombre, precio, cantidad, extra1, extra2);

        if (resultado.success)
            Alerta.Exito("Producto creado correctamente");
        else
            Alerta.Error(resultado.mensaje!);
    }
}
