using System.Diagnostics;
using Spectre.Console;
public class MenuVender
{
    private ProductoController productoCtrl = new ProductoController();
    List<CarritoItem> carrito = new List<CarritoItem>();

    private MenuComprobante comprobante = new MenuComprobante();
    public void EjecutarVender(string nombreSucursal)
    {
        AnsiConsole.Clear();

        var titulo = new FigletText($"Sucursal: {nombreSucursal}")
            .LeftJustified()
            .Color(Color.Cyan);

        var productos = productoCtrl.ListarProductos(nombreSucursal);

        var panel = new Panel("[bold white]Vender productos[/]")
            .Border(BoxBorder.Rounded)
            .BorderStyle(new Style(Color.Purple));




        var salir = false;
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
            tabla.AddColumn("[bold yellow]Precio[/]");
            tabla.AddColumn("[bold yellow]Cantidad[/]");
            tabla.AddColumn("[bold yellow]Subtotal[/]");

            if (carrito.Count > 0)
            {
                foreach (var p in carrito)
                {
                    tabla.AddRow(
                        $"[cyan]{p.id}[/]",
                        $"[green]{p.nombre}[/]",
                        $"[yellow]{p.precio:C}[/]",
                        $"[red]{p.cantidad}[/]",
                        $"[magenta]{p.cantidad * p.precio:C}[/]"
                    );
                }
                AnsiConsole.Write(tabla);
            }
            else
            {
                tabla.AddRow(
                    "[cyan]-[/]",
                    $"[green]-[/]",
                    $"[magenta]-[/]",
                    $"[yellow]-[/]",
                    $"[red]-[/]"
                );
                AnsiConsole.Write(tabla);
            }
            decimal total = carrito.Sum(x => x.precio * x.cantidad);
            AnsiConsole.MarkupLine($"[bold green]Total: {total:C}[/]");
            AnsiConsole.WriteLine();

            var opcion = AnsiConsole.Prompt(
               new SelectionPrompt<string>()
                   .Title("[blue]Elegir opción: [/]")
                   .HighlightStyle("bold")
                   .AddChoices("Agregar al carrito", "Quitar del carrito", "Vaciar carrito", "Vender", "[grey]↩ Volver[/]")
           );

            switch (opcion)
            {
                case "Agregar al carrito":

                    if (productos.Count == 0)
                    {
                        Alerta.Error("Sin productos para vender");
                        break;
                    }
                    var productoSeleccionado = AnsiConsole.Prompt(
                        new SelectionPrompt<Producto>()
                            .Title("[blue]Seleccionar producto:[/]")
                            .UseConverter(p =>
                            {
                                var enCarritoTemp = carrito.Find(x => x.id == p.id)?.cantidad ?? 0;
                                var disponible = p.cantidad - enCarritoTemp;
                                return $"{p.nombre} (Disp: {disponible})";
                            })
                            .AddChoices(productos)
                    );

                    var existente = carrito.Find(x => x.id == productoSeleccionado.id);
                    int enCarrito = existente?.cantidad ?? 0;

                    int disponibleReal = productoSeleccionado.cantidad - enCarrito;

                    if (disponibleReal <= 0)
                    {
                        Alerta.Error("No hay stock disponible para este producto");
                        break;
                    }

                    var cantidadSeleccionada = AnsiConsole.Prompt(
                        new TextPrompt<int>($"[green]Cantidad (disponible: {disponibleReal}):[/]")
                            .Validate(c => c > 0 && c <= disponibleReal
                                ? ValidationResult.Success()
                                : ValidationResult.Error("[red]Stock insuficiente[/]"))
                    );

                    if (existente != null)
                    {
                        existente.cantidad += cantidadSeleccionada;
                    }
                    else
                    {
                        carrito.Add(new CarritoItem
                        {
                            id = productoSeleccionado.id,
                            nombre = productoSeleccionado.nombre,
                            cantidad = cantidadSeleccionada,
                            precio = productoSeleccionado.precio
                        });
                    }

                    Alerta.Exito("Agregado al carrito");
                    break;

                case "Quitar del carrito":
                    if (carrito.Count == 0) { Alerta.Error("No hay productos agregados al carrito"); break; }
                    var productoSeleccionadoQuitar = AnsiConsole.Prompt(
                        new SelectionPrompt<CarritoItem>()
                            .Title("[blue]Seleccionar producto a quitar:[/]")
                            .UseConverter(p => $"{p.nombre} (Cant: {p.cantidad})")
                            .AddChoices(carrito)
                    );
                    carrito.Remove(productoSeleccionadoQuitar);
                    Alerta.Exito("Se removió el producto");
                    break;

                case "Vaciar carrito":
                    if (carrito.Count == 0) { Alerta.Error("No hay productos agregados al carrito"); break; }
                    carrito.Clear();
                    Alerta.Exito("Se vació el carrito");
                    break;

                case "Vender":
                    decimal ingresos = 0;
                    if (carrito.Count == 0) { Alerta.Error("No hay productos agregados al carrito"); break; }

                    bool error = false;

                    foreach (var p in carrito)
                    {
                        var resultado = productoCtrl.VenderProducto(nombreSucursal, p.id, p.cantidad);

                        if (!resultado.success)
                        {
                            Alerta.Error(resultado.mensaje ?? "Error desconocido");
                            error = true;
                            break;
                        }

                        ingresos += resultado.data;
                    }

                    if (!error)
                    {
                        comprobante.Generar(nombreSucursal, carrito, ingresos);
                        carrito.Clear();
                        salir = true;
                    }
                    break;

                case "[grey]↩ Volver[/]":
                    salir = true;
                    carrito.Clear();
                    break;
            }

        }

    }
}