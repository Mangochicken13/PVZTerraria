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
    //TODO: Make animation frames for the projectile, sort out the animation
    //cont: also get sound effects for banana firing
    public class BananaLauncher : ModProjectile
    {
        public ref float AI_State => ref Projectile.ai[0];
        public ref float Shoot_Timer => ref Projectile.ai[1];
        private enum State
        {
            Wait,
            Shoot,
            Cooldown
        }

        internal Vector2 storedTarget;

        public override void SetStaticDefaults()
        {
            //Main.projFrames[Projectile.type] = 12;
        }

        public override void SetDefaults()
        {
            Projectile.height = 68;
            Projectile.width = 52;

            Projectile.tileCollide = true;
            Projectile.DamageType = ModContent.GetInstance<Plants>();
            Projectile.timeLeft = 18000;
            Projectile.netImportant = true;

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
            
            //if 1/4s have passed and the launcher isn't currently shooting reset the target
            if (Projectile.ai[2] >= 15 && AI_State != (float)State.Shoot) { storedTarget = Vector2.Zero; }

            //gravity
            Projectile.velocity.Y += 0.5f;
            if (Projectile.velocity.Y > 12) { Projectile.velocity.Y = 12f; }

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
        }

        public void Wait(Vector2 target)
        {
            if (target != Vector2.Zero)
            {
                AI_State = (float)State.Shoot; //increment state
            }
            //Projectile.frame = 0;
        }

        public void Shoot(Vector2 target)
        {
            Shoot_Timer++;
            //Projectile.frame = (int)Shoot_Timer / 2;
            if (Shoot_Timer >= 20)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, -14), ModContent.ProjectileType<BananaMissile>(), Projectile.damage, 5f, Main.myPlayer, 0, target.X, target.Y);
                //target = Vector2.Zero; //reset target variable after passing position in via the ai[1] & ai[2] parameters
                //Main.player[Projectile.owner].GetModPlayer<BananaTarget>().target = Vector2.Zero;
                AI_State = (float)State.Cooldown; //increment state
            }
        }

        public void Cooldown()
        {
            Shoot_Timer++;

            if (Shoot_Timer >= 1200)
            {
                Shoot_Timer = 0;
                AI_State = (float)State.Wait; //increment state 
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

    public class BananaMissile : ModProjectile
    {
        Vector2 target;
        public override void SetDefaults()
        {
            //Projectile.scale = 2f;
            Projectile.width = 20;
            Projectile.height = 68;
            Projectile.DamageType = ModContent.GetInstance<Plants>();
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 3600;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = -1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            target = new Vector2((int)Projectile.ai[1], (int)Projectile.ai[2]);
            Projectile.ai[1] = 0;
            Projectile.ai[2] = 0;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 180)
            {
                //Vector2 currentPos = Projectile.position;
                float distanceX = target.X - Projectile.position.X;
                int direction;

                //checking the direction
                if (distanceX > 0)
                {
                    direction = -1; //left
                }
                else if (distanceX < 0)
                {
                    direction = 1; //right
                }
                else { direction = 0; }

                float XDiff = (distanceX / 100) * direction;
                Projectile.position = target + new Vector2(XDiff, -(180 * 14));

                Vector2 velocity = 14f * Vector2.Normalize(target - Projectile.Center);
                Projectile.velocity = velocity; //new(velocity.X, 14f);
                Projectile.timeLeft = 189;
                Projectile.netUpdate = true;
            }

            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 9)
            {
                Projectile.velocity = new(0, 0);
                Projectile.alpha = 255;

                Projectile.friendly = true;
                Projectile.knockBack = 10f;

                Projectile.Resize(240, 240);

                Projectile.netUpdate = true;
            }
        }

        public override void Kill(int timeLeft)
        {
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

    public class BananaTargetReticle : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.timeLeft = 140;
            Projectile.height = 36;
            Projectile.width = 34;
        }

        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (i != Projectile.identity && Main.projectile[i].type == ModContent.ProjectileType<BananaTargetReticle>())
                {
                    Main.projectile[i].Kill();
                }
            }
            Projectile.scale = 2f;
        }

        public override void AI()
        {
            if (Projectile.ai[0] < 16)
            {
                Projectile.scale = 1 + (15 - Projectile.ai[0]) / 15;
            }
            Projectile.ai[0]++;

            if (Projectile.ai[0] == 25)
            {
                int p = 0;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type == ModContent.ProjectileType<BananaMissile>())
                    {
                        p++;
                    }
                }
                if (p == 0)
                {
                    Projectile.Kill();
                }
            }
        }
    }
}
