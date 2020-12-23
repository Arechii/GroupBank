using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;

namespace Arechi.GroupBank.Utils
{
    public static class PlayerUtil
    {
        public static decimal GetBalance(this UnturnedPlayer player)
        {
            return UconomyUtil.GetBalance(player.Id);
        }

        public static Bank GetBank(this UnturnedPlayer player)
        {
            var group = player.GetGroup();

            if (group == CSteamID.Nil)
                return null;

            return Plugin.Instance.Bank.GetBank(group.ToString());
        }

        public static CSteamID GetGroup(this UnturnedPlayer player)
        {
            var group = GroupManager.getGroupInfo(player.Player.quests.groupID);

            return group != null ? group.groupID : player.SteamGroupID;
        }
    }
}
