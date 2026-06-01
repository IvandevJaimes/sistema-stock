using Spectre.Console;
public class MenuEditar
{
    private ProductoController productoCtrl = new ProductoController();

    public async Task<Result<Producto>?> EjecutarEditar(string nombreSucursal, Producto productoSeleccionado)
    {
        string nombreActual = productoSeleccionado.nombre;
        decimal precioActual = productoSeleccionado.precio;
        int cantidadActual = productoSeleccionado.cantidad;

        double? extra1Actual = productoSeleccionado switch
        {
            Televisor t => t.pulgadas,
            Heladera h => h.capacidad,
            Lavarropas l => l.carga,
            _ => null
        };
        string? extra2Actual = productoSeleccionado switch
        {
            Televisor t => t.tipoPantalla,
            Heladera h => h.tipo,
            Lavarropas l => l.tipo,
            _ => null
        };

        Result<Producto>? nuevo = null;

        var titulo = new FigletText($"Sucursal: {nombreSucursal}")
        .LeftJustified()
        .Color(Color.Cyan);


        var panel = new Panel("[bold cyan]Editar Producto[/]")
            .Border(BoxBorder.Rounded)
            .BorderStyle(new Style(Color.Cyan));

        var salir = false;

        var extrasOpcion1 = productoSeleccionado switch
        {
            Televisor => "Pulgadas",
            Heladera => "Capacidad",
            Lavarropas => "Carga",
            _ => "Extra 1"
        };

        var extrasOpcion2 = productoSeleccionado switch
        {
            Televisor => "Tipo de pantalla",
            Heladera => "Tipo",
            Lavarropas => "Tipo",
            _ => "Extra 2"
        };

        while (!salir)
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(titulo);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[cyan]Producto seleccionado:[/]");
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
                $"[green]{nombreActual}[/]",
                $"[magenta]{productoSeleccionado.GetType().Name}[/]",
                $"[yellow]{precioActual:C}[/]",
                $"[red]{cantidadActual}[/]",
                $"[grey]{productoSeleccionado.ObtenerDetalles()}[/]"
            );
            AnsiConsole.Write(tabla);
            AnsiConsole.Write(panel);

            if (nombreActual != productoSeleccionado.nombre || precioActual != productoSeleccionado.precio || cantidadActual != productoSeleccionado.cantidad)
            {
                AnsiConsole.MarkupLine($"[yellow]Campos a guardar: {nombreActual} {precioActual:C} {cantidadActual}[/]");
            }
            AnsiConsole.WriteLine();
            var opcion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("¿Qué campo querés editar?")
                .HighlightStyle("bold")
                .AddChoices("Nombre", "Precio", "Cantidad", extrasOpcion1, extrasOpcion2, "Guardar cambios", "[grey]↩ Volver[/]")
            );

            switch (opcion)
            {
                case "Nombre":
                    var nuevoNombre = AnsiConsole.Prompt(
                        new TextPrompt<string>($"Nuevo nombre (actual: {nombreActual}):")
                            .Validate(nombre =>
                                !string.IsNullOrWhiteSpace(nombre) && nombre.Length >= 1 && nombre.Length <= 20
                                    ? ValidationResult.Success()
                                    : ValidationResult.Error("[red]El nombre debe tener entre 1 y 20 caracteres[/]")
                            )
                    );
                    if (!string.IsNullOrWhiteSpace(nuevoNombre)) nombreActual = nuevoNombre;
                    AnsiConsole.Clear();
                    break;

                case "Precio":
                    var nuevoPrecio = AnsiConsole.Prompt(
                        new TextPrompt<decimal>($"Nuevo precio (actual: {precioActual:C}):")
                            .Validate(precio =>
                                precio > 0
                                    ? ValidationResult.Success() :
                                    ValidationResult.Error("[red]El precio debe ser mayor a 0[/]")
                            )
                    );
                    precioActual = nuevoPrecio;
                    AnsiConsole.Clear();
                    break;

                case "Cantidad":
                    var nuevaCantidad = AnsiConsole.Prompt(
                        new TextPrompt<int>($"Nueva cantidad (actual: {cantidadActual}):")
                            .Validate(cantidad =>
                                cantidad >= 0
                                    ? ValidationResult.Success()
                                    : ValidationResult.Error("[red]La cantidad no puede ser negativa[/]")
                            )
                    );
                    cantidadActual = nuevaCantidad;
                    AnsiConsole.Clear();
                    break;

                case "Pulgadas":
                case "Capacidad":
                case "Carga":
                    var nuevoExtra1 = AnsiConsole.Prompt(
                        new TextPrompt<double>($"Nuevo valor para {extrasOpcion1} (actual: {extra1Actual}):")
                            .Validate(valor =>
                                valor > 0
                                    ? ValidationResult.Success()
                                    : ValidationResult.Error($"[red]El valor de {extrasOpcion1} debe ser mayor a 0[/]")
                            )
                    );
                    extra1Actual = nuevoExtra1;
                    AnsiConsole.Clear();
                    break;

                case "Tipo de pantalla":
                case "Tipo":
                    var nuevoExtra2 = AnsiConsole.Prompt(
                        new TextPrompt<string>($"Nuevo valor para {extrasOpcion2} (actual: {extra2Actual}):")
                            .Validate(v => !string.IsNullOrWhiteSpace(v)
                                ? ValidationResult.Success()
                                : ValidationResult.Error($"[red]No puede estar vacío[/]"))
                    );
                    extra2Actual = nuevoExtra2;
                    AnsiConsole.Clear();
                    break;

                case "Guardar cambios":
                    var resultado = await productoCtrl.ActualizarProducto(
                        productoSeleccionado.id,
                        nombreSucursal,
                        nombreActual,
                        precioActual,
                        cantidadActual,
                        extra1Actual,
                        extra2Actual
                    );

                    if (resultado.success)
                    {
                        Alerta.Exito("Producto actualizado");
                        nuevo = resultado;
                        salir = true; AnsiConsole.Clear(); break;
                    }
                    else
                    {
                        Alerta.Error(resultado.mensaje!);
                        break;
                    }

                case "[grey]↩ Volver[/]":
                    salir = true; AnsiConsole.Clear();
                    break;
            }

        }
        return nuevo;
    }
}