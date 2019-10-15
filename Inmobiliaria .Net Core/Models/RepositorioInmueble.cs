using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
	public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
		public RepositorioInmueble(IConfiguration configuration) : base(configuration)
		{
			
		}

		public int Alta(Inmueble entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
                
				string sql = $"INSERT INTO Inmuebles (Direccion, Ambientes, Precio, Tipo, Uso, Disponible, PropietarioId) " +
					$"VALUES ('{entidad.Direccion}', '{entidad.Ambientes}','{entidad.Precio}','{entidad.Tipo}','{entidad.Uso}','1','{entidad.PropietarioId}')";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    entidad.Id = Convert.ToInt32(id);
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
				string sql = $"DELETE FROM Inmuebles WHERE Id = {id}";
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
		public int Modificacion(Inmueble inmueble)
		{
     
            int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
                string sql = $"UPDATE Inmuebles SET Direccion=@direccion, Ambientes=@ambientes,Precio=@precio, Tipo=@tipo, Uso=@uso, Disponible=@disponible, PropietarioId=@propietarioId " +
					$"WHERE Id = {inmueble.Id}";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
                    command.Parameters.Add("@direccion", SqlDbType.VarChar).Value = inmueble.Direccion;
                    command.Parameters.Add("@ambientes", SqlDbType.Int).Value = inmueble.Ambientes;
                    command.Parameters.Add("@precio", SqlDbType.Decimal).Value = inmueble.Precio;
                    command.Parameters.Add("@tipo", SqlDbType.VarChar).Value = inmueble.Tipo;
                    command.Parameters.Add("@uso", SqlDbType.VarChar).Value = inmueble.Uso;
                    command.Parameters.Add("@disponible", SqlDbType.Int).Value = inmueble.Disponible;
                    command.Parameters.Add("@propietarioId", SqlDbType.Int).Value = inmueble.PropietarioId;
                    command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Inmueble> ObtenerTodos()
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Direccion, Ambientes, Precio,Tipo, Uso, Disponible, PropietarioId, p.Nombre, p.Apellido" +
                    $" FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.IdPropietario";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble entidad = new Inmueble
						{
							Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Precio = reader.GetDecimal(3),
                            Tipo = reader.GetString(4),
                            Uso= reader.GetString(5),
                            Disponible = reader.GetInt32(6),
                            PropietarioId = reader.GetInt32(7),
                            Duenio = new Propietario
                            {
                                IdPropietario = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
                            }
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Inmueble ObtenerPorId(int id)
		{
			Inmueble entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
                string sql = $"SELECT Id, Direccion, Ambientes, Precio,Tipo, Uso, Disponible, PropietarioId, p.Nombre, p.Apellido" +
                    $" FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.IdPropietario" +
                    $" WHERE Id=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						entidad = new Inmueble
						{
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Precio = reader.GetDecimal(3),
                            Tipo = reader.GetString(4),
                            Uso = reader.GetString(5),
                            Disponible = reader.GetInt32(6),
                            PropietarioId = reader.GetInt32(7),
                            Duenio = new Propietario
                            {
                                IdPropietario = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
                            }
                        };
					}
					connection.Close();
				}
			}
			return entidad;
        }

        public IList<Inmueble> BuscarPorPropietario(int idPropietario)
        {
            List<Inmueble> res = new List<Inmueble>();
            Inmueble entidad = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT Id, Direccion, Ambientes, Precio, Tipo, Uso, Disponible, PropietarioId, p.Nombre, p.Apellido" +
                    $" FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.IdPropietario" +
                    $" WHERE PropietarioId=@idPropietario";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@idPropietario", SqlDbType.Int).Value = idPropietario;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        entidad = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Precio = reader.GetDecimal(3),
                            Tipo = reader.GetString(4),
                            Uso = reader.GetString(5),
                            Disponible = reader.GetInt32(6),
                            PropietarioId = reader.GetInt32(7),
                            Duenio = new Propietario
                            {
                                IdPropietario = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
                            }
                        };
                        res.Add(entidad);
                    }
                    connection.Close();
                }
            }
            return res;
        }
        public Inmueble Update(Inmueble i) { throw new NotImplementedException(); }
        public Inmueble FinalizarContrato(Inmueble i) { throw new NotImplementedException(); }
    }
}
