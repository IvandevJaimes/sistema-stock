using MySqlConnector;
using DotNetEnv;
public class ConexionDB {

    static ConexionDB() {
        Env.Load();
    }
    private string conexionString = Environment.GetEnvironmentVariable("CONEXION_STRING") ?? "";

    public MySqlConnection AbrirConexion () {
        try{    
            var conexion = new MySqlConnection(conexionString);
            if (conexion == null) {
                throw new Exception("No se pudo crear la conexión a la base de datos.");
            }
            conexion.Open();
            return conexion;
        } catch (Exception ex) {
            
            throw new Exception("Error al conectar a la base de datos: " + ex.Message);
        }

    }
}