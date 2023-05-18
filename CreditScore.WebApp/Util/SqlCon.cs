using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CreditScore.WebApp.Util
{
    public static class SqlCon
    {
        public static string _connectionString =
            ConfigurationManager.ConnectionStrings["AlpsConnection"].ConnectionString;
        public static bool IsServerConnected()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
    }
}