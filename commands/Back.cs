using Microsoft.Xna.Framework;
using TShockAPI;

namespace ShockMP
{
    public class Back
    {
        public static void Execute(CommandArgs args)
        {
            TSPlayer target;
            
            if (args.Player.RealPlayer)
            {
                if (args.Parameters.Count > 0 && args.Player.HasPermission("shockmp.backtp"))
                {
                    var players = TSPlayer.FindByNameOrID(args.Parameters[0]);
                    if (players.Count == 0)
                    {
                        args.Player.SendErrorMessage("No player was found");
                        return;
                    }
                    
                    target = players[0];
                }
                else
                {
                    target = args.Player;
                }
            }
            else
            {
                if (args.Parameters.Count == 0)
                {
                    args.Player.SendErrorMessage("You need to specify a player to teleport them to their death point.");
                    return;
                }
                
                var players = TSPlayer.FindByNameOrID(args.Parameters[0]);
                if (players.Count == 0)
                {
                    args.Player.SendErrorMessage("No player was found");
                    return;
                }
                
                target = players[0];
            }

            var deathPoint = target.TPlayer.lastDeathPostion;
            if (deathPoint.X == 0 && deathPoint.Y == 0)
            {
                args.Player.SendErrorMessage(target == args.Player ? "You do not have a recorded death point." : "This player has no recorded death point!");
                return;
            }
            
            if (ShockMP.Config.get("cheatyBack", false)) // cheat
            {
                target.Teleport(deathPoint.X * 16, deathPoint.Y * 16);
                if (target == args.Player)
                    args.Player.SendSuccessMessage("Teleported to your last recorded death point!");
                else
                    args.Player.SendSuccessMessage($"Teleported {target.Name} to their death point.");
            }
            else
            {
                args.Player.SendMessage($"Your death point is located at {(int)deathPoint.X} {(int)deathPoint.Y}", Color.White);
            }
        }
    }
}