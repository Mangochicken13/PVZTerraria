using Terraria.ModLoader;

namespace PlantsVsZombies.Common.Players
{
    public class PlantTimers : ModPlayer
    {
        public int[] plantTimers;
        public override void Initialize()
        {
            // Setting the length of the timer array to the amount of plants.
            // PlantID.Count should always be 1 higher than the highest value plant, but is updated manually.
            // Note to self: Be sure to double check that you have incremented it's value with every new plant added.
            plantTimers = new int[PlantID.Count];
        }

        public override void PostUpdateMiscEffects()
        {
            // Decrease each timer by 1 per frame.
            // Keep note of this method, it might become costly with a lot of plants.
            for (int i = 0; i < PlantID.Count; i++)
            {
                if (plantTimers[i] > 0)
                {
                    plantTimers[i]--;
                }
            }
        }


    }
}
