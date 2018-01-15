using fr34kyn01535.Uconomy;
using Rocket.API;
using Rocket.Unturned.Player;
using Steamworks;
using System.Collections.Generic;

namespace Arechi.GroupBank.Commands
{
    public class CGBuy : IRocketCommand
    {
        public string Name => "gbuy";

        public string Help => "Buys a bank for your group";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "gbuy" };

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (player.SteamGroupID == CSteamID.Nil)
            {
                Main.Instance.Say(player, "no_group");
                return;
            }

            if (Main.Instance.Bank.HasBank(player.SteamGroupID.ToString()))
            {
                Main.Instance.Say(player, "have_bank");
                return;
            }

            if (Uconomy.Instance.Database.GetBalance(player.ToString()) < Main.Instance.Configuration.Instance.BankPrice)
            {
                Main.Instance.Say(player, "bank_error_2", Main.Instance.Configuration.Instance.BankPrice, Main.Instance.Configuration.Instance.MoneyName);
                return;
            }

            Uconomy.Instance.Database.IncreaseBalance(player.ToString(), -Main.Instance.Configuration.Instance.BankPrice);
            Main.Instance.Bank.SetBank(player.SteamGroupID.ToString());
            Main.Instance.Say(player, "bank_bought", Main.Instance.Configuration.Instance.BankPrice, Main.Instance.Configuration.Instance.MoneyName);
            Main.Instance.Notify(player, $"{player.DisplayName} has bought a bank!");
        }
    }
}
