using TShockAPI;

namespace ShockMP.commands
{
    public class ShockCmd
    {
        public static void Execute(CommandArgs args)
        {
            ShockMP.Config.load();
            args.Player.SendSuccessMessage("ShockMP Config reloaded!");
        }
    }
}