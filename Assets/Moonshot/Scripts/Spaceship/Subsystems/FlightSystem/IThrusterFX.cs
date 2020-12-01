using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public interface IThrusterFX
    {
        void SetFX(float throttle);
    }
}
