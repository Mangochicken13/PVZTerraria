using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlantsVsZombies.Common.Players;
using PlantsVsZombies.Common.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

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

        public class PlantUtils
        {
            public static bool CheckCanUse(Player player, int sunCost, int item, int cooldown)
            {
                var Sun = player.GetModPlayer<Sun>();
                var Timer = player.GetModPlayer<PlantTimers>()._plantTimers;
                bool useTimerActive = Timer[item] > 0;
                bool enoughSun = Sun.SunCurrent >= sunCost;

                if (!(!Main.tileSolid[Main.tile[Main.MouseWorld.ToTileCoordinates()].TileType] || Main.tileSolidTop[Main.tile[Main.MouseWorld.ToTileCoordinates()].TileType] || !Main.tile[Main.MouseWorld.ToTileCoordinates()].HasTile))
                {
                    return false;
                }


                if (!useTimerActive && enoughSun)
                {
                    Sun.SunCurrent -= sunCost;
                    Timer[item] = cooldown;
                    return true;
                }
                else { return false; }
            }

            public static void AddSunCost(ref List<TooltipLine> tooltips, Mod mod, int sunCost)
            {
                tooltips.Add(new TooltipLine(mod, "Sun Cost", $"Uses {sunCost} Sun"));
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
            /// Note to self: try and find a way to mask the overlay to only draw over the inventory slot
            /// </summary>
            /// <param name="item">The item you want to draw over's class name, as a string</param>
            /// <param name="cooldownTime">The cooldown for the item, in ticks, as an int</param>
            public static void DrawPlantCooldown(ref SpriteBatch spriteBatch, ref Vector2 position, float scale, int item, int cooldownTime)
            {
                var timers = Main.LocalPlayer.GetModPlayer<PlantTimers>()._plantTimers;
                if (timers[item] <= 0) { return; }

                scale = Main.inventoryScale;

                //Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("PlantsVsZombies/Assets/Ui/GreyOutOverlay");
                Texture2D texture = (Texture2D)TextureAssets.InventoryBack;
                
                float overlayScaleY = timers[item];
                Vector2 newScale;

                if (timers[item] >= 0)
                {
                    overlayScaleY += 0.1f;
                    Math.Clamp(overlayScaleY, 0, 1);
                    newScale = new Vector2(scale, scale * overlayScaleY / cooldownTime);
                }
                else { newScale = new Vector2(scale); }

                spriteBatch.Draw(texture,
                    position: new Vector2(position.X, position.Y + (25 * scale)),
                    sourceRectangle: new Rectangle(0, 0, 50, 50),
                    color: new Color(15, 15, 15, 128),
                    rotation: 0f,
                    origin: new Vector2(25, 50),
                    scale: newScale,
                    SpriteEffects.None,
                    layerDepth: 0f);
            }

            public static NPC TargetClosestInArea(Projectile projectile, Vector2[] targettingArea, NPC target = null)
            {
                bool foundTarget = false;
                Vector2 targetCenter = projectile.position;
                Vector2 pCen = projectile.Center;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC potentialTarget = Main.npc[i];

                    bool validPosition = IsPointInPolygon(targettingArea, potentialTarget.Center);

                    if (potentialTarget.CanBeChasedBy() && validPosition)
                    {
                        float distanceBetween = Vector2.Distance(pCen, potentialTarget.Center);
                        bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height,
                                potentialTarget.position, potentialTarget.width, potentialTarget.height);
                        bool closest = Vector2.Distance(projectile.Center, targetCenter) > distanceBetween;

                        if ((closest || !foundTarget) && lineOfSight)
                        {
                            foundTarget = true;
                            target = potentialTarget;
                            targetCenter = potentialTarget.Center;
                        }
                    }
                }

                return target;
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
