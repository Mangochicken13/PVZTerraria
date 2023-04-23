using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using PlantsVsZombies.Common.Systems;

namespace PlantsVsZombies.Content.Projectiles
{
    internal class Pea : ModProjectile
    {
        //this is a simple projectile: travels in a straight line for a certain amount of time, and hits one enemy
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pea");
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<Plants>();
            Projectile.penetrate = 1;
            Projectile.timeLeft = 1200;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0 )
            {
                Projectile.Kill();
            }
            return false;
        }
    }
}
