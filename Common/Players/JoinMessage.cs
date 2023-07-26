using PlantsVsZombies.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace PlantsVsZombies.Common.Players
{
    public class JoinMessage : ModPlayer
    {
        public override void OnEnterWorld()
        {
            if (ModContent.GetInstance<PlantConfigs>().ShowJoinMessage)
            {
                Main.NewText(
                    """
                    Thanks for downloading this mod!
                    Unfortunately, there isn't much content for you as of yet, some dayblooms and dirt will help you experience it
                    You can [c/FF00000:disable this message] in the config files of this mod
                    Have fun playing!
                    """);
            }
        }
    }
}
