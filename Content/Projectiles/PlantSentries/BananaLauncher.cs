using Microsoft.Xna.Framework;
using PlantsVsZombies.Common.Players;
using PlantsVsZombies.Common.Systems;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlantsVsZombies.Content.Projectiles.PlantSentries
{
    // TODO: Make animation frames for the projectile, sort out the animation in code
    // cont: also get sound effects for banana firing
    public class BananaLauncher : ModProjectile
    {
        public ref float AI_State => ref Projectile.ai[0];
        public ref float Shoot_Timer => ref Projectile.ai[1];
        private enum State
        {
            Wait,
            Shoot,
            Cooldown,
            Reload
        }

        internal Vector2 storedTarget;

        public override void SetStaticDefaults()
        {
            // Main.projFrames[Projectile.type] = 12; //Setting the amount of frames, unused due to not having said frames
        }

        public override void SetDefaults()
        {
            Projectile.height = 68;
            Projectile.width = 52;

            Projectile.tileCollide = true;
            Projectile.DamageType = ModContent.GetInstance<PlantDamage>();
            Projectile.timeLeft = 18000; // 5 minutes lifespan, longer than others due to its cooldown and heavy nature.
            Projectile.netImportant = true; // Sync to clients when they join a server with one of these projectiles present

        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            ref Vector2 target = ref player.GetModPlayer<BananaTarget>().target;

            if (target != Vector2.Zero)
            {
                storedTarget = target;
                Projectile.ai[2] = 0;
            }

            if (Projectile.ai[2] < 16)
            {
                Projectile.ai[2]++;
            }
            
            // If 1/4s have passed and the launcher isn't currently shooting reset the target
            if (Projectile.ai[2] >= 15 && AI_State != (float)State.Shoot) { storedTarget = Vector2.Zero; }


            if (Projectile.owner == Main.myPlayer)
            {
                switch (AI_State)
                {
                    case (float)State.Wait:
                        Wait(storedTarget);
                        break;
                    case (float)State.Shoot:
                        Shoot(storedTarget);
                        break;
                    case (float)State.Cooldown:
                        Cooldown();
                        break;
                }
            }

            //gravity
            Projectile.velocity.Y += 0.5f;
            if (Projectile.velocity.Y > 12) { Projectile.velocity.Y = 12f; }
        }

        public void Wait(Vector2 storedTarget)
        {
            if (storedTarget != Vector2.Zero)
            {
                AI_State = (float)State.Shoot; //increment state
            }
            //Projectile.frame = 0;
        }

        public void Shoot(Vector2 storedTarget)
        {
            Shoot_Timer++;
            //Projectile.frame = (int)Shoot_Timer / 2;
            if (Shoot_Timer >= 20)
            {
                //spawn the missile, initial velocity of 42f upwards, due to it running 3 ai() calls per frame
                //Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, -14), ModContent.ProjectileType<BananaMissile>(), Projectile.damage, 5f, Main.myPlayer, 0, storedTarget.X, storedTarget.Y);
                
                //target = Vector2.Zero; //reset target variable after passing position in via the ai[1] & ai[2] parameters
                //Main.player[Projectile.owner].GetModPlayer<BananaTarget>().target = Vector2.Zero;
                
                AI_State = (float)State.Cooldown; // Increment state
            }
        }

        public void Cooldown()
        {
            if (Shoot_Timer == 21) { Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, -14), ModContent.ProjectileType<BananaMissile>(), Projectile.damage, 5f, Main.myPlayer, 0, storedTarget.X, storedTarget.Y); }
            Shoot_Timer++;

            if (Shoot_Timer >= 1200)
            {
                Shoot_Timer = 0;
                AI_State = (float)State.Wait; // Increment state 
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.Y = 0;
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }
    }

    // Explosive projectile, the one that does damage
    public class BananaMissile : ModProjectile
    {
        // TODO: make a better sprite for this at some point
        internal Vector2 target;
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 68;
            Projectile.DamageType = ModContent.GetInstance<PlantDamage>();
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 3600;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = -1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            // This projectile gets created with a target in its ai[1] and ai[2] fields, which gets passed into an internal variable
            target = new Vector2((int)Projectile.ai[1], (int)Projectile.ai[2]);

            // Iterate through projectiles, if 'i' is the targetting reticle and intersects with the target of the missile set its ai[1] to 1
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].Hitbox.Intersects(new Rectangle((int)Projectile.ai[1], (int)Projectile.ai[2], 1, 1)) && Main.projectile[i].type == ModContent.ProjectileType<BananaTargetReticle>())
                {
                    Main.projectile[i].ai[1] = 1;
                }
            }
            Projectile.ai[1] = 0;
            Projectile.ai[2] = 0;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Projectile.ai[0] == 180)
                {
                    /*
                    //Vector2 currentPos = Projectile.position;
                    float distanceX = target.X - Projectile.position.X;
                    int direction;

                    //checking the direction, maybe remove this in favour of a random offset

                    
                    if (distanceX > 0) { direction = -1; } //left
                    else if (distanceX < 0) { direction = 1; } //right
                    else { direction = 0; }

                    float XDiff = (distanceX / 100) * direction;
                    */
                    Projectile.position = target + new Vector2(Main.rand.Next(-160, 161), -(180 * 14));

                    Vector2 velocity = 14f * Vector2.Normalize(target - Projectile.Center);
                    Projectile.velocity = velocity; //new(velocity.X, 14f);
                    Projectile.timeLeft = 189;
                    Projectile.netUpdate = true;
                }
            }

            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 9)
            {
                Projectile.velocity = new(0, 0);
                Projectile.alpha = 255;

                Projectile.friendly = true;
                Projectile.knockBack = 10f;

                Projectile.Resize(240, 240); // NOTE TO SELF: Potentially change the magic number 240 to a variable: the hitbox is 15x15 tiles; 7.5 tiles from the center

                Projectile.netUpdate = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            // Copied from example mod's explosion code, slightly 


            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            // Smoke Dust spawn
            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
                dust.velocity *= 1.4f;
            }

            for (int g = 0; g < 2; g++)
            {
                var goreSpawnPosition = new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f);
                Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
                gore.scale = 1.5f;
                gore.velocity.X += 1.5f;
                gore.velocity.Y += 1.5f;
                gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
                gore.scale = 1.5f;
                gore.velocity.X -= 1.5f;
                gore.velocity.Y += 1.5f;
                gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
                gore.scale = 1.5f;
                gore.velocity.X += 1.5f;
                gore.velocity.Y -= 1.5f;
                gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
                gore.scale = 1.5f;
                gore.velocity.X -= 1.5f;
                gore.velocity.Y -= 1.5f;
            }
        }
    } 

    // Shows the player where the target is
    public class BananaTargetReticle : ModProjectile
    {
        internal bool isAimedAt;
        public override void SetDefaults()
        {
            Projectile.timeLeft = 137;
            Projectile.height = 36;
            Projectile.width = 34;
        }

        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                // TODO: Try to check if any of the targets are where a missile is headed, and not kill those ones
                if (i != Projectile.identity && Main.projectile[i].type == ModContent.ProjectileType<BananaTargetReticle>())
                {
                    if (Main.projectile[i].ai[1] != 1 && Projectile.ai[0] >= 25)
                    {
                        Main.projectile[i].Kill();
                    }
                }
            }
            Projectile.scale = 2f;
        }

        public override void AI()
        {
            if (Projectile.ai[0] < 26)
            {
                Projectile.scale = 1 + (25 - Projectile.ai[0]) / 25;
            }
            Projectile.ai[0]++;

            if (Projectile.ai[0] == 25)
            {
                // Trying to make the target despawn if no missiles have fired
                int p = 0;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type == ModContent.ProjectileType<BananaMissile>() && Main.projectile[i].active == true)
                    {
                        p++;
                    }
                }
                if (p == 0 || Projectile.ai[1] != 1)
                {
                    Projectile.Kill();
                }
            }
        }
    } 
}
