using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public interface IThruster
    {
        Transform Root
        {
            get;
            set;
        }

        Vector3 ForwardDirection
        {
            get;
        }

        Vector3 Position
        {
            get;
        }

        Vector3 RelativePosition
        {
            get;
        }
        
        Vector3 Force
        {
            get;
        }

        Vector3 Torque
        {
            get;
        }

        float Throttle
        {
            get;
            set;
        }

        void Init(Transform root);
        Vector3 GetForce();

        Vector3 GetTorque();

        float GetForceByDirection(Vector3 direction);

        float GetTorqueByDirection(Vector3 direction);

        float ActivateRelative(Vector3 translation, Vector3 rotation, Vector3 centerOfMass);

        float Activate(Vector3 translation, Vector3 rotation);
    }
}