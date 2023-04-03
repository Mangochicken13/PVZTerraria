using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using PlantsVsZombies.Common.Systems;
using Terraria.Utilities;

namespace PlantsVsZombies.Content.Projectiles
{
    internal class Peashooter : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Peashooter");

            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 40;
            DrawOffsetX = -5;

            Projectile.tileCollide = true;

            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.sentry = true;
            Projectile.minion = true;
            Projectile.minionSlots = 0;
            Projectile.DamageType = ModContent.GetInstance<Plants>();
            Projectile.penetrate = -1;
            Projectile.timeLeft = 7200;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.5f;
            if (Projectile.velocity.Y > 10) { Projectile.velocity.Y = 10f; }

            bool foundTarget = false;
            Vector2 targetCenter = Projectile.position;
            float attackRange = 1000f;
            NPC target = null;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC potentialTarget = Main.npc[i];

                if (potentialTarget.CanBeChasedBy())
                {
                    float distanceBetween = Vector2.Distance(potentialTarget.Center, Projectile.Center);
                    bool closest = Vector2.Distance(Projectile.Center, targetCenter) > distanceBetween;
                    bool inRange = distanceBetween < attackRange;
                    bool inRangeY = Math.Abs(Projectile.Center.Y - potentialTarget.Center.Y) < 120;
                    bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, potentialTarget.Center, potentialTarget.width, potentialTarget.height);

                    if (((closest && inRange) || !foundTarget) && lineOfSight && inRangeY)
                    {
                        foundTarget = true;
                        targetCenter = potentialTarget.Center;
                        target = potentialTarget;
                    }
                }
            }

            Projectile.ai[0]++;

            if (foundTarget && Projectile.ai[0] >= 100)
            {
                Projectile.ai[0] = 0;

                if ((Projectile.Center - target.Center).X > 0)
                {
                    Projectile.spriteDirection = -1;
                }
                else if ((Projectile.Center - target.Center).X < 0)
                {
                    Projectile.spriteDirection = 1;
                }

                Vector2 velocity = target.Center - Projectile.Center;
                velocity.Normalize();
                Vector2 spawnlocation = new(Projectile.spriteDirection * 12, -8);

                Projectile.NewProjectile(Projectile.GetSource_FromAI(), (Projectile.Center + spawnlocation), velocity * 7f, ModContent.ProjectileType<Pea>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }

            Visuals();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.Y = 0f;
            return false;
        }

        private void Visuals()
        {
            int frameSpeed = 12;

            Projectile.frameCounter++;

            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }
        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return false;
        }
    }
}
