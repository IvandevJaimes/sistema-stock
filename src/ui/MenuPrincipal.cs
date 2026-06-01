using Spectre.Console;
public class MenuPrincipal
{
    private MenuSucursal menuSucursal = new MenuSucursal();
    public async Task Ejecutar()
    {
      

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
            var opcion = AnsiConsole.Prompt(

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
