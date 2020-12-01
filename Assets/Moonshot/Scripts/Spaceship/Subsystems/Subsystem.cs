using UnityEngine;
using System.Collections;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public abstract class Subsystem : MonoBehaviour
    {
        public abstract string Name
        {
            get;
            set;
        }

        public abstract string Type
        {
            get;
            set;
        }

        public abstract SystemState Status
        {
            get;
            set;
        }

        public abstract Ship Ship
        {
            get;
            set;
        }

        public abstract void Init();

        public abstract void TakeDamage();

        public abstract void TakeDamage(float damage);

        public abstract void Repair(float dt);
    }
}