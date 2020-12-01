using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public interface IWeapon
    {
        string Name
        {
            get;
        }

        Ship Ship
        {
            get;
            set;
        }

        AudioSource AudiouOutput
        {
            get;
        }

        AudioClip ShootSound
        {
            get;
        }

        void Fire(Transform target);

        void Release();

    }
}
