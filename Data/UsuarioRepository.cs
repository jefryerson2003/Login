using EmpleadosApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace EmpleadosApp.Data
{
    public class UsuarioRepository
    {
        private readonly string _conn;
        public UsuarioRepository(IConfiguration config)
            => _conn = config.GetConnectionString("ArcGIS")!;

        // Valida credenciales y devuelve los roles del usuario
        public (Usuario? user, List<string> roles) Validate(string username, string password)
        {
            Usuario? u = null;
            var roles = new List<string>();

            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(
              @"SELECT UsuarioId, Username 
                FROM Usuarios 
                WHERE Username=@u AND [Password]=@p", cn);
            cmd.Parameters.Add("@u", SqlDbType.NVarChar).Value = username;
            cmd.Parameters.Add("@p", SqlDbType.NVarChar).Value = password;
            cn.Open();
            using var rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                u = new Usuario {
                   UsuarioId = rd.GetInt32(0),
                   Username  = rd.GetString(1),
                };
            }
            rd.Close();

            if (u != null)
            {
                using var cmd2 = new SqlCommand(
                   @"SELECT R.RoleName
                     FROM UsuarioRoles UR
                     JOIN Roles R ON UR.RoleId = R.RoleId
                     WHERE UR.UsuarioId=@uid", cn);
                cmd2.Parameters.Add("@uid", SqlDbType.Int).Value = u.UsuarioId;
                using var rd2 = cmd2.ExecuteReader();
                while (rd2.Read())
                    roles.Add(rd2.GetString(0));
            }

            return (u, roles);
        }
    }
}
