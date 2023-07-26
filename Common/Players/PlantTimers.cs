using System.Collections.Generic;
using Terraria.ModLoader;

namespace PlantsVsZombies.Common.Players
{
    public class PlantTimers : ModPlayer
    {
        public int[] _plantTimers;

        //public Dictionary<string, int> plantTimer = new();
        public override void Initialize()
        {   /*remember 60 ticks per second, cooldowns in seconds listed after each line as comments
            plantTimer.Add("SunflowerPacket", 0); //10s
            plantTimer.Add("PeashooterPacket", 0); //7.5s
            plantTimer.Add("RotobagaPacket", 0); //15s
            */

            _plantTimers = new int[PlantID.Count];
        }

        public override void PostUpdateMiscEffects()
        {
            /*foreach (string i in plantTimer.Keys)
            {
                if (plantTimer[i] > 0)
                {
                    plantTimer[i]--;
                }
            }*/

            for (int i = 0; i < PlantID.Count; i++)
            {
                if (_plantTimers[i] > 0)
                {
                    _plantTimers[i]--;
                }
            }
        }

        
    }
}
