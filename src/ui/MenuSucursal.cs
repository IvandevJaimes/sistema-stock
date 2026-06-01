using Spectre.Console;
//menu central de cada sucursal. desde aca se accede a gestion, busqueda, ventas e historial
public class MenuSucursal
{
    private ProductoController productoCtrl = new ProductoController();
    private MenuProducto menuProducto = new MenuProducto();
    private MenuBuscar menuBuscar = new MenuBuscar();

    private MenuVender menuVender = new MenuVender();
    private MenuVentas menuVentas = new MenuVentas();
    public async Task Ejecutar(string nombreSucursal)
    {
        //cargar los productos al inicio para conocer la cantidad disponible y mostrar feedback rapido
        var productos = await productoCtrl.ListarProductos(nombreSucursal);

        if (productos.data == null)
        {
            AnsiConsole.MarkupLine($"[yellow]No hay productos en la sucursal {nombreSucursal}[/]");
            AnsiConsole.MarkupLine($"[green]Puedes agregar productos desde la opción 'Gestionar'[/]");
        }
        else if (productos.success == false)
        {
            AnsiConsole.MarkupLine($"[red]{productos.mensaje}[/]");
            return;
        }

        var titulo = new FigletText($"Sucursal: {nombreSucursal}")
            .LeftJustified()
            .Color(Color.Cyan);


        var salir = false;

        while (!salir)
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(titulo);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[cyan]Productos totales: {productos.data?.Count() ?? 0}[/]");
            AnsiConsole.WriteLine();
            var opcion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Elegir opcion: [/]")
                    .HighlightStyle("bold")
                    .AddChoices("Gestionar", "Buscar producto","Vender producto", "Ver ventas", "[grey]↩ Volver[/]")
            );

            switch (opcion)
            {
                case "Gestionar": await menuProducto.Ejecutar(nombreSucursal);
                    break;

                case "Buscar producto":
                    await menuBuscar.EjecutarBuscar(nombreSucursal);
                    break;

                case "Vender producto":
                    await menuVender.EjecutarVender(nombreSucursal);
                    break;

                case "Ver ventas":
                    await menuVentas.Ejecutar(nombreSucursal);
                    break;

                case "[grey]↩ Volver[/]":
                    salir = true; AnsiConsole.Clear();
                    break;
            }
        }

    }
}