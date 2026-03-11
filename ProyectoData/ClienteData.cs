// ProyectoData/ClienteData.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ProyectoModelo;
using Microsoft.Extensions.Configuration;
using ProyectoData;

namespace ProyectoData
{
    public class ClienteData
    {
        private readonly string _connectionString;

        public ClienteData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaSQL");
        }

        public IEnumerable<Cliente> GetAll()
        {
            var list = new List<Cliente>();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT IdCliente, Identificacion, Nombres, Apellidos, Direccion, Telefono, FechaRegistro FROM Cliente", conn))
            {
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new Cliente
                        {
                            IdCliente = rdr.GetInt32(0),
                            Identificacion = rdr.GetString(1),
                            Nombres = rdr.GetString(2),
                            Apellidos = rdr.GetString(3),
                            Direccion = rdr.IsDBNull(4) ? null : rdr.GetString(4),
                            Telefono = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                            FechaRegistro = rdr.GetDateTime(6)
                        });
                    }
                }
            }
            return list;
        }

        public Cliente? GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT IdCliente, Identificacion, Nombres, Apellidos, Direccion, Telefono, FechaRegistro FROM Cliente WHERE IdCliente = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        return new Cliente
                        {
                            IdCliente = rdr.GetInt32(0),
                            Identificacion = rdr.GetString(1),
                            Nombres = rdr.GetString(2),
                            Apellidos = rdr.GetString(3),
                            Direccion = rdr.IsDBNull(4) ? null : rdr.GetString(4),
                            Telefono = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                            FechaRegistro = rdr.GetDateTime(6)
                        };
                    }
                }
            }
            return null;
        }

        public int Create(Cliente cliente)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"INSERT INTO Cliente (Identificacion, Nombres, Apellidos, Direccion, Telefono)
                                             VALUES (@ident, @nombres, @apellidos, @direccion, @telefono);
                                             SELECT SCOPE_IDENTITY();", conn))
            {
                cmd.Parameters.AddWithValue("@ident", cliente.Identificacion);
                cmd.Parameters.AddWithValue("@nombres", cliente.Nombres);
                cmd.Parameters.AddWithValue("@apellidos", cliente.Apellidos);
                cmd.Parameters.AddWithValue("@direccion", (object?)cliente.Direccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@telefono", (object?)cliente.Telefono ?? DBNull.Value);

                conn.Open();
                var id = cmd.ExecuteScalar();
                return Convert.ToInt32(id);
            }
        }
    }
}