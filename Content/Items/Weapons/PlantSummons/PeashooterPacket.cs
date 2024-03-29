﻿using Microsoft.Xna.Framework;
using PlantsVsZombies.Content.Projectiles.PlantSentries;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlantsVsZombies.Content.Items.Weapons.PlantSummons
{
    public class PeashooterPacket : BasePlantPacket
    {
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
            ID = PlantID.Peashooter;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int _);
            int pushupY = 42 / 2; // Note to Self: find some way to access the projectile hitbox and use that instead of magic number 42 (the Y of the peashooter hitbox)
            position = new Vector2(worldX, worldY - pushupY);
            
            //GroundedSentrySpawningMethod(42);
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
