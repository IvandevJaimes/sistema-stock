using Spectre.Console;

//pantalla de busqueda de productos por nombre o codigo con resultados en tabla y opciones de editar/eliminar desde los resultados
public class MenuBuscar
{
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
    public async Task EjecutarBuscar(String nombreSucursal)
    {
       

        var titulo = new FigletText($"Sucursal: {nombreSucursal}")
            .LeftJustified()
            .Color(Color.Cyan);


        var panel = new Panel("[bold white]Buscar y gestionar producto[/]")
            .Border(BoxBorder.Rounded)
            .BorderStyle(new Style(Color.Orange1));
        var salir = false;

        List<Producto> resultado = new(); //lista de resultados que se va llenando con las busquedas y persiste entre iteraciones

        while (!salir)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(titulo);
            AnsiConsole.Write(panel);
            //tabla que muestra los resultados de la ultima busqueda. se actualiza cada vez que se hace una nueva busqueda
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


            if (resultado.Count() > 0)
            {
                foreach (var p in resultado)
                {
                    tabla.AddRow(
                        $"[cyan]{p.id}[/]",
                        $"[cyan]{p.codigo}[/]",
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
                    "Buscar producto por nombre",
                    "Buscar producto por codigo",
                    "Editar producto",
                    "Eliminar producto",
                    "[grey]↩ Volver[/]"
                )
            );
            AnsiConsole.WriteLine();
            switch (opcion)
            {

                case "Buscar producto por nombre":
                    //busqueda parcial case-insensitive que devuelve todos los productos que contengan el termino en el nombre
                    var termino = AnsiConsole.Ask<string>("Ingresar termino de busqueda:");
                    var respuesta = await productoController.BuscarProductoPorNombre(nombreSucursal, termino);

                    if (!respuesta.success)
                    {
                        resultado.Clear();
                        Alerta.Error(respuesta?.mensaje ?? "Ocurrió un error");
                        break;
                    }
                    if (respuesta.data == null || respuesta.data.Count == 0)
                    {
                        resultado.Clear();
                        Alerta.Error(respuesta?.mensaje ?? "Ocurrió un error");
                        break;
                    }

                    resultado = respuesta.data ?? new List<Producto>();
                    break;
                case "Buscar producto por codigo":
                    //busqueda exacta por codigo numerico unico por sucursal
                    var codigo = AnsiConsole.Ask<int>("Ingresar codigo de producto:");
                    var respuestaCodigo = await productoController.BuscarProductoPorCodigo(nombreSucursal, codigo);

                    if (!respuestaCodigo.success)
                    {
                        resultado.Clear();
                        Alerta.Error(respuestaCodigo?.mensaje ?? "Ocurrió un error");
                        break;
                    }
                    if (respuestaCodigo.data == null)
                    {
                        resultado.Clear();
                        Alerta.Error(respuestaCodigo?.mensaje ?? "Ocurrió un error");
                        break;
                    }

                    resultado = respuestaCodigo.data ?? new List<Producto>() ;
                    break;
                case "Editar producto":
                    //editar un producto directamente desde los resultados de la busqueda
                    if (resultado.Count == 0)
                    {
                        Alerta.Error("Debe hacer una busqueda si desea editar un producto");
                        break;
                    }
                    else
                    {
                        var productoSeleccionado = SeleccionarProducto(resultado);
                        await menuEditar.EjecutarEditar(nombreSucursal, productoSeleccionado);
                        resultado.Remove(productoSeleccionado); //remover de la lista local porque ya no existe en la bd (se actualizo y podria tener otro nombre/codigo)
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
                        await menuEliminar.EjecutarEliminar(nombreSucursal, productoSeleccionado);
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