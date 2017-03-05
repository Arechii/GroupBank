using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace Arechi.GroupBank
{
    public class Plugin : RocketPlugin<Config>
    {
        public static Plugin Instance;
        public Bank Bank;
        public Color color;

        protected override void Load()
        {
            Instance = this;
            Bank = new Bank();
            color = UnturnedChat.GetColorFromName(Configuration.Instance.Color, Color.green);
            Logger.Log("GroupBank has been loaded!");
            int deleted = Bank.DeleteRows();
            if ( deleted > 0) Logger.LogWarning(deleted + " inactive banks have been deleted");
        }

        protected override void Unload()
        {
            Logger.Log("GroupBank has been unloaded!");
        }

        public void Say(UnturnedPlayer player, string key, params object[] args)
        {
            UnturnedChat.Say(player, Translate(key, args), color);
        }

        public void Notify(CSteamID player, CSteamID sgroup, string msg)
        {
            foreach (var splayer in Provider.clients)
            {
                if (splayer.playerID.group == sgroup)
                {
                    ChatManager.instance.channel.send("tellChat", splayer.playerID.steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[] { player, (byte)EChatMode.GROUP, color, msg });
                }
            }
        }

        public bool CheckPlayer(UnturnedPlayer player)
        {
            if (player.SteamGroupID == CSteamID.Nil)
            {
                Say(player, "no_group"); return false;
            }

            if (!Plugin.Instance.Bank.HasBank(player.SteamGroupID.ToString()))
            {
                Say(player, "no_bank"); return false;
            }

            return true;
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList()
                {
                    {"bank", "｢ Group Bank ｣"},
                    {"bank_xp", "# Experience: {0}"},
                    {"bank_money", "# {1}: {0}"},
                    {"gbuy_usage", "Error in command. Try /gbuy"},
                    {"gbank_usage", "Error in command. Try /gbank"},
                    {"gxp_usage", "Error in command. Try /gxp +|- amount"},
                    {"gmoney_usage", "Error in command. Try /gmoney +|- amount"},
                    {"no_group", "You do not have a group!"},
                    {"no_bank", "Your group does not have a bank!"},
                    {"have_bank", "Your group already has a bank!"},
                    {"bank_error", "Bank creation failed. You already have one for your group!"},
                    {"bank_error_2", "Bank creation failed. You need to pay {0} {1} for one!"},
                    {"bank_bought", "Succesfully bought a bank for your group! You paid {0} {1}."},
                    {"dep_error", "Something went wrong. Input a number."},
                    {"dep_error_2", "You only have {0} Experience!"},
                    {"dep_error_3", "You only have {0} {1}!"},
                    {"wit_error", "Something went wrong. Input a number."},
                    {"wit_error_2", "Your bank does not have this!"},
                };
            }
        }
    }
}
