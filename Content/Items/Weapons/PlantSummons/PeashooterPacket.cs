﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PlantsVsZombies.Common.Systems;
using PlantsVsZombies.Common.Players;
using PlantsVsZombies.Content.Projectiles;
using static PlantsVsZombies.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace PlantsVsZombies.Content.Items.Weapons.PlantSummons
{
    internal class PeashooterPacket : ModItem
    {
        private int sunCost;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Peashooter");
            // Tooltip.SetDefault("They appear to have come from another world\nMaybe they can help with the newly empowered zombies that have been turning up recently");
        }

        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<Plants>();
            Item.damage = 11;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 0.2f;
            Item.consumable = false;
            Item.shoot = ModContent.ProjectileType<Peashooter>(); //Make projectile (sentry), It's projectiles, and ai at a later date (done)
            Item.noMelee = true;
            Item.sentry = true;
            Item.autoReuse = false;

            Item.height = 20;
            Item.width = 30;
            Item.useTime = 10;
            Item.useAnimation = 10;

            sunCost = 100;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "Sun Cost", $"Uses {sunCost} Sun"));
        }

        public override bool CanUseItem(Player player)
        {
            var Sun = player.GetModPlayer<Sun>();

            if (Sun.SunCurrent >= sunCost && (!Main.tile[Main.MouseWorld.ToTileCoordinates()].HasUnactuatedTile || Main.tile[Main.MouseWorld.ToTileCoordinates()].Equals(TileID.Platforms)))
            {
                Sun.SunCurrent -= sunCost;
                return true;
            }
            else { return false; }
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
            return;
        }
    }
}
