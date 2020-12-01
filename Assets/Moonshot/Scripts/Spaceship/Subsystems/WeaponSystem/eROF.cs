using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    [System.Serializable]
    public enum eROF
    {
        Single = 1,
        BurstFire = 3,
        LowROF = 5,
        HighROF = 10,
        Auto = 0
    }
}
