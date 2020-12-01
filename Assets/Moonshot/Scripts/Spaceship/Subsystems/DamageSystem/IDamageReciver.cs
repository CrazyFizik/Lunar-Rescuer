﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Moonshot.Scripts
{
    interface IDamageReciver
    {
        void Hit(float energy);
        void Hit(int damage);
        void Destruction();
    }

}
