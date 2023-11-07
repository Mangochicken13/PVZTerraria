using PlantsVsZombies.Common.Configs;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlantsVsZombies.Common.Systems
{
    public class UpdateConfigBacking : ModSystem
    {
        public override void OnModLoad()
        {
            // Don't run this on the server, its not necessary
            if (Main.netMode == NetmodeID.Server)
                return;

            // setting the message shown to true first prevents accidental triggers
            ModContent.GetInstance<PlantConfigs>().UpdateMessageShown = true;

            string dir = Path.Join(Main.SavePath, "ModConfigs");

            // This should never happen, but a safeguard can't hurt
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string versionPath = Path.Join(Main.SavePath, "ModConfigs", "PlantsVsZombies_LastVersion");

            if (File.Exists(versionPath))
            {
                // If the version in the file isn't the mod version, show the associated message
                string vers = File.ReadAllText(versionPath);
                if (vers != Mod.Version.ToString())
                {
                    ModContent.GetInstance<PlantConfigs>().UpdateMessageShown = false;
                    File.WriteAllText(versionPath, Mod.Version.ToString());
                }
            }
            // If the file doesn't exist, it will be the first installation, so show the message and create the file
            else
            {
                ModContent.GetInstance<PlantConfigs>().UpdateMessageShown = false;
                File.WriteAllText(versionPath, Mod.Version.ToString());
            }

#if DEBUG
            //ModContent.GetInstance<PlantConfigs>().UpdateMessageShown = false;
#endif
        }
    }
}
