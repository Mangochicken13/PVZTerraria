using Terraria.ModLoader;

namespace PlantsVsZombies.Common.Systems
{
    // This is the damage class used by the plant weapons
    // They are actually a subclass of summoner, but having this extra class lets me draw the UI when holding a weapon from the subclass
    internal class PlantDamage : DamageClass
    {
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            //this means that the plant class gets all generic damage buffs (13% increased damage, etc)
            if (damageClass == Generic)
            {
                return StatInheritanceData.Full;
            }

            // This damage class is supposed to work as a subclass of summon
            // As such, it inherits all stat boosts to summon weapons
            if (damageClass == Summon)
            {
                return StatInheritanceData.Full;
            }

            // This is effectively an "else" function, covering every other damage class, who's effects aren't inherited
            return StatInheritanceData.None;
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            //this damage class gets any effects that would apply to the parent class, such as a fire debuff effect or lifesteal effect
            if (damageClass == Summon)
                return true;

            return false;
        }
    }
}
