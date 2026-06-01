public class Heladera : Producto //clase hija que hereda atributos y comportamientos de producto
{
    public Heladera(int Id, int Codigo, string Nombre, decimal Precio, int Cantidad, double Capacidad, string Tipo)
        : base(Id, Codigo, Nombre, Precio, Cantidad) //base para llamar al constructor del padre y asignar los atributos heredados
    {
        capacidad = Capacidad;
        tipo = Tipo;
    }
    public double capacidad { get; protected set; }
    public string tipo { get; protected set; }
    //atributos especificos de la heladera, con setter protegido para que solo puedan ser modificados desde la clase o sus hijas
    public override string ObtenerDetalles() //metodo override que sobreescribe el metodo del padre
    {
        return $"Capacidad: {capacidad}L | Tipo: {tipo}";
    }
}