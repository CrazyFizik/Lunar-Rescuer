using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Moonshot.Scripts.Controllers
{
    interface ICollectable
    {
        void PickUp(GameObject item);

        void Drop();
    }
}
