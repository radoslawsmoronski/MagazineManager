using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Diagnostics;
using System.Data;

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

        public static List<string[]> GetSqlQueryResults(string query, (string, dynamic)[] valuesToQuery)
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        //Setting parameters
                        foreach (var value in valuesToQuery)
                        {
                                command.Parameters.AddWithValue(value.Item1, value.Item2);
                        }

                        //Getting data
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            List<string[]> results = new List<string[]>();

                            while (reader.Read())
                            {
                                string[] row = new string[reader.FieldCount];

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[i] = reader.GetValue(i).ToString();
                                }

                                results.Add(row);
                            }

                            return results;
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

        public static bool ExecuteSqlStatement(string query, (string, dynamic)[] valuesToQuery)
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

                        if (command.ExecuteNonQuery() > 0) return true;

                        return false;
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

        public static int BoolToBit(bool value) => value ? 1 : 0;

        public static bool BitToBool(object value) =>
            value is string stringValue && stringValue == "1" ||
            value is int intValue && intValue == 1;
    }
}
