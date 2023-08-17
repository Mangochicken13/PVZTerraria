using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using PlantsVsZombies.Common.UI;
using PlantsVsZombies.Common.Systems;

namespace PlantsVsZombies.Common.Players
{
    public class KeybindPlayer : ModPlayer
    {
        //these key bindings do certain things in the game, namely toggling ui, and adding some extra sun
        //both of these are dev features, and won't stick around for the full mod release
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybindSystem.Dev_ToggleUI.JustPressed)
            {
                if (ModContent.GetInstance<SunUISystem>().MyInterface?.CurrentState == null)
                {
                    ModContent.GetInstance<SunUISystem>().ShowMyUI();
                    Main.NewText("Ui Shown");
                }
                else if (ModContent.GetInstance<SunUISystem>().MyInterface?.CurrentState != null)
                {
                    ModContent.GetInstance<SunUISystem>().HideMyUI();
                    Main.NewText("Ui Hidden");
                }
            }

            if (KeybindSystem.Dev_ExtraSun.JustPressed)
            {
                var Sun = Player.GetModPlayer<Sun>();
                Sun.SunCurrent += 50;
            }
        }
    }
}
