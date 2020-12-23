using Arechi.GroupBank.Data;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Logger = Rocket.Core.Logging.Logger;

namespace Arechi.GroupBank
{
    public class Plugin : RocketPlugin<Config>
    {
        public static Plugin Instance;
        public BankData Bank;

        protected override void Load()
        {
            Instance = this;
            Bank = new BankData(Configuration.Instance);
            
            Logger.Log($"{Bank.DeleteInactiveBanks()} inactive banks have been deleted!");
        }

        protected override void Unload()
        {
            Instance = null;
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "BANK", "｢ Group Bank ｣" },
            { "BANK_EXP", "# Experience: {0}" },
            { "BANK_MONEY", "# {1}: {0}" },
            { "NO_GROUP", "You do not have a group!" },
            { "NO_BANK", "Your group does not have a bank!" },
            { "ALREADY_HAVE_A_BANK", "Your group already has a bank!" },
            { "BANK_BUY_ERROR", "Bank creation failed. You need to pay {0} {1} for one!" },
            { "BANK_BOUGHT", "Succesfully bought a bank for your group! You paid {0} {1}." },
            { "BANK_BOUGHT_GROUP", "{0} has bought a group bank!" },
            { "DEPOSIT_ERROR", "You only have {0}!" },
            { "WITHDRAW_ERROR", "Your bank only has {0}!" },
        };
    }
}
