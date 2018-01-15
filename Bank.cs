using MySql.Data.MySqlClient;
using System;
using C = Rocket.Core.Logging.Logger;

namespace Arechi.GroupBank
{
    public class Bank
    {
        private string Table;

        internal Bank()
        {
            new I18N.West.CP1250();
            MySqlConnection connection = CreateConnection();
            if (connection == null)
            {
                Main.Instance.UnloadPlugin();
                return;
            }
            Table = Main.Instance.Configuration.Instance.DatabaseTableName;
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
                C.LogWarning(ex.Message);
                return null;
            }  
        }

        public bool HasBank(string id)
        {
            bool output = false;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = $"SELECT `GroupID` FROM `{Table}` WHERE `GroupID` = '{id}';";
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                    output = true;

                connection.Close();
            }
            catch (Exception ex)
            {
                C.LogWarning(ex.Message);
            }
            return output;
        }

        public void SetBank(string id)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO `{Table}` (`GroupID`) VALUES (@groupid);";
                command.Parameters.AddWithValue("@groupid", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Dispose();
            }
            catch (Exception ex)
            {
                C.LogWarning(ex.Message);
            }
        }

        public int Update(string id, string kind, int amount)
        {
            int output = 0;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = $"UPDATE `{Table}` SET `{kind}` = `{kind}` + ({amount}) WHERE `GroupID` = '{id}';";
                command.CommandText += $"SELECT `{kind}` FROM `{Table}` WHERE `GroupID` = '{id}'";
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                    Int32.TryParse(result.ToString(), out output);

                connection.Close();
            }
            catch (Exception ex)
            {
                C.LogWarning(ex.Message);
            }
            return output;
        }

        public int Get(string id, string kind)
        {
            int output = 0;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = $"SELECT `{kind}` FROM `{Table}` WHERE `GroupID` = '{id}';";
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                    Int32.TryParse(result.ToString(), out output);

                connection.Close();
            }
            catch (Exception ex)
            {
                C.LogWarning(ex.Message);
            }
            return output;
        }

        public int DeleteRows()
        {
            int interval = Main.Instance.Configuration.Instance.InactiveDaysUntilDeletion;
            int affected = 0;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM `{Table}` WHERE `LastAccessed` < DATE_SUB(NOW(), INTERVAL '{interval}' DAY);";
                connection.Open();
                affected = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                C.LogWarning(ex.Message);
            }
            return affected;
        }

        internal void CheckBank()
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = 
                    $@"CREATE TABLE IF NOT EXISTS `{Table}` (
                       `GroupID` VARCHAR(32) NOT NULL,
                       `Money` INT(32) NOT NULL DEFAULT '0',
                       `Experience` INT(32) NOT NULL DEFAULT '0',
                       `LastAccessed` TIMESTAMP NULL ON UPDATE CURRENT_TIMESTAMP,
                       PRIMARY KEY (`GroupID`));";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                C.LogWarning(ex.Message);
            }
        }
    }
}
