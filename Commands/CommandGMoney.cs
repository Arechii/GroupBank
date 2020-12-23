using Arechi.GroupBank.Utils;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
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
                player.SendMessage("gmoney_usage");
                return;
            }

            if (!Plugin.Instance.CheckPlayer(player))
                return;

            if (command[0].Equals("+")) //Deposit money to bank
            {
                if (!command[1].All(char.IsDigit) || !uint.TryParse(command[1], out uint money))
                {
                    player.SendMessage("dep_error");
                    return;
                }

                if (money > (uint)UconomyUtil.GetBalance(player.Id))
                {
                    player.SendMessage("dep_error_3", EChatMode.SAY, (int)UconomyUtil.GetBalance(player.Id), UconomyUtil.MoneyName);
                    return;
                }

                UconomyUtil.IncreaseBalance(player.Id, -(int)money);
                player.SendGroupMessage("bank");
                player.SendGroupMessage("bank_money", Plugin.Instance.Bank.Update(player.SteamGroupID.ToString(), "Money", (int)money) + $" [+{money}]", UconomyUtil.MoneyName);
            }

            if (command[0].Equals("-")) //Withdraw money from bank
            {
                if (!command[1].All(char.IsDigit) || !uint.TryParse(command[1], out uint money))
                {
                    player.SendMessage("wit_error");
                    return;
                }

                if (money > Plugin.Instance.Bank.Get(player.SteamGroupID.ToString(), "Money"))
                {
                    player.SendMessage("wit_error_2");
                    return;
                }

                UconomyUtil.IncreaseBalance(player.Id, money);
                player.SendGroupMessage("bank");
                player.SendGroupMessage("bank_money", Plugin.Instance.Bank.Update(player.SteamGroupID.ToString(), "Money", -(int)money) + $" [-{money}]", UconomyUtil.MoneyName);
            }
        }
    }
}
