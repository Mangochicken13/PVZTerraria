using PlantsVsZombies.Common.Systems;
using Terraria.GameInput;
using Terraria.ModLoader;
using System;
using Terraria;
using PlantsVsZombies.Common.UI;
using Terraria.UI;

namespace PlantsVsZombies.Common.Players
{
    internal class KeybindPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybindSystem.ToggleUI.JustPressed)
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
        }
    }
}
