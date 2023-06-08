using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

using PlantsVsZombies.Common.Players;
using PlantsVsZombies.Common.Systems;

namespace PlantsVsZombies
{
    public class Utilities 
    {
        public static bool IsPointInPolygon(Vector2[] polygon, Vector2 point)
        {   //this snippet is from dominoc925 on dominoc925.blogspot.com, edited to fit my use scenario
            //honestly, i don't exactly know how this does it, but i figured that it wasn't worth the time spent
            //to figure out how to do this myself, and it does what i wanted, so why bother making my own until it might
            //be necessary for optimisation purposes
            bool isInside = false;

            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)

            {
                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&

                (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))

                {

                    isInside = !isInside;

                }
            }

            return isInside;
        }

        public static void SentrySpawningMethod(ref Vector2 position, Vector2 mouseLocation, int spriteSize)
        {
            var potentialTile = mouseLocation.ToTileCoordinates();

            while (!Main.tileSolid[Main.tile[potentialTile].TileType] || !Main.tile[potentialTile].HasTile)
            {
                potentialTile += new Point(0, 1);
            }
            position = potentialTile.ToWorldCoordinates() - new Vector2(0, (spriteSize/2) + 8);
            if (Main.tile[potentialTile].IsHalfBlock) { position += new Vector2(0, 8); }
        }
    }


    //This whole class is a bit unreadable, but it's designed to make carbon copy items faster to make
    //It's a bit limited in this implementation, so likely won't be used, but I'll copy this into my modding tools mod
    //Original concept came from the Starlight River, adapted code from GabeHasWon's Verdant Mod
    public class QuickItem
    {
        public static void SetBase(ModItem item, int width, int height, int useStyle = ItemUseStyleID.Swing, bool consumable = true)
        {
            item.Item.width = width;
            item.Item.height = height;
            item.Item.useStyle = useStyle;
            item.Item.consumable = consumable;
        }
        /// <summary>
        /// A quick way to set some of the basic stats for my plant summons
        /// </summary>
        /// <param name="item">The item to modify the stats of. Use "this" to select the parent class</param>
        public static void SetPlantSummon(ModItem item, int width, int height, int damage, float knockback, int useTime, int useAnimation)
        {
            item.Item.width = width;
            item.Item.height = height;
            item.Item.DamageType = ModContent.GetInstance<Plants>();
            item.Item.damage = damage;
            item.Item.knockBack = knockback;
            item.Item.useStyle = ItemUseStyleID.Swing;
            item.Item.consumable = false;
            item.Item.useTime = useTime;
            item.Item.useAnimation = useAnimation;
            item.Item.noMelee = true;
            
        }
    }
}
