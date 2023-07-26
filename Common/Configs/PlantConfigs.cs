using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace PlantsVsZombies.Common.Configs
{
    public class PlantConfigs : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(true)]
        public bool ShowJoinMessage;

        [DefaultValue(false)]
        public bool AlwaysShowSunCount;
    }
}
