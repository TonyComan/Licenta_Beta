using Npgsql;
using System;
using System.Windows;

namespace DentalProApp
{
    public static class DbConnectionHelper
    {
        private static readonly string connectionString = "Host=localhost;Port=5432;Database=dentalpro;Username=postgres;Password=tonysql";

        public static NpgsqlConnection GetConnection()
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la conectare: " + ex.Message);
                return null;
            }
        }
    }
}
