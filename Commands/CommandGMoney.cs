using Arechi.GroupBank.Utils;
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
            var player = (UnturnedPlayer)caller;

            if (command.Length != 2 || !command[0].Equals("+") || !command[0].Equals("-") || 
                !command[1].All(char.IsDigit) || !decimal.TryParse(command[1], out var money))
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
            var balance = player.GetBalance();

            if (money > (deposit ? balance : bank.Money))
            {
                player.SendMessage(deposit ? "DEPOSIT_ERROR" : "WITHDRAW_ERROR", deposit ? balance : bank.Money);
                return;
            }

            bank.Money += (deposit ? money : -money);

            UconomyUtil.IncreaseBalance(player.Id, deposit ? -money : money);
            Plugin.Instance.Bank.UpdateBank(bank);

            player.SendGroupMessage("BANK");
            player.SendGroupMessage("BANK_MONEY", $"{bank.Money} [{(deposit ? "+" : "-")} {money}]", UconomyUtil.MoneyName);
            player.SendGroupMessage("BANK_EXP", bank.Experience);
        }
    }
}
