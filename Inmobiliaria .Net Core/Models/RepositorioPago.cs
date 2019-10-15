using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Inmobiliaria_.Net_Core.Models
{
    public class RepositorioPago : RepositorioBase, IRepositorioPago
    {
        public RepositorioPago(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Pago p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Pagos (IdAlquiler,NroPago,Fecha,Importe) " +
                    $"VALUES ('{p.IdAlquiler}','{p.NroPago}','{p.Fecha}','{p.Importe}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    p.Id = Convert.ToInt32(id);
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
                string sql = $"DELETE FROM Pagos WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }


        public int Modificacion(Pago p)
        {
            throw new NotImplementedException();
        }

        public IList<Pago> ObtenerPorAlquiler(int alquiler)
        {
            IList<Pago> res = new List<Pago>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT p.Id, IdAlquiler, Importe , Fecha , c.Inquilino , c.Inmueble , NroPago " +
                    $" FROM Pagos p JOIN Alquiler c ON (p.IdAlquiler=c.IdContrato) WHERE c.IdContrato=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = alquiler;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago p = new Pago
                        {
                            Id = reader.GetInt32(0),
                            NroPago = reader.GetInt32(6),
                            Alquiler = new Alquiler
                            {
                                IdContrato = reader.GetInt32(1),
                                IdInquilino = reader.GetInt32(4),
                                IdInmueble = reader.GetInt32(5)
                            },
                            Importe = reader.GetDecimal(2),
                            Fecha = reader.GetString(3),
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Pago ObtenerPorId(int id)
        {
            Pago res = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT Id, IdAlquiler, Importe , Fecha , NroPago " +
                    $" FROM Pagos p WHERE Id=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Pago p = new Pago
                        {
                            Id = reader.GetInt32(0),
                            NroPago = reader.GetInt32(4),
                            Alquiler = new Alquiler
                            {
                                IdContrato = reader.GetInt32(1),
                            },
                            Importe = reader.GetDecimal(2),
                            Fecha = reader.GetString(3),
                        };
                        res = p;
                    }
                    connection.Close();
                }
            }
            return res;

        }

        public int maxPago(int alquiler)
        {
            int max = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT MAX(NroPago) " +
                    $"FROM Pagos p JOIN Alquiler c ON (p.IdAlquiler=c.IdContrato) WHERE c.IdContrato=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = alquiler;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        max = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    }
                    connection.Close();
                }
            }
            return max;
        }

        public IList<Pago> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        public IList<Pago> Buscar(string clave)
        {
            throw new NotImplementedException();
        }
        public Pago Update(Pago p) { throw new NotImplementedException(); }
        public Pago FinalizarContrato(Pago i) { throw new NotImplementedException(); }
    }
}