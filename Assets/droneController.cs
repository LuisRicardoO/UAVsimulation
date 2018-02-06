using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pidNamespace;
using UnityEngine;
using contolVarsNamespace;
using functionNameSpace;

//control relative linear velocity and angular velocity

//To implement
//control linear velocity x y z
//  control angular position x z
//      control angular velocity x z
//  control force y
//control angular velocity y
//  control torque y

//Implemented (shortcut)
//control linear velocity x y z
//  control force x y z
//control angular velocity y
//  control torque y
namespace controlNameSpace
{
    class droneController
    {
        //input
        //public Vector3 refLinVel;
        //public float refAngVel;
        //output
        //public Vector3 relTorque;
        //public float yForce;

        //time
        float k;

        private PID XlinVelC = new PID(0, 0, 0);
        private PID YlinVelC = new PID(0, 0, 0);
        private PID ZlinVelC = new PID(0, 0, 0);
        //private PID XanglePosC = new PID(0, 0, 0);
        //private PID YanglePosC = new PID(0, 0, 0);
        //private PID ZanglePosC = new PID(0, 0, 0);

        //private PID XangleVelC = new PID(0, 0, 0);
        private PID YangleVelC = new PID(0, 0, 0);
        //private PID ZangleVelC = new PID(0, 0, 0);

        //private PID YforceC = new PID(0, 0, 0);

        //aux funct
        private functions f = new functions();

        //for visual representation
        private Vector3 anglesOld;

        public droneController(controlVars pidXlinVel, controlVars pidYlinVel, controlVars pidZlinVel, controlVars pidXangVel, controlVars pidYangVel, controlVars pidZangVel, controlVars pidXangPos, controlVars pidYangPos, controlVars pidZangPos, controlVars pidYforce)
        {
            defineControlVars(pidXlinVel, pidYlinVel, pidZlinVel, pidXangVel, pidYangVel, pidZangVel, pidXangPos, pidYangPos, pidZangPos, pidYforce);
        }

        public droneController(controlVars pidXlinVel, controlVars pidYlinVel, controlVars pidZlinVel, controlVars pidYangVel, float time)
        {
            defineControlVarsSimplified(pidXlinVel, pidYlinVel, pidZlinVel, pidYangVel);
            k = time;
        }
        public void defineControlVars(controlVars pidXlinVel, controlVars pidYlinVel, controlVars pidZlinVel, controlVars pidXangVel, controlVars pidYangVel, controlVars pidZangVel, controlVars pidXangPos, controlVars pidYangPos, controlVars pidZangPos, controlVars pidYforce)
        {
            XlinVelC.defineControlVars(new Vector3(pidXlinVel.p, pidXlinVel.i, pidXlinVel.d), pidXlinVel.il);
            YlinVelC.defineControlVars(new Vector3(pidYlinVel.p, pidYlinVel.i, pidYlinVel.d), pidYlinVel.il);
            ZlinVelC.defineControlVars(new Vector3(pidZlinVel.p, pidZlinVel.i, pidZlinVel.d), pidZlinVel.il);

            //XanglePosC.defineControlVars(new Vector3(pidXangPos.p, pidXangPos.i, pidXangPos.d), pidXangPos.il);
            //YanglePosC.defineControlVars(new Vector3(pidYangPos.p, pidYangPos.i, pidYangPos.d), pidYangPos.il);
            //ZanglePosC.defineControlVars(new Vector3(pidZangPos.p, pidZangPos.i, pidZangPos.d), pidZangPos.il);

            //XangleVelC.defineControlVars(new Vector3(pidXangVel.p, pidXangVel.i, pidXangVel.d), pidXangVel.il);
            YangleVelC.defineControlVars(new Vector3(pidYangVel.p, pidYangVel.i, pidYangVel.d), pidYangVel.il);
            //ZangleVelC.defineControlVars(new Vector3(pidZangVel.p, pidZangVel.i, pidZangVel.d), pidZangVel.il);

            //YforceC.defineControlVars(new Vector3(pidYforce.p, pidYforce.i, pidYforce.d), pidYforce.il);
        }

        public void defineControlVarsSimplified(controlVars pidXlinVel, controlVars pidYlinVel, controlVars pidZlinVel, controlVars pidYangVel)
        {
            XlinVelC.defineControlVars(new Vector3(pidXlinVel.p, pidXlinVel.i, pidXlinVel.d), pidXlinVel.il);
            YlinVelC.defineControlVars(new Vector3(pidYlinVel.p, pidYlinVel.i, pidYlinVel.d), pidYlinVel.il);
            ZlinVelC.defineControlVars(new Vector3(pidZlinVel.p, pidZlinVel.i, pidZlinVel.d), pidZlinVel.il);

            YangleVelC.defineControlVars(new Vector3(pidYangVel.p, pidYangVel.i, pidYangVel.d), pidYangVel.il);
        }


