using MySqlConnector;
using DotNetEnv;

//clase encargada de crear la conexion a la db. es estatica porque no necesita mantener estado, solo abrir la conexion cada vez que se necesite
public static class ConexionDB {

    private static string conexionString = "";

    static ConexionDB() {
        Env.Load(); // cargar las variables de entorno desde el archivo .env
        conexionString = Environment.GetEnvironmentVariable("CONEXION_STRING") ?? "";
    }

    // metodo para abrir la conexión a la base de datos de forma asíncrona
    public static async Task<MySqlConnection> AbrirConexion () {
        // usar el generico task para devolver la conexión abierta a la base de datos
        try{    
            var conexion = new MySqlConnection(conexionString);
            if (conexion == null) {
                throw new Exception("No se pudo crear la conexión a la base de datos.");
            }
            await conexion.OpenAsync();
            return conexion;
        } catch (Exception ex) {
            
            throw new Exception("Error al conectar a la base de datos: " + ex.Message);
        }

    }
}