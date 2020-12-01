using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Moonshot.Scripts.Mission
{
    public abstract class Mission : MonoBehaviour
    {
        public static List<Mission> _missions = new List<Mission>();

        public abstract bool IsAchieved();
        public abstract void Complete();
        public abstract void DrawHUD();
    }
}