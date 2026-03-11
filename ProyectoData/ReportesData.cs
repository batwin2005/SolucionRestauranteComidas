using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ProyectoData
{
    public class ReportesData
    {
        private readonly string _conn;
        public ReportesData(IConfiguration config) => _conn = config.GetConnectionString("CadenaSQL");

        public IEnumerable<object> VentasPorMesero(DateTime desde, DateTime hasta)
        {
            var list = new List<object>();
            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"
                SELECT m.IdMesero, m.Nombres, m.Apellidos, ISNULL(SUM(f.Total),0) AS TotalVendido
                FROM Mesero m
                LEFT JOIN Factura f ON f.IdMesero = m.IdMesero AND f.Fecha BETWEEN @desde AND @hasta
                GROUP BY m.IdMesero, m.Nombres, m.Apellidos
                ORDER BY TotalVendido DESC", conn);
            cmd.Parameters.AddWithValue("@desde", desde);
            cmd.Parameters.AddWithValue("@hasta", hasta);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new
                {
                    IdMesero = rdr.GetInt32(0),
                    Nombres = rdr.GetString(1),
                    Apellidos = rdr.GetString(2),
                    TotalVendido = rdr.GetDecimal(3)
                });
            }
            return list;
        }

        public IEnumerable<object> ClientesPorConsumo(decimal minimo, DateTime desde, DateTime hasta)
        {
            var list = new List<object>();
            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"
                SELECT c.IdCliente, c.Nombres, c.Apellidos, ISNULL(SUM(f.Total),0) AS TotalConsumo
                FROM Cliente c
                LEFT JOIN Factura f ON f.IdCliente = c.IdCliente AND f.Fecha BETWEEN @desde AND @hasta
                GROUP BY c.IdCliente, c.Nombres, c.Apellidos
                HAVING ISNULL(SUM(f.Total),0) >= @minimo
                ORDER BY TotalConsumo DESC", conn);
            cmd.Parameters.AddWithValue("@minimo", minimo);
            cmd.Parameters.AddWithValue("@desde", desde);
            cmd.Parameters.AddWithValue("@hasta", hasta);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new
                {
                    IdCliente = rdr.GetInt32(0),
                    Nombres = rdr.GetString(1),
                    Apellidos = rdr.GetString(2),
                    TotalConsumo = rdr.GetDecimal(3)
                });
            }
            return list;
        }

        public object ProductoMasVendido(int anio, int mes)
        {
            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(@"
                SELECT TOP 1 df.Plato, SUM(df.Cantidad) AS CantidadVendida, SUM(df.Cantidad * df.ValorUnitario) AS MontoTotal
                FROM DetalleFactura df
                JOIN Factura f ON f.NroFactura = df.NroFactura
                WHERE YEAR(f.Fecha) = @anio AND MONTH(f.Fecha) = @mes
                GROUP BY df.Plato
                ORDER BY CantidadVendida DESC, MontoTotal DESC", conn);
            cmd.Parameters.AddWithValue("@anio", anio);
            cmd.Parameters.AddWithValue("@mes", mes);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                return new
                {
                    Plato = rdr.GetString(0),
                    CantidadVendida = rdr.GetInt32(1),
                    MontoTotal = rdr.GetDecimal(2)
                };
            }
            return null;
        }
    }
}