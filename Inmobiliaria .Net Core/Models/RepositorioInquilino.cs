using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
	public class RepositorioInquilino : RepositorioBase, IRepositorio<Inquilino>
	{
		public RepositorioInquilino(IConfiguration configuration) : base(configuration)
		{
			
		}

        public int Alta(Inquilino p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Inquilino (Nombre, Apellido, Dni, Telefono, Email,DireccionTrabajo,NombreGarante,DniGarante,TelGarante) " +
                    $"VALUES ('{p.Nombre}', '{p.Apellido}','{p.Dni}','{p.Telefono}','{p.Email}','{p.DireccionTrabajo}','{p.NombreGarante}','{p.DniGarante}','{p.TelGarante}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    p.IdInquilino = Convert.ToInt32(id);
                    connection.Close();
                }
            }
            return res;
        }
        public int Baja(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM Inquilino WHERE Id = {id}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        public int Modificacion(Inquilino i)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Inquilino SET Nombre='{i.Nombre}', Apellido='{i.Apellido}', Dni='{i.Dni}', Telefono='{i.Telefono}', Email='{i.Email}', DireccionTrabajo='{i.DireccionTrabajo}', NombreGarante='{i.NombreGarante}', DniGarante='{i.DniGarante}', TelGarante='{i.TelGarante}' " +
					$"WHERE Id = {i.IdInquilino}";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

        public IList<Inquilino> ObtenerTodos()
        {
            IList<Inquilino> res = new List<Inquilino>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT Id, Nombre, Apellido, Email,Telefono,  DireccionTrabajo,NombreGarante,DniGarante,TelGarante,Dni" +
                    $" FROM Inquilino";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inquilino i = new Inquilino
                        {
                            IdInquilino = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            DireccionTrabajo = reader.GetString(5),
                            NombreGarante =reader.GetString(6),
                            DniGarante = reader.GetString(7),
                            TelGarante = reader.GetString(8),
                            Dni = reader.GetString(9)
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Inquilino ObtenerPorId(int id)
        {
            Inquilino i = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Email, DireccionTrabajo,NombreGarante, DniGarante,TelGarante FROM Inquilino" +
                    $" WHERE Id=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        i = new Inquilino
                        {
                            IdInquilino = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            DireccionTrabajo = reader.GetString(6),
                            NombreGarante =reader.GetString(7),
                            DniGarante = reader.GetString(8),
                            TelGarante = reader.GetString(9)
                        };
                        return i;
                    }
                    connection.Close();
                }
            }
            return i;
        }
        public Inquilino Update(Inquilino i) { throw new NotImplementedException(); }
    }
  
}
