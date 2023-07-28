using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlantsVsZombies.Common.Players;
using PlantsVsZombies.Common.Systems;
using PlantsVsZombies.Content.Projectiles.PlantSentries;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static PlantsVsZombies.Utilities.PlantUtils;

namespace PlantsVsZombies.Content.Items.Weapons.PlantSummons
{
    public class SunflowerPacket : ModItem
    {
        private int sunCost;
        private readonly int cooldown = 600;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sunflower");
            // Tooltip.SetDefault("An incredibly happy flower\nThey seem to be able to produce fun size pockets of sun energy for your use");
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = false;
            Item.width = 24;
            Item.height = 36;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.DamageType = ModContent.GetInstance<Plants>();
            Item.shoot = ModContent.ProjectileType<Sunflower>();
            Item.noMelee = true;

            sunCost = 50;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AddSunCost(ref tooltips, Mod, sunCost);
        }
        public override bool CanUseItem(Player player)
        {
            if (CheckCanUse(player, sunCost, PlantID.SunflowerPacket, cooldown))
            {
                return true;
            }
            return false;
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            DrawPlantCooldown(ref spriteBatch, ref position, scale, PlantID.SunflowerPacket, cooldown);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            SentrySpawningMethod(ref position, 46);
        }
    }
}

//checking if the player has enough sun to use the item
//old method for CanUseItem(), updated in Utilities.cs
/**var Sun = player.GetModPlayer<Sun>();
var timer = player.GetModPlayer<PlantTimers>();
bool useTimer = timer.plantTimer["SunflowerPacket"] <= 0;

bool enoughSun = Sun.SunCurrent >= sunCost;
bool tileSolid = Main.tileSolid[Main.tile[Main.MouseWorld.ToTileCoordinates()].TileType];
bool tileSolidTop = Main.tileSolidTop[Main.tile[Main.MouseWorld.ToTileCoordinates()].TileType];
bool tile = Main.tile[Main.MouseWorld.ToTileCoordinates()].HasTile;

if (useTimer && enoughSun && (!tileSolid || tileSolidTop || !tile))
{
    Sun.SunCurrent -= sunCost;
    timer.plantTimer["SunflowerPacket"] = cooldown;
    return true;
}
else { return false; }*/


//old method for PostDrawInInventory(), updated in Utilities.cs
/**var timers = Main.LocalPlayer.GetModPlayer<PlantTimers>();

if (timers.plantTimer["SunflowerPacket"] <= 0)
{
    return;
}

Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("PlantsVsZombies/Assets/Ui/GreyOutOverlay");
float overlayScaleY = timers.plantTimer["SunflowerPacket"];
if (!(timers.plantTimer["SunflowerPacket"] <= 0))
{
    overlayScaleY += 0.1f;
    Math.Clamp(overlayScaleY, 0, 1);
    newScale = new (scale, scale * overlayScaleY/600); // (this line was changed later to do the /600 itself, the comment will remain) I don't know why this cant just be /600 earlier, but it just doesn't want to be for whatever reason
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
    layerDepth: 0f);*/


//Old method for ModifyShootStats(), replicated in Utilities.cs
/**
var potentialTile = Main.MouseWorld.ToTileCoordinates();

while (!Main.tileSolid[Main.tile[potentialTile].TileType] || !Main.tile[potentialTile].HasTile)
{
    potentialTile += new Point(0, 1);
}
position = potentialTile.ToWorldCoordinates() - new Vector2(0, 35);
if (Main.tile[potentialTile].IsHalfBlock) { position += new Vector2(0, 8); }
return;
**/
