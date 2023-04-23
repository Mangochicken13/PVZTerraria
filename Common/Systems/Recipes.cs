using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using PlantsVsZombies.Content.Items.Weapons.PlantSummons;

namespace PlantsVsZombies.Common.Systems
{
    internal class Recipes : ModSystem
    {
        public override void AddRecipes()
        {
            Recipe recipe;

            //adding recipes for the mod's items, so they are obtainable naturally
            recipe = Recipe.Create(ModContent.ItemType<PeashooterPacket>());
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddIngredient(ItemID.Daybloom, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();

            recipe = Recipe.Create(ModContent.ItemType<SunflowerPacket>());
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddIngredient(ItemID.Daybloom, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
