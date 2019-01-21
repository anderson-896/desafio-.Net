using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseHelper
{
    public interface IConnectionFactory
    {
        SqlConnection GetConnection(string connectionName);
    }
    public class ConnectionFactory : IConnectionFactory
    {
        public SqlConnection GetConnection(string connectionName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            return new SqlConnection(connectionString);
        }
    }
}
