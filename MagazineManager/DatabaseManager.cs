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

        public static string GetSingleResultFromDB(string query,//here params object[] args)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        //here
                        foreach (object o in args)
                        {
                            command.Parameters.AddWithValue(o, o);
                        }

                        // Execute the query and retrieve a single result (the hashed password)
                        object result = command.ExecuteScalar();

                        // Check if a result was returned
                        if (result != null)
                        {
                            // Convert the result to a string and return the hashed password
                            return result.ToString();
                        }
                        else
                        {
                            // Display a message if the login does not exist and return null
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle database connection errors by displaying a message and shutting down the application
                    MessageBox.Show($"Database connection error: {ex.Message}");
                    Application.Current.Shutdown();
                    return null;
                }
            }
        }

        public static string GetPasswordByLogin(string login)
        {
            // Using statement ensures proper resource disposal (closing the SqlConnection)
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the database connection
                    sqlConnection.Open();

                    // SQL query to retrieve the hashed password based on the provided login
                    string query = "SELECT HashedPassword FROM Users WHERE Login = @Login;";

                    // Using SqlCommand to execute the SQL query
                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        // Add a parameter to the SQL query to prevent SQL injection
                        command.Parameters.AddWithValue("@Login", login);

                        // Execute the query and retrieve a single result (the hashed password)
                        object result = command.ExecuteScalar();

                        // Check if a result was returned
                        if (result != null)
                        {
                            // Convert the result to a string and return the hashed password
                            return result.ToString();
                        }
                        else
                        {
                            // Display a message if the login does not exist and return null
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle database connection errors by displaying a message and shutting down the application
                    MessageBox.Show($"Database connection error: {ex.Message}");
                    Application.Current.Shutdown();
                    return null;
                }
            }
        }


    }
}
