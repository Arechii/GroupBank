using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Linq;
using UnityEngine;

namespace Arechi.GroupBank.Utils
{
    public static class ChatUtil
    {
        public static void SendMessage(this UnturnedPlayer player, string message, EChatMode mode = EChatMode.SAY, params object[] args)
        {
            Color color = Color.green;

            if (ColorUtility.TryParseHtmlString(Plugin.Instance.Configuration.Instance.Color, out var parsedColor))
                color = parsedColor;

            ChatManager.serverSendMessage(Plugin.Instance.Translate(message, args), color, null, player.SteamPlayer(), mode, 
                Plugin.Instance.Configuration.Instance.IconURL, true);
        }

        public static void SendMessage(this UnturnedPlayer player, string message, params object[] args)
        {
            player.SendMessage(message, EChatMode.SAY, args);
        }

        public static void SendGroupMessage(this UnturnedPlayer player, string message, params object[] args)
        {
            Provider.clients
                .Where(p => p.playerID.group == player.SteamGroupID)
                .Select(UnturnedPlayer.FromSteamPlayer)
                .ToList()
                .ForEach(p => p.SendMessage(message, EChatMode.GROUP, args));
        }
    }
}
