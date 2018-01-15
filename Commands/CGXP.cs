using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using System.Linq;

namespace Arechi.GroupBank.Commands
{
    public class CGXP : IRocketCommand
    {
        public string Name => "gxp";

        public string Help => "Add or retrieve experience from group bank";

        public string Syntax => "<+|-> <amount>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "gxp" };

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length != 2)
            {
                Main.Instance.Say(player, "gxp_usage");
                return;
            }

            if (!Main.Instance.CheckPlayer(player))
                return;

            if (command[0].Equals("+")) //Deposit xp to bank
            {
                if (!command[1].All(char.IsDigit) || !uint.TryParse(command[1], out uint xp))
                {
                    Main.Instance.Say(player, "dep_error");
                    return;
                }

                if (xp > player.Experience)
                {
                    Main.Instance.Say(player, "dep_error_2", player.Experience);
                    return;
                }

                player.Experience -= xp;
                Main.Instance.Notify(player, Main.Instance.Translate("bank"));
                Main.Instance.Notify(player, Main.Instance.Translate("bank_xp", Main.Instance.Bank.Update(player.SteamGroupID.ToString(), "Experience", (int)xp) + $" [+{xp}]"));
            }

            if (command[0].Equals("-")) //Withdraw xp from bank
            {
                if (!command[1].All(char.IsDigit) || !uint.TryParse(command[1], out uint xp))
                {
                    Main.Instance.Say(player, "wit_error");
                    return;
                }

                if (xp > Main.Instance.Bank.Get(player.SteamGroupID.ToString(), "Experience"))
                {
                    Main.Instance.Say(player, "wit_error_2");
                    return;
                }

                player.Experience += xp;
                Main.Instance.Notify(player, Main.Instance.Translate("bank"));
                Main.Instance.Notify(player, Main.Instance.Translate("bank_xp", Main.Instance.Bank.Update(player.SteamGroupID.ToString(), "Experience", -(int)xp) + $" [-{xp}]"));
            }
        }
    }
}
