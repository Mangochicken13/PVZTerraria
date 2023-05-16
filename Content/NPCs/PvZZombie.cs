using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.Utilities;

namespace PlantsVsZombies.Content.NPCs
{
    //this was a temporary ammendment made for my school project, and will not be making it into a release version, due to incomplete sprites, and a current lack of need for enemies
    internal class PvZZombie { } /* : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.damage = 14;
            NPC.defense = 1;
            NPC.lifeMax = 50;
            NPC.aiStyle = -1;
            NPC.height = 32;
            NPC.width = 32;
            NPC.noGravity = true;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZoneOverworldHeight ? 1f : 0f;
        }

        public override bool PreAI()
        {
            NPC.ai[0] = 15;
            return true;
        }
    }*/
}
