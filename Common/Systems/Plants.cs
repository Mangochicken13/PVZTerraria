using Terraria.ModLoader;

namespace PlantsVsZombies.Common.Systems
{
    //this is the damage class used by the plant weapons
    internal class Plants : DamageClass
    {
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            //this means that the plant class gets all generic damage buffs (13% increased damage, etc)
            if (damageClass == Generic)
            {
                return StatInheritanceData.Full;
            }

            //this specifies that it recieves 20% of the buffs to crit chance and armor penetration that the summon class recieves
            if (damageClass == Summon)
            {
                return new StatInheritanceData(
                    damageInheritance: 0f,
                    critChanceInheritance: 0.2f,
                    attackSpeedInheritance: 0f,
                    armorPenInheritance: 0.2f,
                    knockbackInheritance: 0f
                    );
            }

            //this is effectively an "else" function, covering every other damage class
            return StatInheritanceData.None;
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            //this damage class gets any effects that would apply to the below damage classes, such as a fire debuff effect or lifesteal effect
            if (damageClass == Summon)
                return true;
            if (damageClass == Magic)
                return true;
            return false;
        }
    }
}
