using UnityEngine;
using System.Collections;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public enum eSystemState
    {
        OFFLINE = 0x00,
        ONLINE = 0x01,
        DAMAGED = 0x02,        
        INVALID = 0x04,
    }
}