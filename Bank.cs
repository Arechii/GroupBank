using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using System;

namespace Arechi.GroupBank
{
    public class Bank
    {
        private string Table;

        internal Bank()
        {
            new I18N.West.CP1250();
            Table = Plugin.Instance.Configuration.Instance.DatabaseTableName;
            MySqlConnection connection = createConnection();
            try
            {
                connection.Open();
                connection.Close();

                CheckBank();
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
            }
        }

        private MySqlConnection createConnection()
        {
            MySqlConnection connection = null;
            try
            {
                if (Plugin.Instance.Configuration.Instance.DatabasePort == 0) Plugin.Instance.Configuration.Instance.DatabasePort = 3306;
                connection = new MySqlConnection(String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", 
                    Plugin.Instance.Configuration.Instance.DatabaseAddress, 
                    Plugin.Instance.Configuration.Instance.DatabaseName, 
                    Plugin.Instance.Configuration.Instance.DatabaseUsername,
                    Plugin.Instance.Configuration.Instance.DatabasePassword,
                    Plugin.Instance.Configuration.Instance.DatabasePort));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return connection;
        }

        public bool HasBank(string id)
        {
            bool output = false;
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT `GroupID` FROM `" + Table + "` WHERE `GroupID` = '" + id + "';";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) output = true;
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return output;
        }

        public void SetBank(string id)
        {
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO `" + Table + "` (`GroupID`) VALUES (@groupid);";
                command.Parameters.AddWithValue("@groupid", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Dispose();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public int Update(string id, string kind, int amount)
        {
            int output = 0;
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE `" + Table + "` SET `"+ kind + "` = `" + kind + "` + (" + amount + ") WHERE `GroupID` = '" + id + "'; SELECT `" + kind + "` FROM `" + Table + "` WHERE `GroupID` = '" + id + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) Int32.TryParse(result.ToString(), out output);
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return output;
        }

        public int Get(string id, string kind)
        {
            int output = 0;
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT `" + kind + "` FROM `" + Table + "` WHERE `GroupID` = '" + id + "';";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) Int32.TryParse(result.ToString(), out output);
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return output;
        }

        public int DeleteRows()
        {
            int affected = 0;
            try
            {
                MySqlConnection mySqlConnection = createConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = "DELETE FROM `" + Table + "` WHERE `LastAccessed` < date_sub(now(), interval '" + Plugin.Instance.Configuration.Instance.InactiveDaysUntilDeletion + "' day);";
                mySqlConnection.Open();
                affected = mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
            return affected;
        }

        internal void CheckBank()
        {
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "show tables like '" + Table + "'";
                connection.Open();
                object test = command.ExecuteScalar();

                if (test == null)
                {
                    command.CommandText = "CREATE TABLE `" + Table + "` ("
                        + " `GroupID` VARCHAR(32) NOT NULL,"
                        + " `Money` INT(32) NOT NULL DEFAULT '0',"
                        + " `Experience` INT(32) NOT NULL DEFAULT '0',"
                        + " `LastAccessed` TIMESTAMP NULL ON UPDATE CURRENT_TIMESTAMP,"
                        + " PRIMARY KEY (`GroupID`)); ";

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
