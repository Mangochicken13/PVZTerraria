using Terraria;
using Terraria.ModLoader;
using Terraria.ID;


namespace PlantsVsZombies.Content.Projectiles.PlantSentries
{
    public class Rotobaga : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.height = 36;
            Projectile.width = 36;
            Projectile.friendly = true;
            Projectile.netImportant = true;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -2;
        }
    }
}
