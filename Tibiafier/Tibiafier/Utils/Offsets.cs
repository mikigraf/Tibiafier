using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tibiafier.Utils
{
    class Offsets
    {
        public UInt32 XORAddress = 0x53B760;
        public UInt32 TabsBaseAddress = 0x53BA48;
        public UInt32 MaxHealthAddress = 0x6D9048;
        public UInt32 BattleListAddress = 0x733790;
        public UInt32 HealthAddress = 0x6D9000;
        public UInt32 PlayerIDAddress = 0x6D9050;
        public UInt32 ManaAddress = 0x53B794;
        public UInt32 MaxManaAddress = 0x53B764;
        public UInt32 AmmunitionCountAddress = 0x779824;
        public UInt32 AmmunitionTypeAddress = 0x779828;
        public UInt32 WeaponCountAddress = 0x7798A4;
        public UInt32 WeaponTypeAddress = 0x7798A8;
        public UInt32 LevelAddress = 0x53B778;
        public UInt32 ExperienceAddress = 0x53B768;

        public Offsets()
        {

        }
    }
}
