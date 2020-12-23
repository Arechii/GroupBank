using Arechi.GroupBank.Utils;
using Rocket.API;
using Rocket.Unturned.Player;
using Steamworks;
using System.Collections.Generic;

namespace Arechi.GroupBank.Commands
{
    public class CommandGBuy : IRocketCommand
    {
        public string Name => "gbuy";

        public string Help => "Buys a bank for your group";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (player.SteamGroupID == CSteamID.Nil)
            {
                Plugin.Instance.Say(player, "no_group");
                return;
            }

            if (Plugin.Instance.Bank.HasBank(player.SteamGroupID.ToString()))
            {
                Plugin.Instance.Say(player, "have_bank");
                return;
            }

            if (UconomyUtil.GetBalance(player.Id) < Plugin.Instance.Configuration.Instance.BankPrice)
            {
                Plugin.Instance.Say(player, "bank_error_2", Plugin.Instance.Configuration.Instance.BankPrice, UconomyUtil.MoneyName);
                return;
            }

            UconomyUtil.IncreaseBalance(player.Id, -Plugin.Instance.Configuration.Instance.BankPrice);
            Plugin.Instance.Bank.SetBank(player.SteamGroupID.ToString());
            Plugin.Instance.Say(player, "bank_bought", Plugin.Instance.Configuration.Instance.BankPrice, UconomyUtil.MoneyName);
            Plugin.Instance.Notify(player, $"{player.DisplayName} has bought a bank!");
        }
    }
}
