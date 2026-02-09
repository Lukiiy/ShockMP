using Microsoft.Xna.Framework;
using On.Terraria.GameContent;
using Terraria;
using Terraria.ID;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace ShockMP
{
    [ApiVersion(2, 1)]
    public class ShockMP(Main game) : TerrariaPlugin(game)
    {
        public override string Name => "ShockMP";
        public override Version Version => new Version(1, 0);
        public override string Author => "Lukiiy";
        public override string Description => "Shock MP";

        public static ConfigFile Config { get; private set; } = new("shockmp");
        private bool[] sleepingCache = [];

        public override void Initialize()
        {
            GetDataHandlers.ReadNetModule.Register(NetModuleListener);
            ServerApi.Hooks.NetGetData.Register(this, PacketListener);
            On.Terraria.Player.Teleport += OnPlayerTeleport;

            Commands.ChatCommands.Add(new Command(Boop.Execute, "boop"));
            Commands.ChatCommands.Add(new Command(Back.Execute, "back"));
            Commands.ChatCommands.Add(new Command(Bed.Execute, "bed"));

            sleepingCache = new bool[Main.maxPlayers];

            // Config setup
            Config.setIfAbsent("cheatyBack", false);
            Config.setIfAbsent("bedMsg", "{0} is sleeping in a bed. To fast forward time, all players need to sleep.");
            Config.setIfAbsent("infiniteWormholes", false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GetDataHandlers.ReadNetModule.UnRegister(NetModuleListener);
                ServerApi.Hooks.NetGetData.Deregister(this, PacketListener);
                On.Terraria.Player.Teleport -= OnPlayerTeleport;
            }

            base.Dispose(disposing);
        }

        private void PacketListener(GetDataEventArgs args)
        {
            if (args.MsgID == PacketTypes.PlayerUpdate && TShock.Utils.GetActivePlayerCount() > 1)
            {
                int playerId = args.Msg.whoAmI;
                Player player = Main.player[playerId];
                bool sleeping = player.sleeping.isSleeping;

                if (sleeping && !sleepingCache[playerId])
                {
                    sleepingCache[playerId] = true;

                    string msg = string.Format(Config.get("bedMsg", ""), player.name);
                    if (string.IsNullOrEmpty(msg)) return;

                    TShock.Utils.Broadcast(msg, Color.White);
                }

                else if (!sleeping && sleepingCache[playerId]) sleepingCache[playerId] = false;
            }
        }

        private void NetModuleListener(object? obj, GetDataHandlers.ReadNetModuleEventArgs args)
        {
            if (!args.Player.HasPermission("shockmp.mapteleport") || args.ModuleType != GetDataHandlers.NetModuleType.Ping) return;

            var pos = new BinaryReader(args.Data).ReadVector2();
            if (!(pos.X == Tile.Type_Solid && pos.Y == Tile.Type_Solid))
            {
                args.Player.Teleport(pos.X * 16, pos.Y * 16);
                args.Player.SendSuccessMessage($"Teleported to {(int) pos.X} {(int) pos.Y}");
            }

            args.Handled = true; // skip pinging to players
        }

        private static void OnPlayerTeleport(On.Terraria.Player.orig_Teleport orig, Player self, Vector2 newPos, int style, int extraInfo)
        {
            orig(self, newPos, style, extraInfo);

            if (Config.get("infiniteWormholes", false) && style == 3) TShock.Players[self.whoAmI].GiveItem(ItemID.WormholePotion, 1);
        }

    }
}