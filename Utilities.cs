using Microsoft.Xna.Framework;

namespace PlantsVsZombies
{
    public class Utilities
    {
        // This snippet is from dominoc925 on dominoc925.blogspot.com, edited to fit my use scenario.
        // Honestly, i don't exactly know how this does it, but i figured that it wasn't worth the time spent
        // to figure out how to do this myself, and it does what i wanted, so why bother making my own until it might
        // be necessary for optimisation purposes.

        /// <summary>
        /// This a method for checking that a target is inside a desired attacking range/area.
        /// It works via world coordinates, so make sure values are calculated with the 1 tile to 16 units ratio in mind.
        /// </summary>
        /// <param name="polygon">Array of Vector2 points marking the boundaries of the targetting area</param>
        /// <param name="point">Position of the target</param>
        /// <returns></returns>
        public static bool IsPointInPolygon(Vector2[] polygon, Vector2 point)
        {
            bool isInside = false;

            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
                (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))

                {
                    isInside = !isInside;
                }
            }
            return isInside;
        }
    }
}
