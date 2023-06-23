using Microsoft.Xna.Framework;
using PlantsVsZombies.Common.Systems;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PlantsVsZombies.Content.Projectiles.PlantSentries
{
    public class Rotobaga : ModProjectile
    {
        private int attackCooldown = Main.rand.Next(38, 42);
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.height = 30;
            Projectile.width = 30;
            Projectile.damage = 8;
            Projectile.netImportant = true;
            Projectile.DamageType = ModContent.GetInstance<Plants>();
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }
        public override bool PreAI()
        {
            return base.PreAI();
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            Projectile.ai[1]++;

            Projectile.Center = new Vector2(Projectile.Center.X, Projectile.Center.Y + MathF.Sin(Projectile.ai[0] / 150) * 0.07f);
            Vector2 pPos = Projectile.Center;
            Vector2[] targettingArea = {
                pPos + 16 * new Vector2(4, 8),
                pPos + 16 * new Vector2(25, 31),
                pPos + 16 * new Vector2(31, 25),
                pPos + 16 * new Vector2(8, 4),
                pPos + 16 * new Vector2(8, -4),
                pPos + 16 * new Vector2(31, -25),
                pPos + 16 * new Vector2(25, -31),
                pPos + 16 * new Vector2(4, -8),
                pPos + 16 * new Vector2(-4, -8),
                pPos + 16 * new Vector2(-25, -31),
                pPos + 16 * new Vector2(-31, -25),
                pPos + 16 * new Vector2(-8, -4),
                pPos + 16 * new Vector2(-8, 4),
                pPos + 16 * new Vector2(-31, 25),
                pPos + 16 * new Vector2(-25, 31),
                pPos + 16 * new Vector2(-4, 8),
                pPos + 16 * new Vector2(4, 8)};

            if (Projectile.ai[1] >= attackCooldown)
            {
                NPC target = Utilities.PlantUtils.TargetClosestInArea(Projectile, targettingArea, null);

                if (target != null)
                {
                    Projectile.ai[1] = 0;
                    Projectile.ai[2] = 1;

                    attackCooldown = Main.rand.Next(56, 65);

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            Vector2 velocity = new Vector2(5.8f, 0).RotatedBy((Math.Tau * Main.rand.NextFloat(-0.01f, 0.01f)) + (Math.Tau / 8) + i * Math.Tau / 4);
                            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<RotobagaShot>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
                        }
                    }
                }
            }

            if (Projectile.ai[2] != 0)
            {
                Projectile.ai[2]++;

                if ((Projectile.ai[2] - 1) % 5 == 0 && (Projectile.ai[2] - 1) != 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            Vector2 velocity = new Vector2(5.8f, 0).RotatedBy((Math.Tau * Main.rand.NextFloat(-0.01f, 0.01f)) + (Math.Tau / 8) + i * Math.Tau / 4);
                            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<RotobagaShot>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
                        }
                    }

                    if (Projectile.ai[2] > 11)
                    {
                        Projectile.ai[2] = 0;
                    }
                }
            }
        }
        public void Animate()
        {

        }
    }

    public class RotobagaShot : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<Plants>();
            Projectile.penetrate = 2;
            Projectile.timeLeft = 1200;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0) { Projectile.alpha -= 15; }
            if (Projectile.alpha < 0) { Projectile.alpha = 0; }
        }
    }
}
