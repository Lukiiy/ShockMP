using Microsoft.Xna.Framework;
using TShockAPI;

namespace ShockMP
{
    public class Boop
    {
        public static void Execute(CommandArgs args)
        {
            if (args.Parameters.Count == 0)
            {
                args.Player.SendErrorMessage("You do need to specify someone to boop.");
                return;
            }

            var players = TSPlayer.FindByNameOrID(args.Parameters[0]);
            if (players.Count == 0)
            {
                args.Player.SendErrorMessage("Player not found.");
                return;
            }
            if (players.Count > 1)
            {
                args.Player.SendMultipleMatchError(players.Select(p => p.Name));
                return;
            }

            players[0].SendMessage($"{(args.Player.RealPlayer ? args.Player.Name : "Server")} booped you!", Color.Magenta);
        }
    }
}