using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProyectoModelo;

namespace ProyectoData
{
    public class FacturaData
    {
        private readonly string _conn;
        public FacturaData(IConfiguration config)
        {
            _conn = config.GetConnectionString("CadenaSQL");
        }

        // ProyectoData/FacturaData.cs (añadir estos métodos a la clase FacturaData existente)
        public void UpdateFactura(Factura factura)
        {
            using var conn = new SqlConnection(_conn);
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                // 1) Actualizar cabecera
                var cmdUpd = new SqlCommand(@"
            UPDATE Factura
            SET IdCliente = @IdCliente,
                IdMesero = @IdMesero,
                IdMesa = @IdMesa,
                Fecha = @Fecha
            WHERE NroFactura = @NroFactura", conn, tran);
                cmdUpd.Parameters.AddWithValue("@IdCliente", factura.IdCliente);
                cmdUpd.Parameters.AddWithValue("@IdMesero", factura.IdMesero);
                cmdUpd.Parameters.AddWithValue("@IdMesa", (object?)factura.IdMesa ?? DBNull.Value);
                cmdUpd.Parameters.AddWithValue("@Fecha", factura.Fecha);
                cmdUpd.Parameters.AddWithValue("@NroFactura", factura.NroFactura);
                var rows = cmdUpd.ExecuteNonQuery();
                if (rows == 0) throw new InvalidOperationException("Factura no encontrada.");

                // 2) Eliminar detalles existentes (si los vas a reemplazar)
                var cmdDelDet = new SqlCommand("DELETE FROM DetalleFactura WHERE NroFactura = @Nro", conn, tran);
                cmdDelDet.Parameters.AddWithValue("@Nro", factura.NroFactura);
                cmdDelDet.ExecuteNonQuery();

                // 3) Insertar nuevos detalles y calcular total
                decimal total = 0m;
                if (factura.Detalles != null)
                {
                    foreach (var d in factura.Detalles)
                    {
                        var cmdInsDet = new SqlCommand(@"
                    INSERT INTO DetalleFactura (NroFactura, Plato, Cantidad, ValorUnitario)
                    VALUES (@NroFactura,@Plato,@Cantidad,@ValorUnitario);", conn, tran);
                        cmdInsDet.Parameters.AddWithValue("@NroFactura", factura.NroFactura);
                        cmdInsDet.Parameters.AddWithValue("@Plato", d.Plato);
                        cmdInsDet.Parameters.AddWithValue("@Cantidad", d.Cantidad);
                        cmdInsDet.Parameters.AddWithValue("@ValorUnitario", d.ValorUnitario);
                        cmdInsDet.ExecuteNonQuery();
                        total += d.Cantidad * d.ValorUnitario;
                    }
                }

                // 4) Actualizar total
                var cmdUpdTotal = new SqlCommand("UPDATE Factura SET Total = @Total WHERE NroFactura = @Nro", conn, tran);
                cmdUpdTotal.Parameters.AddWithValue("@Total", total);
                cmdUpdTotal.Parameters.AddWithValue("@Nro", factura.NroFactura);
                cmdUpdTotal.ExecuteNonQuery();

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public bool DeleteFactura(int nroFactura)
        {
            using var conn = new SqlConnection(_conn);
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                // Si la FK DetalleFactura tiene ON DELETE CASCADE, basta con borrar la factura.
                var cmd = new SqlCommand("DELETE FROM Factura WHERE NroFactura = @Nro", conn, tran);
                cmd.Parameters.AddWithValue("@Nro", nroFactura);
                var affected = cmd.ExecuteNonQuery();
                tran.Commit();
                return affected > 0;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
        public int CreateFactura(Factura factura)
        {
            using var conn = new SqlConnection(_conn);
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var cmd = new SqlCommand(@"
                    INSERT INTO Factura (IdCliente, IdMesero, IdMesa, Fecha, Total)
                    VALUES (@IdCliente,@IdMesero,@IdMesa,@Fecha,@Total);
                    SELECT SCOPE_IDENTITY();", conn, tran);
                cmd.Parameters.AddWithValue("@IdCliente", factura.IdCliente);
                cmd.Parameters.AddWithValue("@IdMesero", factura.IdMesero);
                cmd.Parameters.AddWithValue("@IdMesa", (object?)factura.IdMesa ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Fecha", factura.Fecha == default ? DateTime.UtcNow : factura.Fecha);
                cmd.Parameters.AddWithValue("@Total", 0m);
                var id = Convert.ToInt32(cmd.ExecuteScalar());

                decimal total = 0m;
                if (factura.Detalles != null)
                {
                    foreach (var d in factura.Detalles)
                    {
                        var cmdDet = new SqlCommand(@"
                            INSERT INTO DetalleFactura (NroFactura, Plato, Cantidad, ValorUnitario)
                            VALUES (@NroFactura,@Plato,@Cantidad,@ValorUnitario);", conn, tran);
                        cmdDet.Parameters.AddWithValue("@NroFactura", id);
                        cmdDet.Parameters.AddWithValue("@Plato", d.Plato);
                        cmdDet.Parameters.AddWithValue("@Cantidad", d.Cantidad);
                        cmdDet.Parameters.AddWithValue("@ValorUnitario", d.ValorUnitario);
                        cmdDet.ExecuteNonQuery();
                        total += d.Cantidad * d.ValorUnitario;
                    }
                }

                var upd = new SqlCommand("UPDATE Factura SET Total = @Total WHERE NroFactura = @Nro", conn, tran);
                upd.Parameters.AddWithValue("@Total", total);
                upd.Parameters.AddWithValue("@Nro", id);
                upd.ExecuteNonQuery();

                tran.Commit();
                return id;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public IEnumerable<Factura> GetFacturas(DateTime desde, DateTime hasta)
        {
            var list = new List<Factura>();
            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"
                SELECT NroFactura, IdCliente, IdMesero, IdMesa, Fecha, Total
                FROM Factura
                WHERE Fecha BETWEEN @desde AND @hasta
                ORDER BY Fecha DESC", conn);
            cmd.Parameters.AddWithValue("@desde", desde);
            cmd.Parameters.AddWithValue("@hasta", hasta);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new Factura
                {
                    NroFactura = rdr.GetInt32(0),
                    IdCliente = rdr.GetInt32(1),
                    IdMesero = rdr.GetInt32(2),
                    IdMesa = rdr.IsDBNull(3) ? null : (int?)rdr.GetInt32(3),
                    Fecha = rdr.GetDateTime(4),
                    Total = rdr.IsDBNull(5) ? 0m : rdr.GetDecimal(5)
                });
            }
            return list;
        }
    }
}