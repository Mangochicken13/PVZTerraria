﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using PlantsVsZombies.Common.Systems;
using Microsoft.Xna.Framework.Graphics;
using PlantsVsZombies.Common.Players;
using System;

namespace PlantsVsZombies
{
    public class Utilities 
    {
        //this snippet is from dominoc925 on dominoc925.blogspot.com, edited to fit my use scenario
        //honestly, i don't exactly know how this does it, but i figured that it wasn't worth the time spent
        //to figure out how to do this myself, and it does what i wanted, so why bother making my own until it might
        //be necessary for optimisation purposes

        /// <summary>
        /// This a method for checking that a target is inside a desired attacking range/area
        /// </summary>
        /// <param name="polygon">Array of Vector2 points marking the boundaries of the targetting area</param>
        /// <param name="point">Position of the target</param>
        /// <returns></returns>
        public static bool IsPointInPolygon(Vector2[] polygon, Vector2 point)
        {
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

        public class PlantSpecificUtils
        {
            public static bool CheckCanUse(Player player, int sunCost, string item, int cooldown)
            {

                var Sun = player.GetModPlayer<Sun>();
                var Timer = player.GetModPlayer<PlantTimers>().plantTimer;
                bool useTimerActive = Timer[item] > 0;
                bool enoughSun = Sun.SunCurrent >= sunCost;
                if (!useTimerActive && enoughSun)
                {
                    Sun.SunCurrent -= sunCost;
                    Timer[item] = cooldown;
                    return true;
                }
                else { return false; }
            }
            public static void SentrySpawningMethod(ref Vector2 position, int projHitboxY)
            {
                var potentialTile = Main.MouseWorld.ToTileCoordinates();

                while (!Main.tileSolid[Main.tile[potentialTile].TileType] || !Main.tile[potentialTile].HasTile)
                {
                    potentialTile += new Point(0, 1);
                }
                position = potentialTile.ToWorldCoordinates() - new Vector2(0, (projHitboxY / 2) + 8);
                if (Main.tile[potentialTile].IsHalfBlock) { position += new Vector2(0, 8); }
            }

            /// <summary>
            /// This method is used to quickly draw the overlay to visually show the player how long until they can use an item again
            /// </summary>
            /// <param name="item">The item you want to draw over's class name, as a string</param>
            /// <param name="cooldownTime">The cooldown for the item, in ticks, as an int</param>
            public static void DrawPlantCooldown(ref SpriteBatch spriteBatch, ref Vector2 position, ref float scale, string item, int cooldownTime)
            {
                var timerDict = Main.LocalPlayer.GetModPlayer<PlantTimers>().plantTimer;
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("PlantsVsZombies/Assets/Ui/GreyOutOverlay");
                float overlayScaleY = timerDict[item];
                Vector2 newScale;

                if (timerDict[item] <= 0) { return; }

                if (!(timerDict[item] <= 0))
                {
                    overlayScaleY += 0.1f;
                    Math.Clamp(overlayScaleY, 0, 1);
                    newScale = new Vector2(scale, scale * overlayScaleY / cooldownTime);
                }
                else { newScale = new Vector2(scale); }

                spriteBatch.Draw(texture,
                    position: new Vector2(position.X, position.Y + (27 * scale)),
                    sourceRectangle: new Rectangle(0, 0, 54, 54),
                    color: new Color(15, 15, 15, 128),
                    rotation: 0f,
                    origin: new Vector2(27, 54),
                    scale: newScale,
                    SpriteEffects.None,
                    layerDepth: 0f);
            }
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
