using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace PlantsVsZombies.Content.Items.Weapons.PlantSummons
{

    public class RotobagaPacket : BasePlantPacket
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 40;
            Item.height = 40;
            Item.damage = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.PlantSentries.Rotobaga>();

            SunCost = 175;
            Cooldown = 1200;
            ID = PlantID.Rotobaga;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }
    }
}
