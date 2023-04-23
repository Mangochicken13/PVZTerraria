using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using Microsoft.Xna.Framework;
using PlantsVsZombies.Common.Players;

namespace PlantsVsZombies.Content.Items
{
    public class SunItemMedium : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sun");

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));

            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.height = 17;
            Item.width = 17;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.LightYellow.ToVector3() * 0.65f * Main.essScale);
        }
        public override bool ItemSpace(Player player)
        {
            return true;
        }
        public override bool OnPickup(Player player)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<Sun>();
            modPlayer.RegenAmount += 25;
            return false;
        }
    }

    public class SunItemSmall : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sun");

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));

            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.height = 12;
            Item.width = 12;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.LightYellow.ToVector3() * 0.45f * Main.essScale);
        }
        public override bool ItemSpace(Player player)
        {
            return true;
        }

        public override bool OnPickup(Player player)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<Sun>();
            modPlayer.RegenAmount += 15;
            return false;
        }
    }

    public class SunItemLarge : ModItem
    {
        public override string Texture => "PlantsVsZombies/Content/Items/SunItemSmall";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sun");

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));

            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.height = 12;
            Item.width = 12;
            Item.scale = 2.2f;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.LightYellow.ToVector3() * 0.85f * Main.essScale);
        }
        public override bool ItemSpace(Player player)
        {
            return true;
        }

        public override bool OnPickup(Player player)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<Sun>();
            modPlayer.RegenAmount += 50;
            return false;
        }
    }
}
