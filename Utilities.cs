using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

using PlantsVsZombies.Common.Players;

namespace PlantsVsZombies
{
    public class Utilities 
    {
        public static bool IsPointInPolygon(Vector2[] polygon, Vector2 point)
        {   //this snippet is from dominoc925 on dominoc925.blogspot.com, edited to fit my use scenario
            //honestly, i don't exactly know how this does it, but i figured that it wasn't worth the time spent
            //to figure out how to do this myself, and it does what i wanted, so why bother making my own until it might
            //be necessary for optimisationg purposes
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


        //i attempted to make a function to check if you had enough sun to use items, but it didn't work.
        //this function will remain for potential use in the future
        public static bool CheckSun(int sunCost, Player player)
        {

            var Sun = player.GetModPlayer<Sun>();

            if (Sun.SunCurrent >= sunCost && (!Main.tile[Main.MouseWorld.ToTileCoordinates()].HasUnactuatedTile || Main.tile[Main.MouseWorld.ToTileCoordinates()].Equals(TileID.Platforms)))
            {
                Sun.SunCurrent -= sunCost;
                return true;
            }
            else { return false; }
        }
    }
}
