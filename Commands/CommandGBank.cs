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
            var player = (UnturnedPlayer)caller;
            var bank = player.GetBank();

            if (bank == null)
            {
                player.SendMessage("NO_BANK");
                return;
            }

            player.SendMessage("BANK");
            player.SendMessage("BANK_EXP", bank.Experience);
            player.SendMessage("BANK_MONEY", bank.Money, UconomyUtil.MoneyName);
        }
    }
}
