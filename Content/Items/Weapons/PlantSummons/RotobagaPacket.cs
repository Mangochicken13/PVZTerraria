using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static PlantsVsZombies.Utilities.PlantUtils;

namespace PlantsVsZombies.Content.Items.Weapons.PlantSummons
{

    public class RotobagaPacket : ModItem
    {
        int sunCost;
        int cooldown = 1200;
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
        public override bool CanUseItem(Player player)
        {
            if (CheckCanUse(player, sunCost, PlantID.RotobagaPacket, cooldown))
            {
                return true;
            }
            return false;
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            DrawPlantCooldown(ref spriteBatch, ref position, PlantID.RotobagaPacket, cooldown);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }
    }
}
