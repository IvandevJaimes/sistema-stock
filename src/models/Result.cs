//clase generica para moldear los resultados de los controladores asi la interfaz puede manejarlos y mostrarlos de forma amigable al usuario
public class Result<T>
{
    public bool success { get; } //inidica si la operacion fue exitosa o no mediante true o false para que sea mas sencillo de manejar
    public T? data { get; }
    public string? mensaje { get; }
    private Result(bool success, T? data, string? mensaje)
    {
        this.success = success;
        this.data = data;
        this.mensaje = mensaje;
    }

    public static Result<T> Ok(T data) //definir como static para que sea mas rapido de usar 
    {
        var result = new Result<T>(true, data, null);
        return result;
    }

        public static Result<T> Error(string mensaje)
    {
        var result = new Result<T>(false, default, mensaje);
        return result;
    }
}