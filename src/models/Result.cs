public class Result<T>
{
    public bool success { get; }
    public T? data { get; }
    public string? mensaje { get; }
    private Result(bool success, T? data, string? mensaje)
    {
        this.success = success;
        this.data = data;
        this.mensaje = mensaje;
    }

    public static Result<T> Ok(T data)
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