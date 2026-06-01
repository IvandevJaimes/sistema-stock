using Spectre.Console;
//pantalla de edicion parcial de producto campo por campo, mostrando los valores actuales y permitiendo modificarlos individualmente
public class MenuEditar
{
    private ProductoController productoCtrl = new ProductoController();

    public async Task<Result<Producto>?> EjecutarEditar(string nombreSucursal, Producto productoSeleccionado)
    {
        //copiar los valores actuales del producto para trabajar con ellos localmente y no modificar el original hasta guardar
        string nombreActual = productoSeleccionado.nombre;
        decimal precioActual = productoSeleccionado.precio;
        int cantidadActual = productoSeleccionado.cantidad;

        //extraer los valores polimorficos segun el tipo del producto usando pattern matching
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

        //nombres de los campos extra segun el tipo de producto para mostrarlos dinamicamente en el menu
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
            //tabla que muestra el estado actual del producto junto con los campos modificados localmente
            var tabla = new Table();
            tabla.Border(TableBorder.Rounded);
            tabla.BorderColor(Color.Grey);
            tabla.Expand();

            tabla.AddColumn("[bold yellow]ID[/]");
            tabla.AddColumn("[bold yellow]Código[/]");
            tabla.AddColumn("[bold yellow]Nombre[/]");
            tabla.AddColumn("[bold yellow]Tipo[/]");
            tabla.AddColumn("[bold yellow]Precio[/]");
            tabla.AddColumn("[bold yellow]Cantidad[/]");
            tabla.AddColumn("[bold yellow]Detalles[/]");


            tabla.AddRow(
                $"[cyan]{productoSeleccionado.id}[/]",
                $"[cyan]{productoSeleccionado.codigo}[/]",
                $"[green]{nombreActual}[/]",
                $"[magenta]{productoSeleccionado.GetType().Name}[/]",
                $"[yellow]{precioActual:C}[/]",
                $"[red]{cantidadActual}[/]",
                $"[grey]{productoSeleccionado.ObtenerDetalles()}[/]"
            );
            AnsiConsole.Write(tabla);
            AnsiConsole.Write(panel);

            //mostrar los campos modificados antes de guardar. compara cada valor local contra el original del producto e incluye los extras polimorficos
            var cambios = new List<string>();
            if (nombreActual != productoSeleccionado.nombre) cambios.Add($"Nombre: [green]{nombreActual}[/]");
            if (precioActual != productoSeleccionado.precio) cambios.Add($"Precio: [green]{precioActual:C}[/]");
            if (cantidadActual != productoSeleccionado.cantidad) cambios.Add($"Cantidad: [green]{cantidadActual}[/]");

            double extra1Original = productoSeleccionado switch { Televisor t => t.pulgadas, Heladera h => h.capacidad, Lavarropas l => l.carga, _ => 0 };
            string extra2Original = productoSeleccionado switch { Televisor t => t.tipoPantalla, Heladera h => h.tipo, Lavarropas l => l.tipo, _ => "" };
            if (extra1Actual != extra1Original) cambios.Add($"{extrasOpcion1}: [green]{extra1Actual}[/]");
            if (extra2Actual != extra2Original) cambios.Add($"{extrasOpcion2}: [green]{extra2Actual}[/]");

            if (cambios.Count > 0)
            {
                var panelCambios = new Panel(string.Join("\n", cambios))
                    .Header("[bold yellow]⚠ Campos modificados[/]")
                    .BorderColor(Color.Orange1)
                    .Border(BoxBorder.Rounded);
                AnsiConsole.Write(panelCambios);
            }
            AnsiConsole.WriteLine();
            var opcion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("¿Qué campo querés editar?")
                .HighlightStyle("bold")
                .AddChoices("Nombre", "Precio", "Cantidad", extrasOpcion1, extrasOpcion2, "Guardar cambios", "[grey]↩ Volver[/]")
            );

            //cada case modifica solo la variable local correspondiente. los cambios se guardan todos juntos al seleccionar "Guardar cambios"
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
                    //enviar todos los campos al controller que valida y pasa al service. los parametros opcionales (string?) permiten actualizacion parcial
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