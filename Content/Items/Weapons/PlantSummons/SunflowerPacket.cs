using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PlantsVsZombies.Common.Players;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

using static PlantsVsZombies.Utilities;
using PlantsVsZombies.Common.Systems;
using PlantsVsZombies.Content.Projectiles.PlantSentries;

namespace PlantsVsZombies.Content.Items.Weapons.PlantSummons
{
    public class SunflowerPacket : ModItem
    {
        private int sunCost;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sunflower");
            // Tooltip.SetDefault("An incredibly happy flower\nThey seem to be able to produce fun size pockets of sun energy for your use");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = false;
            Item.width = 24;
            Item.height = 36;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.DamageType = ModContent.GetInstance<Plants>();
            Item.shoot = ModContent.ProjectileType<Sunflower>();
            Item.noMelee = true;

            sunCost = 50;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "Sun Cost", $"Uses {sunCost} Sun"));
        }

        public override bool CanUseItem(Player player)
        {
            //checking if the player has enough sun to use the item
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
            SentrySpawningMethod(ref position, Main.MouseWorld, 56);

            /*
            var potentialTile = Main.MouseWorld.ToTileCoordinates();

            while (!Main.tileSolid[Main.tile[potentialTile].TileType] || !Main.tile[potentialTile].HasTile)
            {
                potentialTile += new Point(0, 1);
            }
            position = potentialTile.ToWorldCoordinates() - new Vector2(0, 35);
            if (Main.tile[potentialTile].IsHalfBlock) { position += new Vector2(0, 8); }
            return;
            */
        }
    }
}
