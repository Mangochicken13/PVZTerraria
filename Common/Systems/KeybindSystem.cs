using Terraria;
using Terraria.ModLoader;
using PlantsVsZombies.Common.UI;
using Terraria.GameInput;
using Microsoft.Xna.Framework;

namespace PlantsVsZombies.Common.Systems
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind Dev_ToggleUI { get; set; }
        public static ModKeybind Dev_ExtraSun { get; set; }
        public override void Load()
        {
#if DEBUG
            Dev_ToggleUI = KeybindLoader.RegisterKeybind(Mod, "Toggle UI", "U");
            Dev_ExtraSun = KeybindLoader.RegisterKeybind(Mod, "Add 50 sun", "=");
#endif
        }
        public override void Unload()
        {
#if DEBUG
            Dev_ToggleUI = null;
            Dev_ExtraSun = null;
#endif
        }
    }
}
