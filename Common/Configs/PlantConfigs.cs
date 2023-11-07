using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.ModLoader.Config;

namespace PlantsVsZombies.Common.Configs
{
    public class PlantConfigs : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        public bool ShowJoinMessage;

        [DefaultValue(false)]
        public bool AlwaysShowSunCount;

        [DefaultValue(true)]
        public bool ShowUpdateMessages;

        internal bool UpdateMessageShown;
    }
}
