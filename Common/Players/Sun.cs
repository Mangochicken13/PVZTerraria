using System;
using Terraria;
using Terraria.ModLoader;

namespace PlantsVsZombies.Common.Players
{
    public class Sun : ModPlayer
    {
        // Microsoft standard is PascalCase apparently, according to ktaranov's github repo "naming-covention"
        
        public int CurrentSun; // The player's sun amount

        // The value that will show in the UI.
        // This is a seperate variable as it is shown via text,
        // and I want it to "scroll" naturally when using/gaining sun.
        public int CurrentSunDisplay;
        public int SunDifference; // Difference between the internal value and the displayed value, used for math.

        public float SunRegenTimer;
        private const float RegenTime = 600;

        private const int DefaultSunRegenMax = 50; // Default maximum natural regen of sun.
        public int SunRegenMax;

        private const float DefaultSunRegenRate = 1; 
        public float SunRegenRate;

        private const int DefaultRegenAmount = 25;
        public int AdditionalRegen;

        private const int DefaultMaxSun = 9990;
        public int MaxSun;
        public int MaxSun2;

        public int DecreasedCostAdd;
        public float DecreasedCostMult;

        public override void Initialize()
        {
            // Currently this sets the default stats of variables, so that they can be edited later, via in-game features
            MaxSun = DefaultMaxSun;
            SunRegenMax = DefaultSunRegenMax; // The max regen is set to the default amount
            SunRegenRate = DefaultSunRegenRate;
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
            SunRegenMax = DefaultSunRegenMax;
            MaxSun2 = MaxSun;
            AdditionalRegen = 0;
            DecreasedCostAdd = 0;
            DecreasedCostMult = 1;
        }

        public override void PostUpdateMiscEffects()
        {
            // Updating the resource, goes into detail at the end of the file
            UpdateResource();
        }

        public override void OnEnterWorld()
        {
            // Gives you 50 sun to start off with (normally)
            var Sun = Player.GetModPlayer<Sun>();
            Sun.CurrentSun = 5000; // DO NOT KEEP THIS VALUE IN ANY RELEASES
            Sun.CurrentSunDisplay = 0;
        }

        private void UpdateResource()
        {
            #region Regen
            // This timer only updates if you have less sun than your regen maximum (50 by default)
            if (CurrentSun < SunRegenMax)
            {
                SunRegenTimer += (SunRegenRate); // SunRegenRate is planned to be able to be modified, although it isn't currently
            }

            // Ten seconds real time between regen instances (given no frame drops, at the default rate)
            if (SunRegenTimer >= RegenTime)
            {
                SunRegenTimer = 0; // Reset the timer

                // The player will regen 25 sun per natural regen tick, plus any buffs given via accessories or consumables
                CurrentSun += AdditionalRegen + DefaultRegenAmount;
            }
            #endregion

            #region Smooth UI updating

            // This whole block here is just so the number looks more natural in the UI as it changes, the internal amount of sun is changed instantly
            SunDifference = CurrentSun - CurrentSunDisplay;
            if (Math.Abs(SunDifference / 25) > 1)
            {
                // Math to make the current sun increment faster
                int temp = SunDifference / 25;
                CurrentSunDisplay += temp;
            }
            else if (Math.Abs(SunDifference / 15) > 1)
            {
                int temp = SunDifference / 15;
                CurrentSunDisplay += temp;
            }
            else if (Math.Abs(SunDifference) >= 1)
            {
                if (SunDifference > 0)
                {
                    CurrentSunDisplay++;
                }
                else if (SunDifference < 0)
                {
                    CurrentSunDisplay--;
                }
            }

            #endregion

            Math.Clamp(CurrentSun, 0, MaxSun2);
        }
    }
}
