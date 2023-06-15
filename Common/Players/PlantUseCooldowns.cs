using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PlantsVsZombies.Content.Items.Weapons.PlantSummons;
using System;

namespace PlantsVsZombies.Common.Players
{
    public class PlantTimers : ModPlayer
    {
        public Dictionary<string, int> plantTimer = new();
        public override void Initialize()
        {   //remember 60 ticks per second, cooldowns in seconds listed after each line as comments
            plantTimer.Add("SunflowerPacket", 0); //10s
            plantTimer.Add("PeashooterPacket", 0); //7.5s
        }

        public override void PostUpdateMiscEffects()
        {
            foreach (string i in plantTimer.Keys)
            {
                if (plantTimer[i] > 0)
                {
                    plantTimer[i]--;
                }
            }
        }
    }
}
