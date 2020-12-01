using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    public interface IBoid
    {
        Vector2 _Position
        {
            get;
        }

        Vector2 _Velocity
        {
            get;
        }

        float _MaxVelocity
        {
            get;
        }
    }
}
