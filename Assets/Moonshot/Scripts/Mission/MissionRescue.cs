using UnityEngine;
using System.Collections;

namespace Assets.Moonshot.Scripts.Mission
{
    public class MissionRescue : Mission
    {
        // Use this for initialization
        void Start()
        {
            _missions.Add(this);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Complete()
        {
            throw new System.NotImplementedException();
        }

        public override void DrawHUD()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsAchieved()
        {
            throw new System.NotImplementedException();
        }
    }
}