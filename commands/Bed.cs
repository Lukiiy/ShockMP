using Microsoft.Xna.Framework;
using TShockAPI;

namespace ShockMP.commands
{
    public class Bed
    {
        public static void Execute(CommandArgs args)
        {
            if (!args.Player.RealPlayer)
            {
                args.Player.SendErrorMessage("This command can only be used by players!");
                return;
            }

            var bedSpawn = args.TPlayer.SpawnX != -1 ? new Vector2(args.TPlayer.SpawnX, args.TPlayer.SpawnY) : Vector2.Zero;

            if (bedSpawn != Vector2.Zero)
            {
                var distance = Vector2.Distance(args.TPlayer.position / 16, bedSpawn);

                args.Player.SendMessage($"Your bed is located at {bedSpawn.X} {bedSpawn.Y} ({Math.Round(distance)})", Color.Magenta);
            }
            else args.Player.SendErrorMessage("You haven't set your spawn using a bed yet!");
        }
    }
}