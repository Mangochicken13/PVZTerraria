using Terraria;
using Terraria.ModLoader;

namespace PlantsVsZombies.Common.Players
{
    public class Sun : ModPlayer
    {
        public int SunCurrent = 0;
        public const int SunDefaultMax = 9990; //this is the maximum sun in pvz2, so it made sense as a max here too
        public const int SunDefaultRegenMax = 50; //this value sets the default maximum sun
        public int SunMax;
        public int SunRegenMax;
        public float SunRegenRate; 
        internal float SunRegenTimer; //the iterable timer variable
        public int SunCost; //public, ie, available for anything that utilises this mod player's stats
        public int RegenAmount;
        public int AdditionalRegen;
        public override void Initialize()
        {
            SunRegenRate = 1;
            SunMax = SunDefaultMax;
            SunRegenMax = SunDefaultRegenMax; //so that accessories can change the regen maximum
        }

        public override void ResetEffects()
        {
            ResetVariables();
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }

        private void ResetVariables()
        {
            SunRegenMax = SunDefaultRegenMax;
        }

        public override void PostUpdateMiscEffects()
        {
            UpdateResource();
        }

        public override void OnEnterWorld()
        {
            var Sun = Player.GetModPlayer<Sun>();  //Comment this for the official releases
            Sun.SunCurrent = 50;
        }

        private void UpdateResource()
        {
            if (SunCurrent < SunRegenMax) //This timer only updates if you have less sun than your regen maximum
            {
                SunRegenTimer += (SunRegenRate);
            }

            if (SunRegenTimer >= 600) //ten seconds real time between regen instances (given no frame drops)
            {
                SunRegenTimer = 0;
                RegenAmount = RegenAmount + AdditionalRegen + 25; //The player will regen 25 sun per natural regen tick, plus any buffs given via accessories or consumables
            }


            if (RegenAmount / 25 > 1) //this whole block here is just so the number looks more natural as it increases in the UI
            {
                int temp = RegenAmount / 25;
                SunCurrent += temp;
                RegenAmount -= temp;
            }
            else if (RegenAmount >= 1)
            {
                SunCurrent++;
                RegenAmount--;
            }

            SunCurrent = Utils.Clamp(SunCurrent, 0, SunMax); //Making sure that you don't go over the limit, 9990 by default
        }
    }
}
