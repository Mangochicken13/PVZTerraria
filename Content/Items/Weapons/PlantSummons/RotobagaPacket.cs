using System.Collections.Generic;
using Terraria.ModLoader;
using static PlantsVsZombies.Utilities.PlantUtils;

namespace PlantsVsZombies.Content.Items.Weapons.PlantSummons
{

    public class RotobagaPacket : ModItem
    {
        int sunCost = 175;

        public override void SetDefaults()
        {
            QuickItem.SetPlantSummon(this, 40, 40, 5, 0, 15, 15);
            Item.shoot = ModContent.ProjectileType<Projectiles.PlantSentries.Rotobaga>();

            sunCost = 175;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AddSunCost(ref tooltips, Mod, sunCost);
        }
    }
}