        /// <summary>
        /// this controller given a velocities with respect to the robot calculates the forces also relative to robot frame
        /// this is not how the interacction between real drone and ros planners work
        /// For thta use controlByforce2 which is also a simplified version but compatible with ros navigation planner
        /// </summary>
        /// <param name="linVelRef"></param>
        /// <param name="angYvelRef"></param>
        /// <param name="sys"></param>
        /// <returns></returns>
        public Vector4 controlByForce(Vector3 linVelRef, float angYvelRef, Rigidbody sys)
        {
            Vector4 output = new Vector4();
            Vector3 force = controlLinVel(linVelRef, sys.transform.InverseTransformDirection(sys.velocity));
            output = f.copyVector3toVector4(force, output);
            output.w = controlAngVel(angYvelRef, sys.transform.InverseTransformDirection(sys.angularVelocity).y);
            return output;
        }

        /// <summary>
        /// this assumes that the drone does changes angles and given relative velocities are assuming just that
        /// also the resultant force follow assumtion
        /// this is how the planner send the commands to a real UAV
        /// </summary>
        /// <param name="linVelRef"></param>
        /// <param name="angYvelRef"></param>
        /// <param name="sys"></param>
        /// <returns></returns>
        public Vector4 controlByForce2(Vector3 linVelRef, float angYvelRef, Rigidbody sys)
        {
            Vector4 output = new Vector4();
            //obtain drone relative velocity assuming that robot is parallel to the floor
            
            Vector3 paraLinVel = Quaternion.AngleAxis(-sys.rotation.eulerAngles.y, Vector3.up) * sys.velocity;
            Vector3 force = controlLinVel(linVelRef, paraLinVel);//this force is diferent from global only by yaw angle            
            //the force is to be applied relative to the drone but assuming that it is plannar to the floor
            Vector3 paraForce = Quaternion.AngleAxis(sys.rotation.eulerAngles.y, Vector3.up) * force;//undo the effect of yaw angle to get world force coordinates
            output = f.copyVector3toVector4(paraForce, output);
            //Debug.Log(sys.angularVelocity.y);
            //Debug.Log(sys.angularVelocity.y.ToString("0.000")+ "   "+ sys.rotation.eulerAngles.y.ToString("0.000"));
            output.w = controlAngVel(angYvelRef, sys.angularVelocity.y);


            return output;
        }


        /// <summary>
        /// given a velocity controls the necessary force
        /// </summary>
        /// <param name="refVel"></param>
        /// <param name="actVel"></param>
        /// <returns></returns>
        public Vector3 controlLinVel(Vector3 refVel, Vector3 actVel)
        {
            //Debug.Log("vel ref:" + refVel.ToString("0.00") + ". act vel:" + actVel.ToString("0.00"));


            Vector3 force = new Vector3();
            force.x = XlinVelC.Update(refVel.x, actVel.x, k);
            force.y = YlinVelC.Update(refVel.y, actVel.y, k);
            force.z = ZlinVelC.Update(refVel.z, actVel.z, k);
            return force;
        }

        public float controlAngVel(float angYvelRef, float actYvel)
        {
            //Debug.Log("vel ref: " + angYvelRef.ToString("0.00") + ". act vel" + actYvel.ToString("0.00") + ". the P" + YangleVelC.pFactor);
            float torque = 0;
            torque = YangleVelC.Update(angYvelRef, actYvel, k);
            return torque;
        }

        public void visualRepresentationSimplified(Vector3 globalForce, float maxit, float lerpValue, Rigidbody sys, Vector3 forceLimitMin, Vector3 forceLimitMax, Vector3 minAllowedAngle, Vector3 maxAllowedAngle)
        {
            Vector3 localForce = Quaternion.AngleAxis(-sys.rotation.eulerAngles.y, Vector3.up) * globalForce;
            //Debug.Log(localForce.ToString("0.000")+" "+globalForce.ToString("0.000"));


            Vector3 angles = new Vector3(0, 0, 0);

            //pitch
            angles.z = -f.map(localForce.x, forceLimitMin.x, forceLimitMax.x, minAllowedAngle.z, maxAllowedAngle.z);
            
            //roll
            angles.x= f.map(localForce.z, forceLimitMin.z, forceLimitMax.z, minAllowedAngle.x, maxAllowedAngle.x);
            
            //smooth movemnent
            angles = f.vsmoothTransition(angles, anglesOld, maxit, lerpValue);

            //dont mess with this angle
            angles.y = sys.rotation.eulerAngles.y;

            sys.transform.rotation = Quaternion.Euler(angles.x, angles.y, angles.z);

            //update
            anglesOld = angles;
        }
    }
}
