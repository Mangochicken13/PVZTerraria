//all these "using" are necessary for the code to function, as these files extend classes made by the tModLoader team
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using PlantsVsZombies.Content.Items;

namespace PlantsVsZombies.Content.Projectiles
{
    internal class Sunflower : ModProjectile
    {
        //initially setting the time for sun production, at 15 seconds (60 ticks per second)
        float sunProductionTime = 900;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sunflower"); -this method has become outdated with the latest update to tModLoader, in favour of a localisation file
            
            //Setting the amount of frames for the projectile, tells the game where to divide up the image for the projectile
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            //Setting all the stats for this projectile, including hitbox size, where the sprite draws, collision
            //damage hitboxes, and type of projectile
            Projectile.height = 54;
            Projectile.width = 34;
            DrawOffsetX = -5;

            Projectile.tileCollide = true;

            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = 7200;

            Projectile.minion = true;
            Projectile.sentry = true;
            Projectile.minionSlots = -1;
        }

        public override void AI()
        {
            //Adding gravity to the projectile, so that it falls, and stops at a set velocity
            Projectile.velocity.Y += 0.5f;
            if (Projectile.velocity.Y > 12) { Projectile.velocity.Y = 12f; }

            //Incrementing an iterator, used for anything to do with timing
            Projectile.ai[0]++;

            //What happens when the iterable reaches a preset limit
            if (Projectile.ai[0] >= sunProductionTime)
            {
                //randomly changing the limit for the next cycle, at a range of 14-16 seconds
                sunProductionTime = Main.rand.Next(850, 951);

                //resetting the iterable
                Projectile.ai[0] = 0;

                if (Main.netMode != NetmodeID.MultiplayerClient) //making sure that this doesn't run on a multiplayer client, which would cause unintended duplication
                {
                    //spawning the "sun" pickup, which increases the player's sun by 25
                    Item.NewItem(null, Projectile.position, ModContent.ItemType<SunItemMedium>(), 1, false, 0, true, false);
                }
            }
            //Animating the projectile
            Visuals();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.Y = 0f; //Making sure the projectile stops falling when it hits a tile
            return false; //returning false here makes the vanilla behavior not run, as that would kill the projectile
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            //makes sure that the projectile lands on platforms
            fallThrough = false;
            return true;
        }
        private void Visuals() //The code for animating the projectile
        {
            //setting how fast the frames cycle
            int frameSpeed = 16;

            //iterating the internal variable for this purpose
            Projectile.frameCounter++;

            //if the frame counter is greater or equal to the frame speed (16), comes out to slightly less than 4 frames per second
            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                //cycling the counter once reaching the last frame
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}
