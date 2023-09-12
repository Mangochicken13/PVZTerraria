using Microsoft.Xna.Framework;
using PlantsVsZombies.Common.Players;
using PlantsVsZombies.Content.Projectiles.PlantSentries;
using Terraria;
using Terraria.ModLoader;

namespace PlantsVsZombies.Content.Items.Weapons.PlantSummons
{
    public class BananaLauncherPacket : BasePlantPacket
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 864;
            Item.width = 18;
            Item.height = 22;
            Item.shoot = ModContent.ProjectileType<BananaLauncher>();

            SunCost = 700;
            Cooldown = 6000;
            ID = PlantID.BananaLauncher;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                return true;
            }
            if (CheckCanUse(player, SunCost, ID))
            {
                return true;
            }
            return false;
        }

        public override bool? UseItem(Player player)
        {

            if (player.whoAmI == Main.myPlayer)
            {
                if (player.altFunctionUse == 2)
                {
                    player.GetModPlayer<BananaTarget>().target = Main.MouseWorld;
                    return true;
                }
                else
                {
                    base.UseItem(player);
                    return true;
                }
            }
            return false;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                type = ModContent.ProjectileType<BananaTargetReticle>();
                position = Main.MouseWorld;
            }
            else
            {
                position = GroundedSentrySpawningMethod(68);
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
