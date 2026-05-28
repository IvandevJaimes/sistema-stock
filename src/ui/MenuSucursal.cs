using Spectre.Console;
public class MenuSucursal
{
    private SucursalController sucursalCtrl = new SucursalController();
    private MenuProducto menuProducto = new MenuProducto();
    private MenuBuscar menuBuscar = new MenuBuscar();

    private MenuVender menuVender = new MenuVender();
    private MenuVentas menuVentas = new MenuVentas();
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

            AnsiConsole.Write(titulo);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[purple]Direccion: {sucursal?.direccion}[/]");
            AnsiConsole.MarkupLine($"[cyan]Productos totales: {sucursal?.stock.Count()}[/]");
            AnsiConsole.WriteLine();
            var opcion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Elegir opcion: [/]")
                    .HighlightStyle("bold")
                    .AddChoices("Gestionar", "Buscar producto","Vender producto", "Ver ventas", "[grey]↩ Volver[/]")
            );

            switch (opcion)
            {
                case "Gestionar": menuProducto.Ejecutar(nombreSucursal);
                    break;

                case "Buscar producto":
                    menuBuscar.EjecutarBuscar(nombreSucursal);
                    break;

                case "Vender producto":
                    menuVender.EjecutarVender(nombreSucursal);
                    break;

                case "Ver ventas":
                    menuVentas.Ejecutar(nombreSucursal);
                    break;

                case "[grey]↩ Volver[/]":
                    salir = true; AnsiConsole.Clear();
                    break;
            }
        }

    }
}