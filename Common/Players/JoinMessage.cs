using PlantsVsZombies.Common.Configs;
using Terraria;
using Terraria.ModLoader;

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
