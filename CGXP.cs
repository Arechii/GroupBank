using Rocket.API;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;

namespace Arechi.GroupBank
{
    public class CGXP : IRocketCommand
    {
        public string Name { get { return "gxp"; } }
        public string Help { get { return "Add or retrieve experience from group bank"; } }
        public string Syntax { get { return "<+|-> <amount>"; } }
        public List<string> Aliases { get { return new List<string>(); } }
        public List<string> Permissions { get { return new List<string>() { "gxp" }; } }
        public AllowedCaller AllowedCaller { get { return AllowedCaller.Player; } }
        

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length == 2)
            {
                if (Plugin.Instance.CheckPlayer(player) == true)
                {
                    if (string.Compare(command[0], "+", true) == 0) //Deposit xp to bank
                    {
                        int xp;
                        if (!Int32.TryParse(command[1], out xp) || (Int32.TryParse(command[1], out xp) && xp <= 0))
                        {
                            Plugin.Instance.Say(player, "dep_error"); return;
                        }

                        if (xp > (int)player.Experience)
                        {
                            Plugin.Instance.Say(player, "dep_error_2", player.Experience); return;
                        }

                        player.Experience -= (uint)xp;
                        Plugin.Instance.Notify(player.CSteamID, player.SteamGroupID, Plugin.Instance.Translate("bank"));
                        Plugin.Instance.Notify(player.CSteamID, player.SteamGroupID, Plugin.Instance.Translate("bank_xp", Plugin.Instance.Bank.Update(player.SteamGroupID.ToString(), "Experience", xp) + " [+" + xp + "]"));
                    }

                    if (string.Compare(command[0], "-", true) == 0) //Withdraw xp from bank
                    {
                        int xp;
                        if (!Int32.TryParse(command[1], out xp) || (Int32.TryParse(command[1], out xp) && xp <= 0))
                        {
                            Plugin.Instance.Say(player, "wit_error"); return;
                        }

                        if (xp > Plugin.Instance.Bank.Get(player.SteamGroupID.ToString(), "Experience"))
                        {
                            Plugin.Instance.Say(player, "wit_error_2"); return;
                        }

                        player.Experience += (uint)xp;
                        Plugin.Instance.Notify(player.CSteamID, player.SteamGroupID, Plugin.Instance.Translate("bank"));
                        Plugin.Instance.Notify(player.CSteamID, player.SteamGroupID, Plugin.Instance.Translate("bank_xp", Plugin.Instance.Bank.Update(player.SteamGroupID.ToString(), "Experience", -xp) + " [-" + xp + "]"));
                    }
                }
            }
            else
            {
                Plugin.Instance.Say(player, "gxp_usage");
            }
        }
    }
}
