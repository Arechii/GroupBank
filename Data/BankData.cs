using MySql.Data.MySqlClient;

namespace Arechi.GroupBank.Data
{
    public class BankData
    {
        public readonly string _table;
        private Config _config;
        private DatabaseConnection _dbConnection;

        public BankData(Config config)
        {
            _table = config.DatabaseTableName;
            _config = config;
            _dbConnection = new DatabaseConnection(config.DatabaseAddress, config.DatabasePort, config.DatabaseUsername, 
                config.DatabasePassword, config.DatabaseName);
        }

        public Bank GetBank(string groupId)
        {
            var result = _dbConnection.ExecuteReader($"SELECT * FROM `{_table}` WHERE `GroupId` = @groupId;", new MySqlParameter("@groupId", groupId));

            if (result.Rows.Count == 0) return null;

            var row = result.Rows[0];

            return new Bank(row["GroupId"].ToString(), (decimal)row["Money"], (uint)row["Experience"]);
        }

        public void AddBank(string groupId)
        {
            _dbConnection.ExecuteNonQuery($"INSERT INTO `{_table}` (`GroupId`) VALUES (@groupId);", new MySqlParameter("@groupId", groupId));
        }

        public void UpdateBank(Bank bank)
        {
            _dbConnection.ExecuteNonQuery($"UPDATE `{_table}` SET `Money` = @money, `Experience` = @experience WHERE `GroupId` = @groupId;", new[]
            {
                new MySqlParameter("@groupId", bank.GroupId),
                new MySqlParameter("@money", bank.Money),
                new MySqlParameter("@experience", bank.Experience)
            });
        }

        public int DeleteInactiveBanks()
        {
            return _dbConnection.ExecuteNonQuery($"DELETE FROM `{_table}` WHERE `LastAccessed` < DATE_SUB(NOW(), INTERVAL @interval DAY);",
                new MySqlParameter("@interval", _config.InactiveDaysUntilDeletion));
        }

        public void CreateTable()
        {
            _dbConnection.ExecuteNonQuery($@"
                CREATE TABLE IF NOT EXISTS `{_table}` (
                    `GroupId` VARCHAR(32) NOT NULL,
                    `Money` DECIMAL(15,2) NOT NULL DEFAULT 0,
                    `Experience` INT(32) NOT NULL DEFAULT 0,
                    `LastAccessed` TIMESTAMP NULL ON UPDATE CURRENT_TIMESTAMP,
                    PRIMARY KEY (`GroupId`)
                );"
             );

            //Migrate
            _dbConnection.ExecuteNonQuery($"ALTER TABLE `{_table}` CHANGE COLUMN `GroupID` `GroupId` VARCHAR(32);");
            _dbConnection.ExecuteNonQuery($"ALTER TABLE `{_table}` MODIFY `Money` DECIMAL(15,2) NOT NULL DEFAULT 0;");
        }
    }
}
