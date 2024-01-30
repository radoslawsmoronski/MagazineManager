using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Diagnostics;

namespace MagazineManager
{
    public static class DatabaseManager
    {

        private static string connectionStringContent = "MagazineManager.Properties.Settings.UdemyConnectionString"; //Content of connection string
        private static string connectionString; //Connection string is create in CreateConnectionString()
        private static SqlConnection sqlConnection; //SqlConnection is create in CreateConnectionString()

        public static void CreateConnectionString()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings[connectionStringContent].ConnectionString;
                sqlConnection = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Data base connection string error: {ex.Message}");
                Application.Current.Shutdown();
            }
        }

        public static void ConnectionTest()
        {
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Data base connection error: {ex.Message}");
                Application.Current.Shutdown();
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }

        public static string GetSingleResultFromDB(string query, (string, dynamic)[] valuesToQuery)
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        foreach (var value in valuesToQuery)
                        {
                            command.Parameters.AddWithValue(value.Item1, value.Item2);
                        }

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            return result.ToString();
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database connection error: {ex.Message}");
                    Application.Current.Shutdown();
                    return null;
                }
            }
        }

        public static bool InsertValueInDB(string query, (string, dynamic)[] valuesToQuery)
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        foreach (var value in valuesToQuery)
                        {
                            command.Parameters.AddWithValue(value.Item1, value.Item2);
                        }

                        command.ExecuteNonQuery();

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database connection error: {ex.Message}");
                    Application.Current.Shutdown();
                    return false;
                }
            }
        }


    }
}
