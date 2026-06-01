using Spectre.Console;
//clase estatica para mostrar mensajes de feedback al usuario con un delay para que pueda leerlos antes de limpiar la pantalla
public static class Alerta
{
    public static void Exito(string mensaje)
    {
        AnsiConsole.MarkupLine($"[bold green]✔ {mensaje}[/]");
        Thread.Sleep(2000); // pausa para que el usuario pueda leer el mensaje
        AnsiConsole.Clear();
    }
    public static void Error(string mensaje)
    {
        AnsiConsole.MarkupLine($"[bold red]✖ {mensaje}[/]");
        Thread.Sleep(2000);
        AnsiConsole.Clear();
    }
}