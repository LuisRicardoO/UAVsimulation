using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace contolVarsNamespace
{
    class controlVars
    {
        public float p { get; set; }
        public float i { get; set; }
        public float d { get; set; }
        public float il { get; set; }//integral limiter

        public controlVars(float P_,float I_ ,float D_, float il_)
        {
            p = P_;
            i = I_;
            d = D_;
            il = il_;
        }

        public controlVars(Vector4 pid)
        {
            p = pid.x;
            i = pid.y;
            d = pid.z;
            il = pid.w;
        }
    }
}
