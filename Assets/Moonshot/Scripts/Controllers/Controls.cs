using UnityEngine;
using System.Collections;

namespace Assets.Moonshot.Scripts.Controllers
{
    /// <summary>
    /// Common controls used to control characters
    /// </summary>
    [System.Serializable]
    public class Controls
    {
        /// <summary>
        /// Angle
        /// </summary>
        public float turn;

        /// <summary>
        /// Spot position
        /// </summary>
        public Vector2 spot;

        /// <summary>
        /// Spot position
        /// </summary>
        public Vector2 target;

        /// <summary>
        /// Look direction
        /// </summary>
        public Vector2 direction;

        /// <summary>
        /// Move direction
        /// </summary>
        public Vector2 movement;

        /// <summary>
        /// Press to run
        /// </summary>
        public bool run;

        /// <summary>
        /// Ready button pressed
        /// </summary>
        public bool ready;

        /// <summary>
        /// Attack button pressed
        /// </summary>
        public bool attack;

        /// <summary>
        /// Special attack button pressed
        /// </summary>
        public bool special;

        /// <summary>
        /// Normalize direction vector
        /// </summary>
        public void Normalize()
        {
            direction.x = Mathf.Abs(direction.x) > 0.01f ? Mathf.Sign(direction.x) : 0;
            direction.y = Mathf.Abs(direction.y) > 0.01f ? Mathf.Sign(direction.y) : 0;
        }
    }
}