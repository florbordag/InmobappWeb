using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
    public class RepositorioAlquiler : RepositorioBase, IRepositorio<Alquiler>
    {
        public RepositorioAlquiler(IConfiguration configuration) : base(configuration){

        }

        public int Alta(Alquiler a)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string sql = $"INSERT INTO Alquiler (Inquilino,Inmueble,FechaInicio,FechaFin, MontoTotal) " +
                    $"VALUES ('{a.IdInquilino}','{a.IdInmueble}','{a.FechaInicio}','{a.FechaFin}','{a.MontoTotal}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    a.IdContrato = Convert.ToInt32(id);
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
                string sql = $"DELETE FROM Alquiler WHERE IdContrato = {id}";
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
        public int Modificacion(Alquiler a)
        {

            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Alquiler SET FechaFin=@fechaFin, Multa='{a.Multa}' " +
                    $"WHERE IdContrato = {a.IdContrato}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@fechaFin", SqlDbType.VarChar).Value = a.FechaFin;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Alquiler Update(Alquiler a)
        {

            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string fechavieja = a.FechaFin; string fechanueva;
                string sql = $"UPDATE Alquiler SET FechaFin=@fechaFin " +
                    $"WHERE IdContrato = {a.IdContrato}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@fechaFin", SqlDbType.VarChar).Value = a.FechaFin;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    
                    connection.Close();
                }
                a = ObtenerPorId(a.IdContrato);
                fechanueva = a.FechaFin;
                a.ProximoFin = fechanueva;
                a.FechaFin = fechavieja;
                sql = $"UPDATE Alquiler SET FechaFin='{fechavieja}' " +
                 $"WHERE IdContrato = {a.IdContrato}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@fechaFin", SqlDbType.VarChar).Value = a.FechaFin;

                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return a;
        }

        public Alquiler ObtenerPorId(int id)
        {
            Alquiler a = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.IdContrato, p.Nombre, p.Apellido, i.Nombre, i.Apellido, d.Direccion, c.FechaInicio,c.FechaFin, c.MontoTotal,c.Multa, c.Inmueble, d.PropietarioId, i.Id, d.Precio " +
                    $" FROM Alquiler c, Propietarios p, Inquilino i, Inmuebles d WHERE c.Inmueble=d.Id AND d.PropietarioId=p.IdPropietario AND c.Inquilino=i.Id AND c.IdContrato=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        a = new Alquiler
                        {
                            IdContrato = reader.GetInt32(0),
                            FechaInicio = reader.GetString(6),
                            MontoTotal = reader.GetDecimal(8),
                            Inmueble = new Inmueble
                            {
                                Duenio = new Propietario
                                {
                                    IdPropietario = reader.GetInt32(11),
                                    Nombre = reader.GetString(1),
                                    Apellido = reader.GetString(2)
                                },
                                Id = reader.GetInt32(10),
                                Direccion = reader.GetString(5),
                                Precio = reader.GetDecimal(13)
                            },
                            Inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(12),
                                Nombre = reader.GetString(3),
                                Apellido = reader.GetString(4)
                            },
                            Multa = reader.IsDBNull(9) ? 0 : reader.GetDecimal(9),
                            FechaFin = reader.GetString(7),
                        };
                    }
                    connection.Close();
                }
            }
            return a;
        }



        public IList<Alquiler> ObtenerTodos()
        {
            IList<Alquiler> res = new List<Alquiler>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.IdContrato, p.Nombre, p.Apellido, i.Nombre, i.Apellido, d.Direccion, c.FechaInicio,c.FechaFin, c.MontoTotal,c.Multa, c.Inmueble, d.PropietarioId, i.Id " +
                    $" FROM Alquiler c, Propietarios p, Inquilino i, Inmuebles d WHERE c.Inmueble=d.Id AND d.PropietarioId=p.IdPropietario AND c.Inquilino=i.Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Alquiler a = new Alquiler
                        {
                            IdContrato = reader.GetInt32(0),
                            FechaInicio = reader.GetString(6),
                            MontoTotal = reader.GetDecimal(8),
                            Inmueble = new Inmueble
                            {
                                Duenio = new Propietario
                                {
                                    IdPropietario = reader.GetInt32(11),
                                    Nombre = reader.GetString(1),
                                    Apellido = reader.GetString(2)
                                },
                                Id = reader.GetInt32(10),
                                Direccion = reader.GetString(5)
                            },
                            Inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(12),
                                Nombre = reader.GetString(3),
                                Apellido = reader.GetString(4)
                            },
                            Multa = reader.IsDBNull(9) ? 0 : reader.GetDecimal(9),
                            FechaFin = reader.IsDBNull(7) ? "" : reader.GetString(7),
                        };
                        res.Add(a);

                    };
                    connection.Close();
                }
                return res;
            }

        }
        public Alquiler FinalizarContrato(Alquiler a)
        {
            DateTime hoy = DateTime.Now;
            a.ProximoFin = hoy.ToString();
            a.Multa = a.calcularMulta();
            a.Vigente = false;
            
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Alquiler SET FechaFin='{a.ProximoFin}', Multa='{a.calcularMulta()}' " +
                    $"WHERE IdContrato = {a.IdContrato}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return a;
        }

    }
}