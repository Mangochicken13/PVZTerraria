﻿using Microsoft.Xna.Framework;
using PlantsVsZombies.Common.Systems;
using PlantsVsZombies.Content.Projectiles.PlantSentries;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static PlantsVsZombies.Utilities.PlantUtils;

namespace PlantsVsZombies.Content.Items.Weapons.PlantSummons
{
    public class PeashooterPacket : BasePlantPacket
    {
        public static readonly int DefaultSunCost = 100;
        public static readonly int DeafultCooldown = 450;

        public override void SetDefaults()
        {
            base.SetDefaults();
            
            Item.damage = 11;
            Item.knockBack = 0.2f;

            Item.shoot = ModContent.ProjectileType<Peashooter>();

            Item.height = 20;
            Item.width = 30;

            // Fields made by the base class, check their uses in BasePlantPacket.cs
            SunCost = 100;
            Cooldown = 450;
            ID = PlantID.PeashooterPacket;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            GroundedSentrySpawningMethod(42);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 15)
                .AddIngredient(ItemID.Daybloom, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
