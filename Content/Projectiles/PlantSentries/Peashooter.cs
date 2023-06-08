using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using PlantsVsZombies.Common.Systems;
using static PlantsVsZombies.Utilities;
using static Terraria.ModLoader.PlayerDrawLayer;

//See Content/Projectiles/Sunflower for more detailed explanations of common functions

namespace PlantsVsZombies.Content.Projectiles.PlantSentries
{
    internal class Peashooter : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Peashooter"); -deprecated method, replaced by en-US.hjson

            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 42;
            DrawOffsetX = -5;

            Projectile.tileCollide = true;

            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.DamageType = ModContent.GetInstance<Plants>();
            Projectile.penetrate = -1;
            Projectile.timeLeft = 7200;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {

            //gravity
            Projectile.velocity.Y += 0.5f;
            if (Projectile.velocity.Y > 12) { Projectile.velocity.Y = 12f; }

            //variables used to target enemies, and their default values
            bool foundTarget = false;
            Vector2 targetCenter = Projectile.position;
            NPC target = null;

            //creating a polygon of Vector2 points, where the peashooter should target enemies in
            //variable ppos is simply to shorten each line, as Projectile.Center is a relatively long term to type out
            Vector2 ppos = Projectile.Center;
            Vector2[] targettingArea = {
                    ppos + new Vector2(0, 4),
                    ppos + new Vector2(160, 64),
                    ppos + new Vector2(960, 64),
                    ppos + new Vector2(960, -64),
                    ppos + new Vector2(160, -64),
                    ppos + new Vector2(0, -4),
                    ppos + new Vector2(-160, -64),
                    ppos + new Vector2(-960, -64),
                    ppos + new Vector2(-960, 64),
                    ppos + new Vector2(-160, 64)
                };

            //targetting function | Main.maxNPCs is the total available NPC slots, so this for loop cycles through every possible NPC
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                //selects the current npc in the loop for further inspection, referred to as "target" in future comments
                NPC potentialTarget = Main.npc[i];

                //checks if the npc is in the targetting area, function used is in PlantsVsZombies/Utilities
                bool validPosition = IsPointInPolygon(targettingArea, potentialTarget.Center);

                //the CanBeChasedBy() check is a built in function, tldr it checks that you can actually damage the npc, and that it isn't a critter, which isn't worth shooting
                //validPosition makes sure that the target is inside the targetting area
                if (potentialTarget.CanBeChasedBy() && validPosition)
                {
                    //gets the distance between the projectile and target
                    float distanceBetween = Vector2.Distance(Projectile.Center, potentialTarget.Center);

                    //returns if the projectile can "see" the target, this function is built into Terraria
                    bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, potentialTarget.position, potentialTarget.width, potentialTarget.height);

                    //checks if the current target is the closest target
                    bool closest = Vector2.Distance(Projectile.Center, targetCenter) > distanceBetween;

                    //checking that the current target is either the closest one, or if there is not current target, and if it has line of sight
                    if ((closest || !foundTarget) && lineOfSight)
                    {
                        //changes variables to reflect the data found, being the closest npc, and if a target is found
                        foundTarget = true;
                        target = potentialTarget;
                        targetCenter = potentialTarget.Center;
                    }
                }

            }

            //iterating
            Projectile.ai[0]++;

            //targetting code and spawning a secondary projectile
            if (foundTarget && Projectile.ai[0] >= 100)
            {
                //resetting iterable
                Projectile.ai[0] = 0;

                //this block changes the projectile's direction depending on where it's target is, flipping the sprite
                if ((Projectile.Center - target.Center).X > 0)
                {
                    Projectile.spriteDirection = -1;
                }
                else if ((Projectile.Center - target.Center).X < 0)
                {
                    Projectile.spriteDirection = 1;
                }

                //getting the vector between the projectile and it's target, and normalising it (making it travel the same total distance/second no matter the direction)
                Vector2 velocity = target.Center - Projectile.Center;
                velocity.Normalize();

                //slightly modifying the spawn location of the secondary projectile to look more natural
                Vector2 spawnlocation = new(Projectile.spriteDirection * 12, -8);

                //spawning the projectile that will actually damage enemies
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + spawnlocation, velocity * 7f, ModContent.ProjectileType<Pea>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }

            //projectiles are killed/despawned when their player dies
            Player player = Main.player[Projectile.owner];
            if (player.dead || !player.active)
            {
                Projectile.Kill();
            }

            //animating
            Visuals();


            /*bool foundTarget = false;
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
            }*/                          ///Old targeting code, didn't have desired output

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

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }
    }
}
