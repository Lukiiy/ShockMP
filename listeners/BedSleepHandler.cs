using Microsoft.Xna.Framework;
using Terraria;
using TShockAPI;

namespace ShockMP.listeners
{
    internal static class BedSleepHandler
    {
        public static void SleepStart(int playerId, Player player)
        {
            int possibleSleeplings = 0;
            int sleeping = 0;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player p = Main.player[i];
                if (p == null) continue;

                if (p.active && !p.dead)
                {
                    possibleSleeplings++;

                    if (p.sleeping.isSleeping) sleeping++;
                }
            }

            string msg;

            if (possibleSleeplings == sleeping)
            {
                msg = ShockMP.Config.get("bedMsgFull", "");
                if (string.IsNullOrEmpty(msg)) return;

                msg = string.Format(msg, player.name);
            }
            else
            {
                msg = ShockMP.Config.get("bedMsg", "");
                if (string.IsNullOrEmpty(msg)) return;

                msg = string.Format(msg, player.name, possibleSleeplings - sleeping);
            }

            TShock.Utils.Broadcast(msg, Color.White);
            TShock.Log.Info($"[World] {msg}");
        }
    }
}