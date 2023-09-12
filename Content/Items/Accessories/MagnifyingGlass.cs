using PlantsVsZombies.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlantsVsZombies.Content.Items.Accessories
{
    public class MagnifyingGlass : ModItem
    {
        public int ResourceDecrease = 5;
        public float ResourceDecreasePercentage = 10;

        public override string Texture => "Terraria/Images/Item_" + ItemID.MagicMirror;

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.buyPrice(gold: 1, silver: 50);
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var sun = player.GetModPlayer<Sun>();
            sun.DecreasedCostAdd += ResourceDecrease;
            sun.DecreasedCostMult *= (1f - (1 / ResourceDecreasePercentage));
        }
    }
}
