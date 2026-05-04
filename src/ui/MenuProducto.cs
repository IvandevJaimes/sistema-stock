using Spectre.Console;

public class MenuProducto
{
    private SucursalController sucursalCtrl = new SucursalController();
    private ProductoController productoCtrl = new ProductoController();

    private MenuEditar menuEditar = new MenuEditar();
    private MenuEliminar menuEliminar = new MenuEliminar();

    private MenuCrear menuCrear = new MenuCrear();
    private Producto SeleccionarProducto(List<Producto> productos)
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<Producto>()
                .HighlightStyle("bold")
                .UseConverter(p => $"{p.id} - {p.nombre} ({p.precio:C})")
                .PageSize(10)
                .AddChoices(productos)
        );
    }

    public void Ejecutar(string nombreSucursal)
    {
        var sucursal = sucursalCtrl.ObtenerSucursal(nombreSucursal);

        var titulo = new FigletText($"Sucursal: {nombreSucursal}")
            .LeftJustified()
            .Color(Color.Cyan);

        var salir = false;

        while (!salir)
        {
            AnsiConsole.Clear();
            var productos = productoCtrl.ListarProductos(nombreSucursal);

            AnsiConsole.Write(titulo);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[cyan]Productos totales: {productos.Count()}[/]");
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

            foreach (var p in productos)
            {
                tabla.AddRow(
                    $"[cyan]{p.id}[/]",
                    $"[green]{p.nombre}[/]",
                    $"[magenta]{p.GetType().Name}[/]",
                    $"[yellow]{p.precio:C}[/]",
                    $"[red]{p.cantidad}[/]",
                    $"[grey]{p.ObtenerDetalles()}[/]"
                );
            }
            AnsiConsole.Write(tabla);

            var opcion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Elegir opción:[/]")
                    .HighlightStyle("bold")
                    .AddChoices(
                        "Crear producto",
                        "Editar producto",
                        "Eliminar producto",
                        "[grey]↩ Volver[/]"
                    )
            );

            switch (opcion)
            {
                case "Crear producto":
                    menuCrear.EjecutarCrear(nombreSucursal);
                    break;
                case "Editar producto":
                    AnsiConsole.Markup("[green]Seleccionar producto a editar:[/]");
                    AnsiConsole.WriteLine();
                    AnsiConsole.WriteLine();
                    if (productos.Count == 0)
                    {
                        Alerta.Error("Sin productos para editar");
                        break;
                    }
                    var productoSeleccionado = SeleccionarProducto(productos);
                    menuEditar.EjecutarEditar(nombreSucursal, productoSeleccionado);
                    break;

                case "Eliminar producto":
                    AnsiConsole.Markup("[red]Seleccionar producto a eliminar:[/]");
                    AnsiConsole.WriteLine();
                    AnsiConsole.WriteLine();
                    if (productos.Count == 0)
                    {
                        Alerta.Error("Sin productos para eliminar");
                        break;
                    }
                    var productoSeleccionadoABorrar = SeleccionarProducto(productos);
                    menuEliminar.EjecutarEliminar(nombreSucursal, productoSeleccionadoABorrar);
                    break;

                case "[grey]↩ Volver[/]":
                    salir = true;
                    AnsiConsole.Clear();
                    break;
            }
        }
    }
}