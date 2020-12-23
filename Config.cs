using Rocket.API;

namespace Arechi.GroupBank
{
    public class Config : IRocketPluginConfiguration
    {
        public string Color;
        public string IconURL;
        public int BankPrice;
        public int InactiveDaysUntilDeletion;
        public string DatabaseAddress;
        public string DatabaseUsername;
        public string DatabasePassword;
        public string DatabaseName;
        public string DatabaseTableName;
        public uint DatabasePort;

        public void LoadDefaults()
        {
            Color = "#78b159";
            IconURL = null;
            BankPrice = 100;
            InactiveDaysUntilDeletion = 7;
            DatabaseAddress = "localhost";
            DatabaseUsername = "root";
            DatabasePassword = "password";
            DatabaseName = "unturned";
            DatabaseTableName = "groupbank";
            DatabasePort = 3306;
        }
    }
}