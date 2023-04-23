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
        internal float SunRegenTimer; //the iterable timer variable, only available in this file (local)
        public int SunCost; //public, ie, available for anything that utilises this mod player's stats
        public int RegenAmount;
        public int AdditionalRegen;
        public override void Initialize()
        {
            //currently this sets the default stats of variables, so that they can be edited later, via in-game features
            SunRegenRate = 1;
            SunMax = SunDefaultMax;
            SunRegenMax = SunDefaultRegenMax; //so that accessories can change the regen maximum
        }

        public override void ResetEffects()
        {
            //used for resetting variable on player death, same with function below - UpdateDead()
            ResetVariables();
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }

        private void ResetVariables()
        {
            //sets the regen maximum to the default maximum, to enable changes via other methods in the future
            SunRegenMax = SunDefaultRegenMax;
        }

        public override void PostUpdateMiscEffects()
        {
            //updating the resource, goes into detail at the end of the file
            UpdateResource();
        }

        public override void OnEnterWorld()
        {
            //gives you 50 sun to start off with, temporary feature
            var Sun = Player.GetModPlayer<Sun>();  ///Comment this for the official releases
            Sun.SunCurrent = 50;
        }

        private void UpdateResource()
        {
            //This timer only updates if you have less sun than your regen maximum (50)
            if (SunCurrent < SunRegenMax)
            {
                SunRegenTimer += (SunRegenRate);
            }

            //ten seconds real time between regen instances (given no frame drops)
            if (SunRegenTimer >= 600)
            {
                //resetting the iterable
                SunRegenTimer = 0;

                //The player will regen 25 sun per natural regen tick, plus any buffs given via accessories or consumables
                RegenAmount = RegenAmount + AdditionalRegen + 25;
            }

            //this whole block here is just so the number looks more natural as it increases in the UI
            if (RegenAmount / 25 > 1)
            {
                //math to make the current sun increment faster
                int temp = RegenAmount / 25;
                SunCurrent += temp;
                RegenAmount -= temp;
            }
            else if (RegenAmount >= 1)
            {
                SunCurrent++;
                RegenAmount--;
            }

            //Making sure that you don't go over the limit, 9990 by default
            SunCurrent = Utils.Clamp(SunCurrent, 0, SunMax); 
        }
    }
}
