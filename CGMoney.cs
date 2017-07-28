using fr34kyn01535.Uconomy;
using Rocket.API;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arechi.GroupBank
{
    public class CGMoney : IRocketCommand
    {
        public string Name => "gmoney";
        public string Help => "Add or retrieve money from group bank";
        public string Syntax => "<+|-> <amount>";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>() { "gmoney" };
        public AllowedCaller AllowedCaller => AllowedCaller.Player; 

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length != 2)
            {
                Main.Instance.Say(player, "gmoney_usage");
                return;
            }

            if (Main.Instance.CheckPlayer(player))
            {
                if (command[0].Equals("+")) //Deposit money to bank
                {
                    int money;

                    if (!command[1].All(Char.IsDigit) || !Int32.TryParse(command[1], out money) || (Int32.TryParse(command[1], out money) && money <= 0))
                    {
                        Main.Instance.Say(player, "dep_error");
                        return;
                    }

                    if (money > (int)Uconomy.Instance.Database.GetBalance(player.ToString()))
                    {
                        Main.Instance.Say(player, "dep_error_3", (int)Uconomy.Instance.Database.GetBalance(player.ToString()), 
                            Main.Instance.Configuration.Instance.MoneyName);
                        return;
                    }

                    Uconomy.Instance.Database.IncreaseBalance(player.ToString(), -money);
                    Main.Instance.Notify(player, Main.Instance.Translate("bank"));
                    Main.Instance.Notify(player, 
                        Main.Instance.Translate("bank_money", Main.Instance.Bank.Update(player.SteamGroupID.ToString(), "Money", money) + $" [+{money}]", Main.Instance.Configuration.Instance.MoneyName));
                }

                if (command[0].Equals("-")) //Withdraw money from bank
                {
                    int money;

                    if (!command[1].All(Char.IsDigit) || !Int32.TryParse(command[1], out money) || (Int32.TryParse(command[1], out money) && money <= 0))
                    {
                        Main.Instance.Say(player, "wit_error");
                        return;
                    }

                    if (money > Main.Instance.Bank.Get(player.SteamGroupID.ToString(), "Money"))
                    {
                        Main.Instance.Say(player, "wit_error_2");
                        return;
                    }

                    Uconomy.Instance.Database.IncreaseBalance(player.ToString(), money);
                    Main.Instance.Notify(player, Main.Instance.Translate("bank"));
                    Main.Instance.Notify(player, 
                        Main.Instance.Translate("bank_money", Main.Instance.Bank.Update(player.SteamGroupID.ToString(), "Money", -money) + $" [-{money}]", Main.Instance.Configuration.Instance.MoneyName));
                }
            }
        }
    }
}
