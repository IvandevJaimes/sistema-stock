using Spectre.Console;
public static class Alerta
{
    public static void Exito(string mensaje)
    {
        AnsiConsole.MarkupLine($"[bold green]✔ {mensaje}[/]");
        Thread.Sleep(2000);
        AnsiConsole.Clear();
    }
    public static void Error(string mensaje)
    {
        AnsiConsole.MarkupLine($"[bold red]✖ {mensaje}[/]");
        Thread.Sleep(2000);
        AnsiConsole.Clear();
    }
}