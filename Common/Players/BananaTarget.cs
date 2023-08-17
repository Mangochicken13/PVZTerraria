using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace PlantsVsZombies.Common.Players
{
    public class BananaTarget : ModPlayer
    {
        public Vector2 target = Vector2.Zero;
        float timer;

        public override void Initialize()
        {
            target = Vector2.Zero;
            timer = 0;
        }

        public override void PostUpdateMiscEffects()
        {
            if (target != Vector2.Zero)
            {
                timer++;
            }
            if (timer >= 15f)
            {
                target = Vector2.Zero;
                timer = 0;
            }
        }
        public override void UpdateDead()
        {
            /*target = Vector2.Zero;
            timer = timerBaseLength;
            base.UpdateDead();*/
        }
    }
}
