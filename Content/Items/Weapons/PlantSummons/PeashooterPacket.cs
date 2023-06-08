using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PlantsVsZombies.Common.Systems;
using PlantsVsZombies.Common.Players;
using static PlantsVsZombies.Utilities; //attempted to use a utility function, didn't have expected results
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using PlantsVsZombies.Content.Projectiles.PlantSentries;

namespace PlantsVsZombies.Content.Items.Weapons.PlantSummons
{
    internal class PeashooterPacket : ModItem
    {
        private int sunCost;
        /*
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Peashooter"); -again, deprecated method
            // Tooltip.SetDefault("They appear to have come from another world\nMaybe they can help with the newly empowered zombies that have been turning up recently");
        }
        */

        public override void SetDefaults()
        {
            //setting all the stats for the item. Important one is the damage type, as that is used by my ui
            Item.DamageType = ModContent.GetInstance<Plants>();
            Item.damage = 11;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 0.2f;
            Item.consumable = false;
            Item.shoot = ModContent.ProjectileType<Peashooter>(); //Make projectile (sentry), It's projectiles, and ai at a later date (done)
            Item.noMelee = true;
            Item.sentry = true;

            Item.height = 20;
            Item.width = 30;
            Item.useTime = 10;
            Item.useAnimation = 10;

            sunCost = 100;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //adding the sun cost to the tooltip, as it is a modded requirement
            tooltips.Add(new TooltipLine(Mod, "Sun Cost", $"Uses {sunCost} Sun"));
        }

        public override bool CanUseItem(Player player)
        {
            var Sun = player.GetModPlayer<Sun>();

            if (Sun.SunCurrent >= sunCost && ((!Main.tileSolid[Main.tile[Main.MouseWorld.ToTileCoordinates()].TileType] || Main.tileSolidTop[Main.tile[Main.MouseWorld.ToTileCoordinates()].TileType]) || !Main.tile[Main.MouseWorld.ToTileCoordinates()].HasTile))
            {
                Sun.SunCurrent -= sunCost;
                return true;
            }
            else { return false; }
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            var potentialTile = Main.MouseWorld.ToTileCoordinates();

            while (!Main.tileSolid[Main.tile[potentialTile].TileType] || !Main.tile[potentialTile].HasTile)
            {
                potentialTile += new Point(0, 1);
            }
            position = potentialTile.ToWorldCoordinates() - new Vector2(0, 29);
            if (Main.tile[potentialTile].IsHalfBlock) { position += new Vector2(0, 8); }

            return;
        }
    }
}
