using Terraria;
using Terraria.ModLoader;
using PlantsVsZombies.Common.UI;
using Terraria.GameInput;
using Microsoft.Xna.Framework;

namespace PlantsVsZombies.Common.Systems
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind ToggleUI { get; set; }
        public override void Load()
        {
            ToggleUI = KeybindLoader.RegisterKeybind(Mod, "Toggle UI", "U");
        }
        public override void Unload()
        {
            ToggleUI = null;
        }
    }
}
