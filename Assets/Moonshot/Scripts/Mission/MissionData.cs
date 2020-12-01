using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Moonshot.Scripts.Mission
{
    public class MissionData
    {
        public string Description { get; set; }
        public int Reward { get; set; }
        public bool Completed { get; set; }
        public void Complete()
        {
            Completed = true;
        }
    }
}
