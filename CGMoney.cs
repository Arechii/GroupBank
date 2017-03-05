using fr34kyn01535.Uconomy;
using Rocket.API;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;

namespace Arechi.GroupBank
{
    public class CGMoney : IRocketCommand
    {
        public string Help { get { return "Add or retrieve money from group bank"; } }
        public string Name { get { return "gmoney"; } }
        public string Syntax { get { return "<+|-> <amount>"; } }
        public List<string> Aliases { get { return new List<string>(); } }
        public AllowedCaller AllowedCaller { get { return AllowedCaller.Player; } }
        public List<string> Permissions { get { return new List<string>() { "gmoney" }; } }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length == 2)
            {
                if (Plugin.Instance.CheckPlayer(player) == true)
                {
                    if (string.Compare(command[0], "+", true) == 0) //Deposit money to bank
                    {
                        int money;
                        if (!Int32.TryParse(command[1], out money))
                        {
                            Plugin.Instance.Say(player, "dep_error"); return;
                        }

                        if (money > (int)Uconomy.Instance.Database.GetBalance(player.ToString()))
                        {
                            Plugin.Instance.Say(player, "dep_error_3", (int)Uconomy.Instance.Database.GetBalance(player.ToString()), Plugin.Instance.Configuration.Instance.MoneyName); return;
                        }

                        Uconomy.Instance.Database.IncreaseBalance(player.ToString(), -money);
                        Plugin.Instance.Notify(player.CSteamID, player.SteamGroupID, Plugin.Instance.Translate("bank"));
                        Plugin.Instance.Notify(player.CSteamID, player.SteamGroupID, Plugin.Instance.Translate("bank_money", Plugin.Instance.Bank.Update(player.SteamGroupID.ToString(), "Money", money) + " [+" + money + "]", Plugin.Instance.Configuration.Instance.MoneyName));
                    }

                    if (string.Compare(command[0], "-", true) == 0) //Withdraw money from bank
                    {
                        int money;
                        if (!Int32.TryParse(command[1], out money))
                        {
                            Plugin.Instance.Say(player, "wit_error"); return;
                        }

                        if (money > Plugin.Instance.Bank.Get(player.SteamGroupID.ToString(), "Money"))
                        {
                            Plugin.Instance.Say(player, "wit_error_2"); return;
                        }

                        Uconomy.Instance.Database.IncreaseBalance(player.ToString(), money);
                        Plugin.Instance.Notify(player.CSteamID, player.SteamGroupID, Plugin.Instance.Translate("bank"));
                        Plugin.Instance.Notify(player.CSteamID, player.SteamGroupID, Plugin.Instance.Translate("bank_money", Plugin.Instance.Bank.Update(player.SteamGroupID.ToString(), "Money", -money) + " [-" + money + "]", Plugin.Instance.Configuration.Instance.MoneyName));
                    }
                }            
            }
            else
            {
                Plugin.Instance.Say(player, "gmoney_usage");
            }
        }
    }
}
