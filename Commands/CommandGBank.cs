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

            if (Plugin.Instance.CheckPlayer(player))
            {
                Plugin.Instance.Say(player, "bank");
                Plugin.Instance.Say(player, "bank_xp", Plugin.Instance.Bank.Get(player.SteamGroupID.ToString(), "Experience"));
                Plugin.Instance.Say(player, "bank_money", Plugin.Instance.Bank.Get(player.SteamGroupID.ToString(), "Money"), Plugin.Instance.Configuration.Instance.MoneyName);
            }
        }
    }
}
