
using UnityEngine;

namespace pidNamespace
{
    public class PID
    {
        public float pFactor, iFactor, dFactor;

        private bool integrate=true;

        float integral;
        float lastError;
        float limit;


        public PID(float pFactor, float iFactor, float dFactor)
        {
            this.pFactor = pFactor;
            this.iFactor = iFactor;
            this.dFactor = dFactor;
        }


        public float Update(float setpoint, float actual, float timeFrame)
        {

            float error = setpoint - actual;
            float preIntegral = integral + error * timeFrame;

            float deriv = (error - lastError) / timeFrame;
            lastError = error;
                        
            float preOutput = error * pFactor + preIntegral * iFactor + deriv * dFactor;
            //anti-windup clamping
           
            if (Mathf.Abs(preOutput) > limit /*&& Mathf.Sign(integral) == Mathf.Sign(preOutput)*/)
            {
                
                integrate = false;
            }
            else if (Mathf.Abs(preOutput) < limit /*&& (Mathf.Sign(integral) != Mathf.Sign(preOutput))*/)
            {
               
                integrate = true;
            }

            

            if (integrate)
            {
                integral += error * timeFrame;//with integration

            }else
            {
                integral = 0;
            }

            return error * pFactor + integral * iFactor + deriv * dFactor;
        }


        public void defineControlVars(Vector3 Ks,float limitPid)
        {
            pFactor = Ks.x;
            iFactor = Ks.y;
            dFactor = Ks.z;
            limit = limitPid;

        }

        public Vector3 showK()
        {
            return new Vector3(pFactor, iFactor, dFactor);
        }
        
    }
}