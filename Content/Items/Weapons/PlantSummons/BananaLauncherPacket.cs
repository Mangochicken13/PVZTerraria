using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlantsVsZombies.Common.Players;
using PlantsVsZombies.Common.Systems;
using PlantsVsZombies.Content.Projectiles.PlantSentries;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static PlantsVsZombies.Utilities.PlantUtils;

namespace PlantsVsZombies.Content.Items.Weapons.PlantSummons
{
    public class BananaLauncherPacket : ModItem
    {
        private int sunCost;
        private int cooldown; //Change value

        public override void SetDefaults()
        {
            Item.damage = 864;
            Item.DamageType = ModContent.GetInstance<Plants>();
            Item.width = 18;
            Item.height = 22;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<BananaLauncher>();

            sunCost = 700;
            cooldown = 6000;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AddSunCost(ref tooltips, Mod, sunCost);
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                return true;
            }
            if (CheckCanUse(player, sunCost, PlantID.BananaLauncherPacket, cooldown))
            {
                return true;
            }
            return false;
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            DrawPlantCooldown(ref spriteBatch, ref position, PlantID.BananaLauncherPacket, cooldown);
        }
        public override bool? UseItem(Player player)
        {
            
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.altFunctionUse == 2)
                {
                    player.GetModPlayer<BananaTarget>().target = Main.MouseWorld;
                    return true;
                }
                else
                {
                    return null;
                }
            }
            return false;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                type = ModContent.ProjectileType<BananaTargetReticle>();
                position = Main.MouseWorld;
            }
            else
            {
                SentrySpawningMethod(ref position, 68);
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
