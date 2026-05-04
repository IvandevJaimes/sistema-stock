using Spectre.Console;

public class MenuBuscar
{
    private SucursalController sucursalCtrl = new SucursalController();
    private ProductoController productoController = new ProductoController();

    private MenuEditar menuEditar = new MenuEditar();
    private MenuEliminar menuEliminar = new MenuEliminar();

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
    public void EjecutarBuscar(String nombreSucursal)
    {
        var sucursal = sucursalCtrl.ObtenerSucursal(nombreSucursal);

        var titulo = new FigletText($"Sucursal: {nombreSucursal}")
            .LeftJustified()
            .Color(Color.Cyan);


        var panel = new Panel("[bold white]Buscar y gestionar producto[/]")
            .Border(BoxBorder.Rounded)
            .BorderStyle(new Style(Color.Orange1));
        var salir = false;

        List<Producto> resultado = new();

        while (!salir)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(titulo);
            AnsiConsole.Write(panel);
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


            if (resultado.Count() > 0)
            {
                foreach (var p in resultado)
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
                AnsiConsole.MarkupLine($"[cyan]Resultados: {resultado.Count()}[/]");
                AnsiConsole.Write(tabla);
            }

            var opcion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[blue]Elegir opción:[/]")
                .HighlightStyle("bold")
                .AddChoices(
                    "Buscar producto",
                    "Editar producto",
                    "Eliminar producto",
                    "[grey]↩ Volver[/]"
                )
            );
            AnsiConsole.WriteLine();
            switch (opcion)
            {

                case "Buscar producto":

                    var termino = AnsiConsole.Ask<string>("Ingresar termino de busqueda:");
                    var respuesta = productoController.BuscarProducto(nombreSucursal, termino);

                    if (respuesta == null || !respuesta.success)
                    {
                        resultado.Clear();
                        Alerta.Error(respuesta?.mensaje ?? "Ocurrió un error");
                        break;
                    }

                    resultado = respuesta.data ?? new List<Producto>();
                    break;

                case "Editar producto":
                    if (resultado.Count == 0)
                    {
                        Alerta.Error("Debe hacer una busqueda si desea editar un producto");
                        break;
                    }
                    else
                    {
                        var productoSeleccionado = SeleccionarProducto(resultado);
                        menuEditar.EjecutarEditar(nombreSucursal, productoSeleccionado);
                        resultado.Remove(productoSeleccionado);
                    }
                    break;

                case "Eliminar producto":
                    if (resultado.Count == 0)
                    {
                        Alerta.Error("Debe hacer una busqueda si desea eliminar un producto");
                        break;
                    }
                    else
                    {
                        var productoSeleccionado = SeleccionarProducto(resultado);
                        menuEliminar.EjecutarEliminar(nombreSucursal, productoSeleccionado);
                        resultado.Remove(productoSeleccionado);
                    }
                    break;

                case "[grey]↩ Volver[/]":
                    salir = true;
                    break;
            }
        }
    }
}