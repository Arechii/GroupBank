using fr34kyn01535.Uconomy;
using Rocket.API;
using Rocket.Unturned.Player;
using Steamworks;
using System.Collections.Generic;

namespace Arechi.GroupBank
{
    public class CGBuy : IRocketCommand
    {
        public string Name { get { return "gbuy"; } }
        public string Help { get { return "Buys a bank for your group"; } }
        public string Syntax { get { return ""; } }
        public List<string> Aliases { get { return new List<string>(); } }
        public List<string> Permissions { get { return new List<string>() { "gbuy" }; } }
        public AllowedCaller AllowedCaller { get { return AllowedCaller.Player; } }
        

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length == 0)
            {
                if (player.SteamGroupID == CSteamID.Nil)
                {
                    Plugin.Instance.Say(player, "no_group"); return;
                }

                if (Plugin.Instance.Bank.HasBank(player.SteamGroupID.ToString()))
                {
                    Plugin.Instance.Say(player, "have_bank"); return;
                }

                if (Uconomy.Instance.Database.GetBalance(player.ToString()) < Plugin.Instance.Configuration.Instance.BankPrice)
                {
                    Plugin.Instance.Say(player, "bank_error_2", Plugin.Instance.Configuration.Instance.BankPrice, Plugin.Instance.Configuration.Instance.MoneyName); return;
                }

                Uconomy.Instance.Database.IncreaseBalance(player.ToString(), -Plugin.Instance.Configuration.Instance.BankPrice);
                Plugin.Instance.Bank.SetBank(player.SteamGroupID.ToString());
                Plugin.Instance.Say(player, "bank_bought", Plugin.Instance.Configuration.Instance.BankPrice, Plugin.Instance.Configuration.Instance.MoneyName);
                Plugin.Instance.Notify(player.CSteamID, player.SteamGroupID, player.DisplayName + " has bought a bank!");
            }
            else
            {
                Plugin.Instance.Say(player, "gbuy_usage");
            }
        }
    }
}
