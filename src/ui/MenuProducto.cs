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
                .UseConverter(p => $"{p.id} - {p.codigo} - {p.nombre} ({p.precio:C} - {p.cantidad} unidades) - {p.GetType().Name}")
                .PageSize(10)
                .AddChoices(productos)
        );
    }

    public async Task Ejecutar(string nombreSucursal)
    {
        var sucursal = await sucursalCtrl.ObtenerSucursal(nombreSucursal);

        var titulo = new FigletText($"Sucursal: {nombreSucursal}")
            .LeftJustified()
            .Color(Color.Cyan);

        var salir = false;

        while (!salir)
        {
            AnsiConsole.Clear();
            var productos = await productoCtrl.ListarProductos(nombreSucursal);
            if (productos.data == null)
            {
                if (productos.success)
                {
                    AnsiConsole.MarkupLine($"[yellow]No hay productos en la sucursal {nombreSucursal}[/]");
                    AnsiConsole.MarkupLine($"[green]Puedes agregar productos desde la opción 'Gestionar'[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]{productos.mensaje}[/]");
                }
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]Presiona cualquier tecla para volver...[/]");
                Console.ReadKey();
                return;
            }
            var listaProductos = productos.data;
            AnsiConsole.Write(titulo);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[cyan]Productos totales: {listaProductos.Count()}[/]");
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

            foreach (var p in listaProductos)
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
                    await menuCrear.EjecutarCrear(nombreSucursal);
                    break;
                case "Editar producto":
                    AnsiConsole.Markup("[green]Seleccionar producto a editar:[/]");
                    AnsiConsole.WriteLine();
                    AnsiConsole.WriteLine();
                    if (listaProductos.Count() == 0)
                    {
                        Alerta.Error("Sin productos para editar");
                        break;
                    }
                    var productoSeleccionado = SeleccionarProducto(listaProductos.ToList());
                    await menuEditar.EjecutarEditar(nombreSucursal, productoSeleccionado);
                    break;

                case "Eliminar producto":
                    AnsiConsole.Markup("[red]Seleccionar producto a eliminar:[/]");
                    AnsiConsole.WriteLine();
                    AnsiConsole.WriteLine();
                    if (listaProductos.Count() == 0)
                    {
                        Alerta.Error("Sin productos para eliminar");
                        break;
                    }
                    var productoSeleccionadoABorrar = SeleccionarProducto(listaProductos.ToList());
                     await menuEliminar.EjecutarEliminar(nombreSucursal, productoSeleccionadoABorrar);
                    break;

                case "[grey]↩ Volver[/]":
                    salir = true;
                    AnsiConsole.Clear();
                    break;
            }
        }
    }
}