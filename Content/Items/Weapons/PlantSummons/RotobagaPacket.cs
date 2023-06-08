﻿using Terraria.ModLoader;

namespace PlantsVsZombies.Content.Items.Weapons.PlantSummons
{
    
    public class RotobagaPacket : ModItem
    {
        public override string Texture => "PlantsVsZombies/Content/Items/Weapons/PlantSummons/PeashooterPacket";

        int sunCost;
        public override void SetDefaults()
        {
            QuickItem.SetPlantSummon(this, 40, 40, 5, 0, 15, 15);
            Item.shoot = ModContent.ProjectileType<Rotobaga>();

            sunCost = 175;
        }
    }
    
}
