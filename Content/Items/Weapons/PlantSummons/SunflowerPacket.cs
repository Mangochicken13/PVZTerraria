using Microsoft.Xna.Framework;
using PlantsVsZombies.Content.Projectiles.PlantSentries;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlantsVsZombies.Content.Items.Weapons.PlantSummons
{
    public class SunflowerPacket : BasePlantPacket
    {
        public override void SetDefaults()
        {
            // All other important variables are set in the base class
            // As such, base.SetDefaults(); is incredibly important to keep those variable changes.
            base.SetDefaults();

            Item.width = 24;
            Item.height = 36;
            Item.shoot = ModContent.ProjectileType<Sunflower>();

            // These are the custom fields created in the base class, used for all the basic functions
            SunCost = 50;
            Cooldown = 600;
            ID = PlantID.SunflowerPacket;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = GroundedSentrySpawningMethod(46);
        }

        public override void AddRecipes()
        {
            CreateRecipe() //sunflower version
                .AddIngredient(ItemID.DirtBlock, 8)
                .AddIngredient(ItemID.Sunflower, 1)
                .AddTile(TileID.WorkBenches)
                .Register();

            CreateRecipe() //daybloom version
                .AddIngredient(ItemID.DirtBlock, 10)
                .AddIngredient(ItemID.Daybloom, 2)
                .AddTile(TileID.WorkBenches)
                .Register();

            CreateRecipe() //daybloom seeds version
                .AddIngredient(ItemID.DirtBlock, 15)
                .AddIngredient(ItemID.DaybloomSeeds, 4)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}