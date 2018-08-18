using MySql.Data.MySqlClient;
using System;
using System.Data;
using C = Rocket.Core.Logging.Logger;

namespace Arechi.GroupBank
{
    public class Bank
    {
        private string _table;
        private MySqlConnection _connection;

        internal Bank()
        {
            new I18N.West.CP1250();
            _connection = CreateConnection();

            if (_connection == null)
            {
                Main.Instance.UnloadPlugin();
                return;
            }

            _table = Main.Instance.Configuration.Instance.DatabaseTableName;
            CheckBank();
        }

        private MySqlConnection CreateConnection()
        {
            try
            {
                MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder
                {
                    Server = Main.Instance.Configuration.Instance.DatabaseAddress,
                    Port = Main.Instance.Configuration.Instance.DatabasePort,
                    Database = Main.Instance.Configuration.Instance.DatabaseName,
                    UserID = Main.Instance.Configuration.Instance.DatabaseUsername,
                    Password = Main.Instance.Configuration.Instance.DatabasePassword,
                };

                return new MySqlConnection(connectionString.ToString()); ;
            }
            catch (Exception ex)
            {
                C.LogError(ex.Message);
                return null;
            }  
        }

        private void CheckConnection()
        {
            if (_connection == null)
                _connection = CreateConnection();

            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }

        public bool HasBank(string id)
        {
            try
            {
                CheckConnection();
                MySqlCommand command = _connection.CreateCommand();
                command.CommandText = $"SELECT `GroupID` FROM `{_table}` WHERE `GroupID` = @group";
                command.Prepare();
                command.Parameters.AddWithValue("@group", id);
                return command.ExecuteScalar() != null;
            }
            catch (Exception ex)
            {
                C.LogError(ex.Message);
                return false;
            }
        }

        public void SetBank(string id)
        {
            try
            {
                CheckConnection();
                MySqlCommand command = _connection.CreateCommand();
                command.CommandText = $"INSERT INTO `{_table}` (`GroupID`) VALUES (@group)";
                command.Prepare();
                command.Parameters.AddWithValue("@group", id);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                C.LogError(ex.Message);
            }
        }

        public int Update(string id, string column, int amount)
        {
            try
            {
                CheckConnection();
                MySqlCommand command = _connection.CreateCommand();
                command.CommandText = $"UPDATE `{_table}` SET `{column}` = `{column}` + @amount WHERE `GroupID` = @group;";
                command.CommandText += $"SELECT `{column}` FROM `{_table}` WHERE `GroupID` = @group";
                command.Prepare();
                command.Parameters.AddWithValue("@group", id);
                command.Parameters.AddWithValue("@amount", amount);
                object result = command.ExecuteScalar();

                return result != null ? int.Parse(result.ToString()) : 0;
            }
            catch (Exception ex)
            {
                C.LogError(ex.Message);
                return 0;
            }
        }

        public int Get(string id, string column)
        {
            try
            {
                CheckConnection();
                MySqlCommand command = _connection.CreateCommand();
                command.CommandText = $"SELECT `{column}` FROM `{_table}` WHERE `GroupID` = @group";
                command.Prepare();
                command.Parameters.AddWithValue("@group", id);
                object result = command.ExecuteScalar();

                return result != null ? int.Parse(result.ToString()) : 0;
            }
            catch (Exception ex)
            {
                C.LogError(ex.Message);
                return 0;
            }
        }

        public int DeleteRows()
        {
            try
            {
                CheckConnection();
                MySqlCommand command = _connection.CreateCommand();
                command.CommandText = $"DELETE FROM `{_table}` WHERE `LastAccessed` < DATE_SUB(NOW(), INTERVAL @interval DAY)";
                command.Prepare();
                command.Parameters.AddWithValue("@interval", Main.Instance.Configuration.Instance.InactiveDaysUntilDeletion);
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                C.LogError(ex.Message);
                return 0;
            }
        }

        internal void CheckBank()
        {
            try
            {
                CheckConnection();
                MySqlCommand command = _connection.CreateCommand();
                command.CommandText = 
                    $@"CREATE TABLE IF NOT EXISTS `{_table}` (
                       `GroupID` VARCHAR(32) NOT NULL,
                       `Money` INT(32) NOT NULL DEFAULT '0',
                       `Experience` INT(32) NOT NULL DEFAULT '0',
                       `LastAccessed` TIMESTAMP NULL ON UPDATE CURRENT_TIMESTAMP,
                       PRIMARY KEY (`GroupID`));";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                C.LogError(ex.Message);
            }
        }
    }
}
