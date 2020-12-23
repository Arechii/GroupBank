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
                Plugin.Instance.Say(player, "gxp_usage");
                return;
            }

            if (!Plugin.Instance.CheckPlayer(player))
                return;

            if (command[0].Equals("+")) //Deposit xp to bank
            {
                if (!command[1].All(char.IsDigit) || !uint.TryParse(command[1], out uint xp))
                {
                    Plugin.Instance.Say(player, "dep_error");
                    return;
                }

                if (xp > player.Experience)
                {
                    Plugin.Instance.Say(player, "dep_error_2", player.Experience);
                    return;
                }

                player.Experience -= xp;
                Plugin.Instance.Notify(player, Plugin.Instance.Translate("bank"));
                Plugin.Instance.Notify(player, Plugin.Instance.Translate("bank_xp", Plugin.Instance.Bank.Update(player.SteamGroupID.ToString(), "Experience", (int)xp) + $" [+{xp}]"));
            }

            if (command[0].Equals("-")) //Withdraw xp from bank
            {
                if (!command[1].All(char.IsDigit) || !uint.TryParse(command[1], out uint xp))
                {
                    Plugin.Instance.Say(player, "wit_error");
                    return;
                }

                if (xp > Plugin.Instance.Bank.Get(player.SteamGroupID.ToString(), "Experience"))
                {
                    Plugin.Instance.Say(player, "wit_error_2");
                    return;
                }

                player.Experience += xp;
                Plugin.Instance.Notify(player, Plugin.Instance.Translate("bank"));
                Plugin.Instance.Notify(player, Plugin.Instance.Translate("bank_xp", Plugin.Instance.Bank.Update(player.SteamGroupID.ToString(), "Experience", -(int)xp) + $" [-{xp}]"));
            }
        }
    }
}
