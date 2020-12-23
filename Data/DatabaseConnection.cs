using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using System.Data;

namespace Arechi.GroupBank.Data
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(string address, uint port, string userId, string password, string database)
        {
            _connectionString = new MySqlConnectionStringBuilder()
            {
                Server = address,
                Port = port,
                UserID = userId,
                Password = password,
                Database = database
            }.ToString();
        }

        public int ExecuteNonQuery(string query, params MySqlParameter[] parameters)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = PrepareCommand(query, connection, parameters))
                    {
                        var result = command.ExecuteNonQuery();
                        return result;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
                return 0;
            }
        }

        public DataTable ExecuteReader(string query, params MySqlParameter[] parameters)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = PrepareCommand(query, connection, parameters))
                    {
                        var result = new DataTable();
                        result.Load(command.ExecuteReader());
                        return result;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public object ExecuteScalar(string query, params MySqlParameter[] parameters)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = PrepareCommand(query, connection, parameters))
                    {
                        return command.ExecuteScalar();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public MySqlCommand PrepareCommand(string query, MySqlConnection connection, params MySqlParameter[] parameters)
        {
            var command = new MySqlCommand(query, connection);

            command.Prepare();

            foreach (var param in parameters)
                command.Parameters.Add(param);

            return command;
        }
    }
}
