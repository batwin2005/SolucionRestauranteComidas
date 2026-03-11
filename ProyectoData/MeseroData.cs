// ProyectoData/MeseroData.cs
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoModelo;

namespace ProyectoData
{
    public class MeseroData
    {
        private readonly string _connectionString;

        public MeseroData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaSQL");
        }

        // Crea un nuevo mesero y devuelve el Id generado
        public int Create(Mesero mesero)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
                INSERT INTO Mesero (Nombres, Apellidos, Edad, Antiguedad, FechaIngreso)
                VALUES (@Nombres, @Apellidos, @Edad, @Antiguedad, @FechaIngreso);
                SELECT SCOPE_IDENTITY();", conn);

            cmd.Parameters.AddWithValue("@Nombres", mesero.Nombres ?? string.Empty);
            cmd.Parameters.AddWithValue("@Apellidos", mesero.Apellidos ?? string.Empty);
            cmd.Parameters.AddWithValue("@Edad", (object?)mesero.Edad ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Antiguedad", (object?)mesero.Antiguedad ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaIngreso", (object?)mesero.FechaIngreso ?? DBNull.Value);

            conn.Open();
            var id = cmd.ExecuteScalar();
            return Convert.ToInt32(id);
        }

        // Obtiene todos los meseros
        public IEnumerable<Mesero> GetAll()
        {
            var list = new List<Mesero>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT IdMesero, Nombres, Apellidos, Edad, Antiguedad, FechaIngreso FROM Mesero ORDER BY Nombres, Apellidos", conn);

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new Mesero
                {
                    IdMesero = rdr.GetInt32(0),
                    Nombres = rdr.IsDBNull(1) ? null! : rdr.GetString(1),
                    Apellidos = rdr.IsDBNull(2) ? null! : rdr.GetString(2),
                    Edad = rdr.IsDBNull(3) ? null : (int?)rdr.GetInt32(3),
                    Antiguedad = rdr.IsDBNull(4) ? null : (int?)rdr.GetInt32(4),
                    FechaIngreso = rdr.IsDBNull(5) ? null : (DateTime?)rdr.GetDateTime(5)
                });
            }
            return list;
        }

        // Obtiene un mesero por id
        public Mesero? GetById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT IdMesero, Nombres, Apellidos, Edad, Antiguedad, FechaIngreso FROM Mesero WHERE IdMesero = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                return new Mesero
                {
                    IdMesero = rdr.GetInt32(0),
                    Nombres = rdr.IsDBNull(1) ? null! : rdr.GetString(1),
                    Apellidos = rdr.IsDBNull(2) ? null! : rdr.GetString(2),
                    Edad = rdr.IsDBNull(3) ? null : (int?)rdr.GetInt32(3),
                    Antiguedad = rdr.IsDBNull(4) ? null : (int?)rdr.GetInt32(4),
                    FechaIngreso = rdr.IsDBNull(5) ? null : (DateTime?)rdr.GetDateTime(5)
                };
            }
            return null;
        }

        // Actualiza un mesero existente; devuelve true si se actualizó
        public bool Update(Mesero mesero)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
                UPDATE Mesero
                SET Nombres = @Nombres,
                    Apellidos = @Apellidos,
                    Edad = @Edad,
                    Antiguedad = @Antiguedad,
                    FechaIngreso = @FechaIngreso
                WHERE IdMesero = @IdMesero", conn);

            cmd.Parameters.AddWithValue("@Nombres", mesero.Nombres ?? string.Empty);
            cmd.Parameters.AddWithValue("@Apellidos", mesero.Apellidos ?? string.Empty);
            cmd.Parameters.AddWithValue("@Edad", (object?)mesero.Edad ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Antiguedad", (object?)mesero.Antiguedad ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaIngreso", (object?)mesero.FechaIngreso ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdMesero", mesero.IdMesero);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // Elimina un mesero por id; devuelve true si se eliminó
        public bool Delete(int idMesero)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("DELETE FROM Mesero WHERE IdMesero = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", idMesero);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}