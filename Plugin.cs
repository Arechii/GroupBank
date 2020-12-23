using Arechi.GroupBank.Utils;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using Steamworks;
using Logger = Rocket.Core.Logging.Logger;

namespace Arechi.GroupBank
{
    public class Plugin : RocketPlugin<Config>
    {
        public static Plugin Instance;
        public Bank Bank;

        protected override void Load()
        {
            Instance = this;
            Bank = new Bank();
            
            Logger.Log($"{Bank.DeleteRows()} inactive banks have been deleted!");
        }

        protected override void Unload()
        {
            Instance = null;
        }

        public bool CheckPlayer(UnturnedPlayer player)
        {
            if (player.SteamGroupID == CSteamID.Nil)
            {
                player.SendMessage("no_group");
                return false;
            }

            if (!Bank.HasBank(player.SteamGroupID.ToString()))
            {
                player.SendMessage("no_bank");
                return false;
            }

            return true;
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "bank", "｢ Group Bank ｣" },
            { "bank_xp", "# Experience: {0}" },
            { "bank_money", "# {1}: {0}" },
            { "gxp_usage", "Error in command. Try /gxp +|- amount" },
            { "gmoney_usage", "Error in command. Try /gmoney +|- amount" },
            { "no_group", "You do not have a group!" },
            { "no_bank", "Your group does not have a bank!" },
            { "have_bank", "Your group already has a bank!" },
            { "bank_error", "Bank creation failed. You already have one for your group!" },
            { "bank_error_2", "Bank creation failed. You need to pay {0} {1} for one!" },
            { "bank_bought", "Succesfully bought a bank for your group! You paid {0} {1}." },
            { "dep_error", "Something went wrong. Input a number." },
            { "dep_error_2", "You only have {0} Experience!" },
            { "dep_error_3", "You only have {0} {1}!" },
            { "wit_error", "Something went wrong. Input a number." },
            { "wit_error_2", "Your bank does not have this!" },
        };
    }
}
