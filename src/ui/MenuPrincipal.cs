using Spectre.Console;
//pantalla principal del sistema con seleccion de sucursal. es el entry point de toda la interfaz
public class MenuPrincipal
{
    private SucursalController sucursalCtrl = new SucursalController();
    private MenuSucursal menuSucursal = new MenuSucursal();
    public async Task Ejecutar()
    {
        var sucursales = await sucursalCtrl.ListarSucursales(); //cargar las sucursales disponibles desde la base de datos


        var titulo = new FigletText("Sistema de Stock")
            .LeftJustified()
            .Color(Color.Yellow);

        var salir = false;

        while (!salir)
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(titulo);
            AnsiConsole.Write(new Rule("[grey]Sistema de gestión de inventario[/]").RuleStyle("grey").Centered());
            AnsiConsole.MarkupLine("[grey]Versión: 1.0.0 | .NET 10[/]");
            AnsiConsole.WriteLine();
            if (sucursales.success == false)
            {
                AnsiConsole.MarkupLine($"[red]{sucursales.mensaje}[/]");
                AnsiConsole.MarkupLine("[grey]Presiona cualquier tecla para salir...[/]");
                Console.ReadKey();
                AnsiConsole.Clear();
                return;
            }
            else
            {
                var opcion = AnsiConsole.Prompt(
                //menu de seleccion de sucursal. actualmente las sucursales estan hardcodeadas pero se pueden obtener desde la base de datos si se desea
                new SelectionPrompt<string>()
                    .Title("[blue]Elegir sucursal: [/]")
                    .HighlightStyle("bold")
                    .AddChoices("Centro", "Norte", "[grey]x Cerrar programa[/]")
                );
                switch (opcion)
                {
                    case "Centro": await menuSucursal.Ejecutar("Centro"); break;
                    case "Norte": await menuSucursal.Ejecutar("Norte"); break;
                    case "[grey]x Cerrar programa[/]": salir = true; AnsiConsole.Clear(); break;

                }
            }


        }
    }
}
