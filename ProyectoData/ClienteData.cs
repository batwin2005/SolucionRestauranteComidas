// ProyectoData/ClienteData.cs
using Microsoft.Data.SqlClient; // usa Microsoft.Data.SqlClient
using Microsoft.Extensions.Configuration;
using ProyectoModelo;
using System;
using System.Collections.Generic;

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
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT IdCliente, Identificacion, Nombres, Apellidos, Direccion, Telefono, FechaRegistro FROM Cliente", conn);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
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
            return list;
        }

        public Cliente? GetById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT IdCliente, Identificacion, Nombres, Apellidos, Direccion, Telefono, FechaRegistro FROM Cliente WHERE IdCliente = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
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
            return null;
        }
                
        public int Create(Cliente cliente)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
                INSERT INTO Cliente (Identificacion, Nombres, Apellidos, Direccion, Telefono)
                VALUES (@ident, @nombres, @apellidos, @direccion, @telefono);
                SELECT SCOPE_IDENTITY();", conn);
            cmd.Parameters.AddWithValue("@ident", cliente.Identificacion);
            cmd.Parameters.AddWithValue("@nombres", cliente.Nombres);
            cmd.Parameters.AddWithValue("@apellidos", cliente.Apellidos);
            cmd.Parameters.AddWithValue("@direccion", (object?)cliente.Direccion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@telefono", (object?)cliente.Telefono ?? DBNull.Value);

            conn.Open();
            var id = cmd.ExecuteScalar();
            return Convert.ToInt32(id);
        }

        // Actualiza un cliente existente
        public bool Update(Cliente cliente)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
                UPDATE Cliente
                SET Identificacion = @Identificacion,
                    Nombres = @Nombres,
                    Apellidos = @Apellidos,
                    Direccion = @Direccion,
                    Telefono = @Telefono
                WHERE IdCliente = @IdCliente", conn);
            cmd.Parameters.AddWithValue("@Identificacion", cliente.Identificacion);
            cmd.Parameters.AddWithValue("@Nombres", cliente.Nombres);
            cmd.Parameters.AddWithValue("@Apellidos", cliente.Apellidos);
            cmd.Parameters.AddWithValue("@Direccion", (object?)cliente.Direccion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefono", (object?)cliente.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // Elimina un cliente por id
        public bool Delete(int idCliente)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("DELETE FROM Cliente WHERE IdCliente = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", idCliente);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}