using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Helper;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp.Data
{



    /// <summary>
    /// The DB class provides functionality for managing database connections 
    /// using ADO.NET. It includes methods to open and close the database connection.
    /// </summary>
    public class DbConnection
    {
        public SqlConnection Connection;
        private static string connectionString;
        public const string APP_CONFIG_CONNECTION_NAME = "ConnectionString";
        public const string APP_CONFIG_CONNECTION_VALUE_NAME = "connectionStrings";
        public const string APP_CONFIG_SETTINGS_NAME = "appSettings";
        public const string APP_CONFIG_CONNECTION_PROVIDER_NAME = "System.Data.SqlClient";

        public static event EventHandler ConnectionFailed;


        public DbConnection()
        {


            try
            {
                connectionString = GetConfigConnectionString();
                Connection = new SqlConnection(connectionString);
                OpenConnection();
            }

            catch (Exception ex)
            {
                Log.Error(ex);

            }
            finally
            {
                CloseConnection();
            }
        }
        public void OnConnectionFailed()
        {
            try
            {
                ConnectionFailed?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        public static int IsConnected()
        {
            try
            {
                string tConnectionString = GetConfigConnectionString();
                using (SqlConnection tConnection = new SqlConnection(tConnectionString))
                {
                    tConnection.Open();
                    return ResultCode.SUCCESS;
                }
            }
            catch (SqlException ex)
            {
                return DbException.HandleSqlException(ex);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
        }

        // Open the database connection
        public int OpenConnection()
        {
            try
            {
                using (Connection)
                {
                    if (Connection != null && Connection.State != ConnectionState.Closed)
                    {
                        Connection.Close();
                    }
                }

                Connection = new SqlConnection(connectionString);
                Connection.Open();

                return ResultCode.SUCCESS;
            }
            catch (SqlException ex)
            {
                OnConnectionFailed();
                return DbException.HandleSqlException(ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                OnConnectionFailed();

                return ResultCode.ERROR;
            }
        }
        protected static string GetConfigConnectionString()
        {
            try
            {
                ConfigurationManager.RefreshSection(APP_CONFIG_SETTINGS_NAME);
                ConfigurationManager.RefreshSection(APP_CONFIG_CONNECTION_VALUE_NAME);
                return ConfigurationManager.ConnectionStrings[APP_CONFIG_CONNECTION_NAME].ConnectionString;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return "";
        }
        // Close the database connection
        public void CloseConnection()
        {
            try
            {
                if (Connection != null)
                {
                    Connection.Close();
                    Connection = null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);

            }

        }

        public void RefreshConnectionString()
        {
            try
            {
                connectionString = GetConfigConnectionString();
                Log.Info("Connection String Updated <*-_-*><-_->");
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }


    }

}
