using Arechi.GroupBank.Utils;
using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Arechi.GroupBank.Commands
{
    public class CommandGBank : IRocketCommand
    {
        public string Name => "gbank";

        public string Help => "Show current group bank status";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (!Plugin.Instance.CheckPlayer(player)) return;

            player.SendMessage("bank");
            player.SendMessage("bank_xp", Plugin.Instance.Bank.Get(player.SteamGroupID.ToString(), "Experience"));
            player.SendMessage("bank_money", Plugin.Instance.Bank.Get(player.SteamGroupID.ToString(), "Money"), UconomyUtil.MoneyName);
        }
    }
}
