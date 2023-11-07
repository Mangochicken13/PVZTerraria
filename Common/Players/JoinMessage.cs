using PlantsVsZombies.Common.Configs;
using Terraria;
using Terraria.ID;
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

            if (Main.netMode != NetmodeID.Server)
            {
                bool updateMessageShown = ModContent.GetInstance<PlantConfigs>().UpdateMessageShown;

                if (!updateMessageShown)
                {
                    Main.NewText(
                        $"""
                        Updated to version {ModContent.GetInstance<PlantsVsZombies>().Version}
                        This is just a small maintanence update fixing the cooldown UI not drawing properly (how did i miss that?)
                        If you find any bugs please mention them on the workshop page's bug reporting discussion.
                        This message will only show this one time, future version updates may have their own messages too.
                        """);
                    ModContent.GetInstance<PlantConfigs>().UpdateMessageShown = true;
                }
            }
        }
    }
}
