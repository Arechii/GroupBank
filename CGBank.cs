using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Arechi.GroupBank
{
    public class CGBank : IRocketCommand
    {
        public string Name => "gbank";
        public string Help => "Show current group bank status";
        public string Syntax => string.Empty;
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>() { "gbank" };
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (Main.Instance.CheckPlayer(player) == true)
            {
                Main.Instance.Say(player, "bank");
                Main.Instance.Say(player, "bank_xp", Main.Instance.Bank.Get(player.SteamGroupID.ToString(), "Experience"));
                Main.Instance.Say(player, "bank_money", Main.Instance.Bank.Get(player.SteamGroupID.ToString(), "Money"), Main.Instance.Configuration.Instance.MoneyName);
            }
        }
    }
}
