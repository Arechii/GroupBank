using Arechi.GroupBank.Utils;
using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using System.Linq;

namespace Arechi.GroupBank.Commands
{
    public class CommandGXP : IRocketCommand
    {
        public string Name => "gxp";

        public string Help => "Add or retrieve experience from group bank";

        public string Syntax => "<+|-> <amount>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length != 2)
            {
                player.SendMessage("gxp_usage");
                return;
            }

            if (!Plugin.Instance.CheckPlayer(player))
                return;

            var bank = Plugin.Instance.Bank.GetBank(player.SteamGroupID.ToString());

            if (command[0].Equals("+")) //Deposit xp to bank
            {
                if (!command[1].All(char.IsDigit) || !uint.TryParse(command[1], out uint xp))
                {
                    player.SendMessage("dep_error");
                    return;
                }

                if (xp > player.Experience)
                {
                    player.SendMessage("dep_error_2", player.Experience);
                    return;
                }

                player.Experience -= xp;
                bank.Experience += xp;
                Plugin.Instance.Bank.UpdateBank(bank);
                player.SendGroupMessage("bank");
                player.SendGroupMessage("bank_xp", $"{bank.Experience} [+{xp}]");
            }

            if (command[0].Equals("-")) //Withdraw xp from bank
            {
                if (!command[1].All(char.IsDigit) || !uint.TryParse(command[1], out uint xp))
                {
                    player.SendMessage("wit_error");
                    return;
                }

                if (xp > bank.Experience)
                {
                    player.SendMessage("wit_error_2");
                    return;
                }

                player.Experience += xp;
                bank.Experience -= xp;
                Plugin.Instance.Bank.UpdateBank(bank);
                player.SendGroupMessage("bank");
                player.SendGroupMessage("bank_xp", $"{bank.Experience} [-{xp}]");
            }
        }
    }
}
