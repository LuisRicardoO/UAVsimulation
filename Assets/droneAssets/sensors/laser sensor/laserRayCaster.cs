using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using pclInterfaceName;
using RosSharp.RosBridgeClient;

namespace laserRayCasterName
{
    /// <summary>
    /// Used to ease the ray cast part of unity and 
    /// resposible for pushing the points to the pcl cloud
    /// </summary>
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
        private int repetitions;

        private pclInterface interPcl;

        //used for sequential capturing but regulated by the number casted from single update
        public laserRayCaster()
        {
            stackDone = true;            
        }

        public laserRayCaster(int _repetitions)
        {
            stackDone = true;
            repetitions = _repetitions;
        }

        //WORK IN PROGRESS!!!!!!!!
        public bool casteRay(ref SensorPointCloud2 rosCloud, laserSensor laser, bool drawRay)
        {
            if (stackDone)//restart
            {
                currentAngle = topA;
                stackDone = false;
                interPcl = new pclInterface();
            }
            return false;
        }











































        /// <summary>
        /// work in progress        
        /// </summary>
        /// <param name="pcl"></param>
        /// <param name="laser"></param>
        /// <param name="drawRay"></param>
        /// <param name="rosCloud"></param>
        /// <returns></returns>
        public bool runCasterSequentialRep2(ref SensorPointCloud2 rosCloud, laserSensor laser, bool drawRay)
        {            
            if (stackDone)//restart
            {
                currentAngle = topA;
                stackDone = false;
            }

            pclInterface pcl = new pclInterface();
            pcl.createPclCloud(0, 0, true);

            int cicles = 0;
            while (cicles < repetitions)
            {
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
                    stackDone = true;
                    if (rosCloud.data.Length>0)
                    {
                        return true;
                    }
                }
                cicles++;
            }
            return false;

        }

        public bool runCasterSequentialRep(ref pclInterface pcl, laserSensor laser, bool drawRay)
        {                                    
            if (stackDone)//restart
            {
                currentAngle = topA;
                stackDone = false;
            }

            int cicles = 0;
            while (cicles < repetitions)
            {
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
                cicles++;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pcl"></param>
        /// <param name="laser"></param>
        /// <param name="drawRay">to draw raycasts send this as true</param>
        /// <returns>true if there are a complete cloud in pcl otherwise return false</returns>
        public bool runCasterSequential(ref pclInterface pcl, laserSensor laser, bool drawRay)
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

        /// <summary>
        /// when pcl cloud completed return true
        /// </summary>
        /// <param name="pcl"></param>
        /// <param name="laser"></param>
        /// <param name="drawRay"></param>
        /// <returns></returns>
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
                    getPointFromRay(currentHoriAngle, currentVertAngle, maxDist, ref pcl, laser, drawRay);
                    currentHoriAngle += horiInc;
                }
                currentVertAngle -= vertInc;
            }
            if (pcl.cloudHasPoints())
            {
                //Debug.Log("Going to be converted");
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
                pcl.pushPointToCloud(hit.point);
            }
            if (drawRay)
                Debug.DrawRay(laserObject.transform.position, direction * maxDist, new Color(254, 254, 254, 0.5f));
        }

        private void getPointFromRay(float yAngle, float zAngle, float maxDist, ref pclInterface pcl, laserSensor laserObject, bool drawRay,ref SensorPointCloud2 rosCloud)
        {
            Vector3 direction = laserObject.transform.right;
            direction = Quaternion.AngleAxis(zAngle, laserObject.transform.forward) * direction;
            direction = Quaternion.AngleAxis(yAngle - 90, laserObject.transform.up) * direction;
            direction.Normalize();
            RaycastHit hit;
            if (Physics.Raycast(laserObject.transform.position, direction, out hit, maxDist, rayLayer))
            {
                pcl.pushPointToCloud(hit.point);
            }
            if (drawRay)
                Debug.DrawRay(laserObject.transform.position, direction * maxDist, new Color(254, 254, 254, 0.5f));
        }
        public void defineParameters(float _leftA, float _rightA, float _topA, float _bottomA, float vInc, float hInc, float MD, int _rayLayer, int _repetitions)
        {
            leftA = _leftA;
            rightA = _rightA;
            topA = _topA;
            bottomA = _bottomA;

            if (vInc < 2)
                vertInc = 2;
            else
                vertInc = vInc;
            if (hInc < 0.1)
                horiInc = 0.1f;
            else
                horiInc = hInc;
            maxDist = MD;
            rayLayer = _rayLayer;
            repetitions = _repetitions;
        }
    }
}
