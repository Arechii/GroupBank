using fr34kyn01535.Uconomy;
using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using System.Linq;

namespace Arechi.GroupBank.Commands
{
    public class CommandGMoney : IRocketCommand
    {
        public string Name => "gmoney";

        public string Help => "Add or retrieve money from group bank";

        public string Syntax => "<+|-> <amount>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public AllowedCaller AllowedCaller => AllowedCaller.Player; 

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length != 2)
            {
                Plugin.Instance.Say(player, "gmoney_usage");
                return;
            }

            if (!Plugin.Instance.CheckPlayer(player))
                return;

            if (command[0].Equals("+")) //Deposit money to bank
            {
                if (!command[1].All(char.IsDigit) || !uint.TryParse(command[1], out uint money))
                {
                    Plugin.Instance.Say(player, "dep_error");
                    return;
                }

                if (money > (uint)Uconomy.Instance.Database.GetBalance(player.ToString()))
                {
                    Plugin.Instance.Say(player, "dep_error_3", (int)Uconomy.Instance.Database.GetBalance(player.ToString()), Plugin.Instance.Configuration.Instance.MoneyName);
                    return;
                }

                Uconomy.Instance.Database.IncreaseBalance(player.ToString(), -(int)money);
                Plugin.Instance.Notify(player, Plugin.Instance.Translate("bank"));
                Plugin.Instance.Notify(player, Plugin.Instance.Translate("bank_money", Plugin.Instance.Bank.Update(player.SteamGroupID.ToString(), "Money", (int)money) + $" [+{money}]", Plugin.Instance.Configuration.Instance.MoneyName));
            }

            if (command[0].Equals("-")) //Withdraw money from bank
            {
                if (!command[1].All(char.IsDigit) || !uint.TryParse(command[1], out uint money))
                {
                    Plugin.Instance.Say(player, "wit_error");
                    return;
                }

                if (money > Plugin.Instance.Bank.Get(player.SteamGroupID.ToString(), "Money"))
                {
                    Plugin.Instance.Say(player, "wit_error_2");
                    return;
                }

                Uconomy.Instance.Database.IncreaseBalance(player.ToString(), money);
                Plugin.Instance.Notify(player, Plugin.Instance.Translate("bank"));
                Plugin.Instance.Notify(player, Plugin.Instance.Translate("bank_money", Plugin.Instance.Bank.Update(player.SteamGroupID.ToString(), "Money", -(int)money) + $" [-{money}]", Plugin.Instance.Configuration.Instance.MoneyName));
            }
        }
    }
}
