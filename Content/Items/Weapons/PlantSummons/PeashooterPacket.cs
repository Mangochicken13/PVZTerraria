using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using PlantsVsZombies.Common.Systems;
using PlantsVsZombies.Common.Players;
using PlantsVsZombies.Content.Projectiles.PlantSentries;
using Microsoft.Xna.Framework.Graphics;
using static PlantsVsZombies.Utilities.PlantUtils;

namespace PlantsVsZombies.Content.Items.Weapons.PlantSummons
{
    public class PeashooterPacket : ModItem
    {
        private int sunCost;
        private readonly int cooldown = 450;
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
            Item.shoot = ModContent.ProjectileType<Peashooter>();
            Item.noMelee = true;

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
            if (CheckCanUse(player, sunCost, PlantID.PeashooterPacket, cooldown))
            {
                return true;
            }
            else return false;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            DrawPlantCooldown(ref spriteBatch, ref position, scale, PlantID.PeashooterPacket, cooldown);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            SentrySpawningMethod(ref position, 42);
        }
    }
}
