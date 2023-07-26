using Microsoft.Xna.Framework;
using PlantsVsZombies.Common.Players;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlantsVsZombies.Content.Items
{
    //these three items are pickups that increase your sun total, by 25, 15, and 50 respectively (medium, small, large)
    //all their classes are very similar, so only SunItemMedium is fully commented
    public class SunItemMedium : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sun");

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));

            //makes the item animate when dropped in the world
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            //sets the item's hitbox
            Item.height = 17;
            Item.width = 17;
        }

        public override void PostUpdate()
        {
            //makes the items emit a bit of light, since they are sun after all
            Lighting.AddLight(Item.Center, Color.LightYellow.ToVector3() * 0.65f * Main.essScale);
        }
        public override bool ItemSpace(Player player)
        {
            //lets you pick up the item even if you have a full inventory
            return true;
        }
        public override bool OnPickup(Player player)
        {
            //this code makes the item add to your sun, then delete itself, as it is never supposed to be available in the inventory
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
