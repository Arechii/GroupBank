using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Arechi.GroupBank
{
    public class CGBank : IRocketCommand
    {
        public string Name { get { return "gbank"; } }
        public string Help { get { return "Show current group bank status"; } }
        public string Syntax { get { return ""; } }
        public List<string> Aliases { get { return new List<string>(); } }
        public List<string> Permissions { get { return new List<string>() { "gbank" }; } }
        public AllowedCaller AllowedCaller { get { return AllowedCaller.Player; } }
        

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length == 0)
            {
                if (Plugin.Instance.CheckPlayer(player) == true)
                {
                    Plugin.Instance.Say(player, "bank");
                    Plugin.Instance.Say(player, "bank_xp", Plugin.Instance.Bank.Get(player.SteamGroupID.ToString(), "Experience"));
                    Plugin.Instance.Say(player, "bank_money", Plugin.Instance.Bank.Get(player.SteamGroupID.ToString(), "Money"), Plugin.Instance.Configuration.Instance.MoneyName);
                }
            }
            else
            {
                Plugin.Instance.Say(player, "gbank_usage");
            }
        }
    }
}
