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
            var player = (UnturnedPlayer)caller;

            if (command.Length != 2 || !command[0].Equals("+") || !command[0].Equals("-") ||
                !command[1].All(char.IsDigit) || !uint.TryParse(command[1], out var experience))
            {
                player.SendMessage($"/{Name} {Syntax}");
                return;
            }

            var bank = player.GetBank();

            if (bank == null)
            {
                player.SendMessage("NO_BANK");
                return;
            }

            var deposit = command[0].Equals("+");

            if (experience > (deposit ? player.Experience : bank.Experience))
            {
                player.SendMessage(deposit ? "DEPOSIT_ERROR" : "WITHDRAW_ERROR", deposit ? player.Experience : bank.Experience);
                return;
            }

            if (deposit)
            {
                bank.Experience += experience;
                player.Experience -= experience;
            }
            else
            {
                bank.Experience -= experience;
                player.Experience += experience;
            }

            Plugin.Instance.Bank.UpdateBank(bank);

            player.SendGroupMessage("BANK");
            player.SendGroupMessage("BANK_MONEY", bank.Money, UconomyUtil.MoneyName);
            player.SendGroupMessage("BANK_EXP", $"{bank.Experience} [{(deposit ? "+" : "-")} {experience}]");
        }
    }
}
