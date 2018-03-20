using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using pclInterfaceName;

namespace laserRayCasterName
{
    class laserRayCaster
    {
        //basic parameters
        public float leftA { get; set; }
        public float rightA { get; set; }
        public float topA { get; set; }
        public float bottomA { get; set; }
        public float vertInc { get; set; }
        public float horiInc { get; set; }

        public float maxDist { get; set; }

        //which layer should rays consider
        public int rayLayer { get; set; }

        //main storage of point used for mode 1 and 2
        public Stack<Vector3> points = new Stack<Vector3>();

        //flag for stack completion
        public bool stackDone;

        //for operating sequential modes
        private float currentAngle;

        public laserRayCaster()
        {
            stackDone = true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pcl"></param>
        /// <param name="laser"></param>
        /// <param name="drawRay">to draw raycasts send this as true</param>
        /// <returns>true if there are a complete cloud in pcl otherwise return false</returns>
        public bool runCaster(ref pclInterface pcl, laserSensor laser, bool drawRay)
        {

            if (stackDone)//restart
            {
                currentAngle = topA;
                stackDone = false;
            }

            if (currentAngle >= bottomA)
            {
                float currentHoriAngle = leftA;
                while (currentHoriAngle < rightA)
                {
                    getPointFromRay(currentHoriAngle, currentAngle, maxDist, ref pcl, laser, drawRay);
                    currentHoriAngle += horiInc;
                }
                currentAngle -= horiInc;           
            }
            else
            {
                //Debug.Log("done " + pcl.cloudHasPoints());
                stackDone = true;
                if (pcl.cloudHasPoints())
                {
                    return true;
                }
            }
            return false;
        }

        public bool runCasterSimultaneos(ref pclInterface pcl, laserSensor laser, bool drawRay)
        {            
            float currentVertAngle = topA;
            //vertical laser movement
            while (currentVertAngle >= bottomA)
            {
                float currentHoriAngle = leftA;
                //horizontal lase movement
                while (currentHoriAngle < rightA)
                {
                    getPointFromRay(currentHoriAngle, currentVertAngle, maxDist,ref pcl, laser,drawRay);
                    currentHoriAngle += horiInc;
                }
                currentVertAngle -= vertInc;
            }
            if(pcl.cloudHasPoints())
            {
                return true;
            }
            return false;
        }

        private void getPointFromRay(float yAngle, float zAngle, float maxDist, ref pclInterface pcl, laserSensor laserObject, bool drawRay)
        {
            Vector3 direction = laserObject.transform.right;
            direction = Quaternion.AngleAxis(zAngle, laserObject.transform.forward) * direction;
            direction = Quaternion.AngleAxis(yAngle - 90, laserObject.transform.up) * direction;
            direction.Normalize();
            RaycastHit hit;
            if (Physics.Raycast(laserObject.transform.position, direction, out hit, maxDist, rayLayer))
            {                
                pcl.pushPointToCloud(new Vector3(hit.point.x, hit.point.z, hit.point.y));
            }
            if (drawRay)
                Debug.DrawRay(laserObject.transform.position, direction * maxDist, new Color(254, 254, 254, 0.5f));
        }

        public void defineParameters(float _leftA, float _rightA, float _topA, float _bottomA, float vInc, float hInc, float MD, int _rayLayer)
        {
            leftA = _leftA;
            rightA = _rightA;
            topA = _topA;
            bottomA = _bottomA;
            vertInc = vInc;
            horiInc = hInc;
            maxDist = MD;
            rayLayer = _rayLayer;
        }
    }
}
