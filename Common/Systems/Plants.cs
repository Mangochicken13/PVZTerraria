using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace PlantsVsZombies.Common.Systems
{
    internal class Plants : DamageClass
    {
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic)
            {
                return StatInheritanceData.Full;
            }

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

            return StatInheritanceData.None;
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (damageClass == Summon)
                return true;
            if (damageClass == Magic)
                return true;
            return false;
        }
    }
}
