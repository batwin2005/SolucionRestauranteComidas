// ProyectoData/PlatoData.cs
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoModelo;

namespace ProyectoData
{
    public class PlatoData
    {
        private readonly string _connectionString;

        public PlatoData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaSQL");
        }

        // Crea un nuevo plato y devuelve el Id generado
        public int Create(Plato plato)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
                INSERT INTO Plato (Nombre, Precio)
                VALUES (@Nombre, @Precio);
                SELECT SCOPE_IDENTITY();", conn);

            cmd.Parameters.AddWithValue("@Nombre", plato.Nombre ?? string.Empty);
            cmd.Parameters.AddWithValue("@Precio", plato.Precio);

            conn.Open();
            var id = cmd.ExecuteScalar();
            return Convert.ToInt32(id);
        }

        // Obtiene todos los platos
        public IEnumerable<Plato> GetAll()
        {
            var list = new List<Plato>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT IdPlato, Nombre, Precio FROM Plato ORDER BY Nombre", conn);

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new Plato
                {
                    IdPlato = rdr.GetInt32(0),
                    Nombre = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                    Precio = rdr.IsDBNull(2) ? 0m : rdr.GetDecimal(2)
                });
            }
            return list;
        }

        // Obtiene un plato por id
        public Plato? GetById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT IdPlato, Nombre, Precio FROM Plato WHERE IdPlato = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                return new Plato
                {
                    IdPlato = rdr.GetInt32(0),
                    Nombre = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                    Precio = rdr.IsDBNull(2) ? 0m : rdr.GetDecimal(2)
                };
            }
            return null;
        }

        // Actualiza un plato existente; devuelve true si se actualizó
        public bool Update(Plato plato)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
                UPDATE Plato
                SET Nombre = @Nombre,
                    Precio = @Precio
                WHERE IdPlato = @IdPlato", conn);

            cmd.Parameters.AddWithValue("@Nombre", plato.Nombre ?? string.Empty);
            cmd.Parameters.AddWithValue("@Precio", plato.Precio);
            cmd.Parameters.AddWithValue("@IdPlato", plato.IdPlato);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // Elimina un plato por id; devuelve true si se eliminó
        public bool Delete(int idPlato)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("DELETE FROM Plato WHERE IdPlato = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", idPlato);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}