using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Moonshot.Scripts.Controllers
{
    public interface IControls
    {
        Controls Controls
        {
            get;
            set;
        }
    }
}
