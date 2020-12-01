using System;
using UnityEngine;

namespace Assets.Moonshot.Scripts.Controllers
{
    public abstract class PlayerInput : MonoBehaviour
    {
        public abstract GameObject Ship { get; set; }
        public abstract IControls ShipControls { get; }
    }
}
