using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MagazineManager
{
    public static class DatabaseManager
    {

        private static string connectionStringContent = "MagazineManager.Properties.Settings.magazineConnectionString"; //Content of connection string
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




    }
}
