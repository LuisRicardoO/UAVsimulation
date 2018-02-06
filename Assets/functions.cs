using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace functionNameSpace
{
    class functions
    {
        public functions()
        {

        }
        public Vector4 copyVector3toVector4(Vector3 addontop, Vector4 ofthis)
        {
            return new Vector4(addontop.x, addontop.y, addontop.z,ofthis.w);
        }

        public Vector3 copyVector4toVector3(Vector4 addontop)
        {
            return new Vector3(addontop.x, addontop.y, addontop.z);
        }

        public Vector3 vlimitv(Vector3 v, Vector3 min, Vector3 max)
        {
            return new Vector3(limit(v.x, min.x, max.x), limit(v.y, min.y, max.y), limit(v.z, min.z, max.z));
        }

        public float limit(float input, float min, float max)
        {
            if (input < min)
                input = min;
            if (input > max)
                input = max;
            return input;
        }

        public float map(float x, float in_min, float in_max, float out_min, float out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        public Vector3 vsmoothTransition(Vector3 to, Vector3 from, float maxIteration, float lerpVar)
        {
            return new Vector3(smoothTransition(to.x, from.x, maxIteration, lerpVar), smoothTransition(to.y, from.y, maxIteration, lerpVar), smoothTransition(to.z, from.z, maxIteration, lerpVar));
        }

        public float smoothTransition(float to, float from, float maxIteration, float lerpVar)
        {
            if (Mathf.Abs(to - from) > maxIteration)
                return from + maxIteration * Mathf.Sign(to - from);
            else
                return Mathf.Lerp(to, from, lerpVar);

        }

        public Vector2 vsmoothTransitionXZ(Vector2 to, Vector2 from, float maxIteration, float lerpVar)
        {
            return new Vector3(smoothTransition(to.x, from.x, maxIteration, lerpVar), smoothTransition(to.y, from.y, maxIteration, lerpVar));
        }
    }
}
