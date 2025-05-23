using System.Data;  
using Microsoft.Data.SqlClient;    // ← cambia aquí
using EmpleadosApp.Models;
using Microsoft.Extensions.Configuration;

namespace EmpleadosApp.Data
{
    public class EmpleadoRepository
    {
        private readonly string _conn;

        public EmpleadoRepository(IConfiguration config)
        {
            // <- el ! le dice al compilador "confío en que no es null"
            _conn = config.GetConnectionString("ArcGIS")!;  
        }

        public IEnumerable<Empleado> GetAll()
        {
            var lista = new List<Empleado>();
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(
               "SELECT EmpleadoID, Nombre, FechaIngreso, Latitud, Longitud FROM dbo.Empleados",
               cn);
            cn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                lista.Add(new Empleado {
                    EmpleadoID   = rd.GetInt32(0),
                    Nombre       = rd.GetString(1),
                    FechaIngreso = rd.GetDateTime(2),
                    Latitud      = rd.GetDouble(3),
                    Longitud     = rd.GetDouble(4)
                });
            }
            return lista;
        }

        public void Add(Empleado e)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(
                "INSERT INTO dbo.Empleados (Nombre, FechaIngreso, Latitud, Longitud) VALUES (@n,@f,@lat,@lon)",
                cn);
            cmd.Parameters.Add("@n", SqlDbType.NVarChar).Value   = e.Nombre;
            cmd.Parameters.Add("@f", SqlDbType.DateTime).Value   = e.FechaIngreso;
            cmd.Parameters.Add("@lat", SqlDbType.Float).Value    = e.Latitud;
            cmd.Parameters.Add("@lon", SqlDbType.Float).Value    = e.Longitud;
            cn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
