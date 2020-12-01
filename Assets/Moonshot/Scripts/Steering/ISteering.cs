using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    public interface ISteering
    {
        IBoid _Host
        {
            get;
            set;
        }

        Vector2 _Movement
        {
            set;
        }

        Vector2 _Direction
        {
            set;
        }

        void Update();
    }
}
