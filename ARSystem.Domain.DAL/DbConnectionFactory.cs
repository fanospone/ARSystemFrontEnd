using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBGFramework.DataAccess;

namespace ARSystem.Domain.DAL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly DbProviderFactory _provider;
        private readonly string _connectionString;
        private readonly string _name;

        /// <summary>
        /// DbConnectionFactory Contructor
        /// </summary>
        /// <param name="connectionName">Connection Name</param>
        public DbConnectionFactory(string connectionName)
        {
            if (connectionName == null) throw new ArgumentNullException("connectionName");

           
            var conStr = MSSQL.GetConnectionString(connectionName);

            //string test = MSSQL.GetConnectionString(ConfigurationManager.ConnectionStrings[connectionName].ToString());
            if (conStr == null)
                throw new ConfigurationErrorsException(string.Format("Failed to find connection string named '{0}' in app/web.config.", connectionName));

            //_name = conStr.ProviderName;
            //_provider = DbProviderFactories.GetFactory(conStr.ProviderName);
            //_connectionString = conStr.ConnectionString;

            _name = "System.Data.SqlClient";
            _provider = DbProviderFactories.GetFactory(_name);
            _connectionString = conStr;

        }

        public DbConnectionFactory(string connectionName, string backup)
        {
            if (connectionName == null) throw new ArgumentNullException("connectionName");

            var conStr = ConfigurationManager.ConnectionStrings[connectionName];
            if (conStr == null)
                throw new ConfigurationErrorsException(string.Format("Failed to find connection string named '{0}' in app/web.config.", connectionName));

            _name = conStr.ProviderName;
            _provider = DbProviderFactories.GetFactory(conStr.ProviderName);
            _connectionString = conStr.ConnectionString;
        }

        /// <summary>
        /// Create Connection
        /// </summary>
        /// <returns></returns>
        public IDbConnection Create()
        {
            var connection = _provider.CreateConnection();
            if (connection == null)
                throw new ConfigurationErrorsException(string.Format("Failed to create a connection using the connection string named '{0}' in app/web.config.", _name));

            connection.ConnectionString = _connectionString;
            connection.Open();
            return connection;
        }
    }
}
