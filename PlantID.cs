using ReLogic.Reflection;

namespace PlantsVsZombies
{
    public class PlantID
    {
        // Note to self: Try and find a way to automatically populate this according to some order

        public const short Null = 0;

        public const short Sunflower = 1;
        public const short Peashooter = 2;
        public const short Rotobaga = 3;
        public const short BananaLauncher = 4;

        public const short Count = 5;

        public static readonly IdDictionary Search = IdDictionary.Create<PlantID, short>();
    }
}