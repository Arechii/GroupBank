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
            var player = (UnturnedPlayer)caller;

            if (player.GetGroup() == CSteamID.Nil)
            {
                player.SendMessage("NO_GROUP");
                return;
            }

            if (player.GetBank() != null)
            {
                player.SendMessage("ALREADY_HAVE_A_BANK");
                return;
            }

            if (UconomyUtil.GetBalance(player.Id) < Plugin.Instance.Configuration.Instance.BankPrice)
            {
                player.SendMessage("BANK_BUY_ERROR", Plugin.Instance.Configuration.Instance.BankPrice, UconomyUtil.MoneyName);
                return;
            }

            UconomyUtil.IncreaseBalance(player.Id, -Plugin.Instance.Configuration.Instance.BankPrice);
            Plugin.Instance.Bank.AddBank(player.SteamGroupID.ToString());

            player.SendMessage("BANK_BOUGHT", Plugin.Instance.Configuration.Instance.BankPrice, UconomyUtil.MoneyName);
            player.SendGroupMessage("BANK_BOUGHT_GROUP", player.DisplayName);
        }
    }
}
