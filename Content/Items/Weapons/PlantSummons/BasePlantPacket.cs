using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlantsVsZombies.Common.Players;
using PlantsVsZombies.Common.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PlantsVsZombies.Content.Items.Weapons.PlantSummons
{
    public abstract class BasePlantPacket : ModItem
    {
        public float SunCost;
        public int Cooldown;
        public short ID;

        #region Overrides
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useAnimation = 15;

            Item.DamageType = ModContent.GetInstance<PlantDamage>();
            Item.noMelee = true;

            Cooldown = 60;
            SunCost = 10;
            ID = 0;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(line => line.Name == "Tooltip0");
            if (index >= 0)
            {
                tooltips.Insert(index, new TooltipLine(Mod, "SunCostText", Language.GetTextValue(Mod.GetLocalizationKey("CommonItemTooltip.SunCostText"), GetTrueSunCost(SunCost))));
                tooltips.Insert(index + 1, new TooltipLine(Mod, "CooldownText", Language.GetTextValue(Mod.GetLocalizationKey("CommonItemTooltip.CooldownText"), (Cooldown / 60f).ToString())));
            }

            index = tooltips.FindIndex(line => line.Mod == "Terraria" && line.Name == "Speed");
            if (index != -1)
            {
                tooltips.RemoveAt(index);
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (CheckCanUse(player, (int)SunCost, ID))
            {
                return true;
            }
            return false;
        }
        public override bool? UseItem(Player player)
        {
            UseItemConsumeSun(player, SunCost, ID, Cooldown);

            return true;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            DrawPlantCooldown(ref spriteBatch, position, ID, Cooldown);
        }

        #endregion Overrides

        #region Get true sun cost
        public static int GetTrueSunCost(float sunCost)
        {
            var sun = Main.LocalPlayer.GetModPlayer<Sun>();
            sunCost *= sun.DecreasedCostMult;
            sunCost -= sun.DecreasedCostAdd;
            sunCost = MathHelper.Clamp(sunCost, 0, 5E4f);

            sunCost = (int)Math.Round(sunCost / 5.0) * 5;

            return (int)sunCost;
        }

        #endregion

        #region Use Item
        public static void UseItemConsumeSun(Player player, float sunCost, short plantID, int cooldown)
        {
            var sun = player.GetModPlayer<Sun>();
            var timers = player.GetModPlayer<PlantTimers>().plantTimers;

            sunCost = GetTrueSunCost(sunCost);

            sun.CurrentSun -= (int)sunCost; // Cast here is theoretically unnecessary due to sunCost being returned as an int earlier, but the computer doesn't know that
            
            timers[plantID] = cooldown;
        }
        #endregion

        #region DrawCooldown
        /// <summary>
        /// This method is used to quickly draw the overlay to visually show the player how long until they can use an item again
        /// Note To Self: try and find a way to mask the overlay to only draw over the inventory slot
        /// </summary>
        /// <param name="item">The item you want to draw over's internal ID</param>
        /// <param name="cooldown">The cooldown for the item, in ticks, as an int</param>
        public static void DrawPlantCooldown(ref SpriteBatch spriteBatch, Vector2 position, int item, int cooldown)
        {
            var timers = Main.LocalPlayer.GetModPlayer<PlantTimers>().plantTimers;

            if (timers[item] <= 0) { return; } // Stop the method if the timer is less than or equal to 0

            float scale = Main.inventoryScale;

            // Get the texture used for the back of the inventory sprites
            var texture = (Texture2D)TextureAssets.InventoryBack;

            float overlayScaleY = timers[item] / (float)cooldown;

            var newScale = new Vector2(scale, scale * overlayScaleY);

            spriteBatch.Draw(texture,
                position: new Vector2(position.X, position.Y + (25 * scale)),
                sourceRectangle: new Rectangle(0, 0, 50, 50),
                color: new Color(15, 15, 15, 128), // The color here is a grey-ish one, made transparent by the 128 alpha component
                rotation: 0f,
                origin: new Vector2(25, 50), // Origin gets set to the bottom middle of the inventory slot, so the Y scaling works properly
                scale: newScale,
                SpriteEffects.None,
                layerDepth: 0f);
        }
        #endregion

        #region SentrySpawning
        /// <summary>
        /// Use this to spawn plant "sentries" that should be grounded.
        /// Changed return type to Vector2 to be more descriptive
        /// </summary>
        /// <param name="projHitboxY">Height of the projectile's hitbox</param>
        public static Vector2 GroundedSentrySpawningMethod(int projHitboxY)
        {
            var potentialTile = Main.MouseWorld.ToTileCoordinates();

            // Iterate vertically through the 'tile' array, until hitting a tile
            while (!Main.tileSolid[Main.tile[potentialTile].TileType] || !Main.tile[potentialTile].HasTile)
            {
                potentialTile += new Point(0, 1);
            }

            // Converts the tile coordinates back to world coordinates, adds 8 to bring the projectile to the top of the tile,
            // then ads half of its hitbox height so it spawns directly on the ground
            var position = potentialTile.ToWorldCoordinates() - new Vector2(0, (projHitboxY / 2) + 8);
            position.X = Main.MouseWorld.X; // Makes the X coords the same as where the item was used

            // Decreasse height by half a tile if the final tile is a half block, makes it spawn without falling
            if (Main.tile[potentialTile].IsHalfBlock) { position += new Vector2(0, 8); }
            return position;
        }
        #endregion

        #region Check Can Use
        public static bool CheckCanUse(Player player, float sunCost, int item)
        {
            var sun = player.GetModPlayer<Sun>();
            var timers = player.GetModPlayer<PlantTimers>().plantTimers;
            bool useTimerActive = timers[item] > 0;

            sunCost = GetTrueSunCost(sunCost);
            bool enoughSun = sun.CurrentSun >= sunCost; //

            var mousePos = Main.MouseWorld.ToTileCoordinates();
     
            if (!(!Main.tileSolid[Main.tile[mousePos].TileType] || Main.tileSolidTop[Main.tile[Main.MouseWorld.ToTileCoordinates()].TileType] || !Main.tile[Main.MouseWorld.ToTileCoordinates()].HasTile))
            {
                return false;
            }

            // Check the plant is off cooldown, and the user has enough sun
            if (!useTimerActive && enoughSun)
            {
                return true;
            }

            else { return false; }
        }
        #endregion

        #region Target Closest In Area
        /// <summary>
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="targettingArea">An array of Vector2 points, defining the area. Expected in world coordinate format</param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static NPC TargetClosestInArea(Projectile projectile, Vector2[] targettingArea, NPC target = null)
        {
            bool foundTarget = false;
            var targetCenter = projectile.position;
            var pCen = projectile.Center;

            // Did this ages ago, can't remember how specifically it works but I'm not going to break it by touching
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                var potentialTarget = Main.npc[i];

                bool validPosition = Utilities.IsPointInPolygon(targettingArea, potentialTarget.Center);

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
        #endregion
    }
}
