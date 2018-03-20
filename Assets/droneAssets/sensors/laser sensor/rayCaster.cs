using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using pclInterfaceName;

namespace rayCasterName
{
    class rayCaster
    {
        
        public Stack<Vector3> points = new Stack<Vector3>();
        public float leftAngle { get; set; }
        public float rightAngle { get; set; }
        public float topAngle { get; set; }
        public float bottomAngle { get; set; }
        public float vertIncrement { get; set; }
        public float horiIncrement { get; set; }
        public float maxDist { get; set; }
        public laserSensor laserObject { get; set; }
        public int rayLayer { get; set; }

        public bool stackDone = true;
        private float currentVertAngle;        
        private Stack<Vector3> tempPoints = new Stack<Vector3>();

        public rayCaster(float leftA, float rightA, float topA, float bottomA, float vInc, float hInc, float MD, laserSensor _laserObject, int _rayLayer)
        {
            leftAngle = leftA;
            rightAngle = rightA;
            topAngle = topA;
            bottomAngle = bottomA;
            vertIncrement = vInc;
            horiIncrement = hInc;
            maxDist = MD;
            laserObject = _laserObject;
            rayLayer = _rayLayer;            
        }

        /// <summary>
        /// 1=sequential
        ///in sequential mode for each call of runRaycaster
        ///it will only calculate ONE horizontal line
        ///only when full specefied coverage is complete
        ///the stack points will be filled
        ///2=simultaneos mode
        ///every raycast is exacuted in a single call to runRayCaster();
        ///3=equal to simultaneos but using pcl to add directly the points() maybe the fastest
        /// </summary>        
        public void runRayCaster(int mode)
        {
            //points.Clear();
            if (mode==1)
            {
                sequentialPushPoints();
            }
            else if(mode==2){
                //in non sequential all rays are casted                
                pushPoints();
            }
        }
        public void runRayCaster(pclInterface pcl)
        {
                pclSequentialMode(pcl);
        }
        private void pclSequentialMode(pclInterface pcl)
        {
            if (stackDone)//restart
            {
                currentVertAngle = topAngle;
                stackDone = false;
            }
            if (currentVertAngle >= bottomAngle)
            {

                float currentHoriAngle = leftAngle;
                while (currentHoriAngle < rightAngle)
                {
                    getPointFromRay(currentHoriAngle, currentVertAngle, maxDist,pcl);
                    currentHoriAngle += horiIncrement;
                }
                currentVertAngle -= horiIncrement;
            }
            else
            {
                stackDone = true;
                Debug.Log("stackDone");
            }
        }

        private void sequentialPushPoints()
        {
            if (stackDone)//restart
            {
                currentVertAngle = topAngle;
                stackDone = false;
            }
            if (currentVertAngle >= bottomAngle)
            {

                float currentHoriAngle = leftAngle;
                while (currentHoriAngle < rightAngle)
                {
                    getPointFromRay(currentHoriAngle, currentVertAngle, maxDist, this.points);
                    currentHoriAngle += horiIncrement;
                }
                currentVertAngle -= horiIncrement;
            }
            else
            {
                stackDone = true;                
            }
        }

        private void pushPoints()
        {

            float currentVertAngle = topAngle;
            //vertical laser movement
            while (currentVertAngle >= bottomAngle)
            {
                float currentHoriAngle = leftAngle;
                //horizontal lase movement
                while (currentHoriAngle < rightAngle)
                {
                    getPointFromRay(currentHoriAngle, currentVertAngle, maxDist, this.points);
                    currentHoriAngle += horiIncrement;
                }
                currentVertAngle -= vertIncrement;
            }
            stackDone = true;
        }

        private void getPointFromRay(float yAngle, float zAngle, float maxDist, Stack<Vector3> stack)
        {
            Vector3 direction = laserObject.transform.right;
            direction = Quaternion.AngleAxis(zAngle, laserObject.transform.forward) * direction;
            direction = Quaternion.AngleAxis(yAngle - 90, laserObject.transform.up) * direction;
            direction.Normalize();
            //Debug.Log("zangle: " + zAngle.ToString()+" yangle: "+yAngle.ToString());
            RaycastHit hit;
            if (Physics.Raycast(laserObject.transform.position, direction, out hit, maxDist, rayLayer))
            {
                stack.Push(hit.point);
            }

            // Debug.DrawRay(laserObject.transform.position, direction * maxDist, new Color(254, 254, 254, 0.5f));            
        }

        private void getPointFromRay(float yAngle, float zAngle, float maxDist, pclInterface pcl)
        {
            Vector3 direction = laserObject.transform.right;
            direction = Quaternion.AngleAxis(zAngle, laserObject.transform.forward) * direction;
            direction = Quaternion.AngleAxis(yAngle - 90, laserObject.transform.up) * direction;
            direction.Normalize();
            //Debug.Log("zangle: " + zAngle.ToString()+" yangle: "+yAngle.ToString());
            RaycastHit hit;
            if (Physics.Raycast(laserObject.transform.position, direction, out hit, maxDist, rayLayer))
            {
                pcl.pushPointToCloud(hit.point);
            }
            // Debug.DrawRay(laserObject.transform.position, direction * maxDist, new Color(254, 254, 254, 0.5f));            
        }

        //https://stackoverflow.com/questions/7391348/c-sharp-clone-a-stack
        public static Stack<Vector3> Clone(Stack<Vector3> original)
        {
            var arr = new Vector3[original.Count];
            original.CopyTo(arr, 0);
            Array.Reverse(arr);
            return new Stack<Vector3>(arr);
        }
    }
}
