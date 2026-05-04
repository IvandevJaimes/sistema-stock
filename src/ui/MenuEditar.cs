using Spectre.Console;
public class MenuEditar
{
    private ProductoController productoCtrl = new ProductoController();

    public Result<Producto>? EjecutarEditar(string nombreSucursal, Producto productoSeleccionado)
    {
        string nombreActual = productoSeleccionado.nombre;
        decimal precioActual = productoSeleccionado.precio;
        int cantidadActual = productoSeleccionado.cantidad;

        Result<Producto>? nuevo = null;

        var titulo = new FigletText($"Sucursal: {nombreSucursal}")
        .LeftJustified()
        .Color(Color.Cyan);


        var panel = new Panel("[bold cyan]Editar Producto[/]")
            .Border(BoxBorder.Rounded)
            .BorderStyle(new Style(Color.Cyan));

        var salir = false;

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
                .AddChoices("Nombre", "Precio", "Cantidad", "Guardar cambios", "[grey]↩ Volver[/]")
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

                case "Guardar cambios":
                    var resultado = productoCtrl.ActualizarProducto(
                        nombreSucursal,
                        productoSeleccionado.id,
                        nombreActual,
                        precioActual,
                        cantidadActual
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