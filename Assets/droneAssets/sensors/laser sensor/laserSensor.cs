using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using System;
using System.Runtime.InteropServices;
using pclInterfaceName;
using rayCasterName;
//using pcLib;
using laserRayCasterName;

public class laserSensor : MonoBehaviour
{


    public bool run;
    public bool drawSpheres;
    public int sequentialMode = 3;
    public string publishTopic = "drone/laserScan";
    public string frame_id = "laserFrame";
    public float maxLeftAngle;
    public float maxRightAngle;
    public float maxTopAngle;
    public float maxBottomAngle;
    public float verticalIncrement;
    public float horizontalIncrement;
    public float maxDistance;
    public bool drawRay;

    private laserRayCaster laser2 = new laserRayCaster();

    private rayCaster laser;

    pclInterface pcl = new pclInterface();

    private Rigidbody droneBody;
    private RosSocket rosSocket;
    private int publication_id;

    private Vector3 gizmoPoint = new Vector3(0, 0, 0);
    private Stack<Vector3> gizStack = new Stack<Vector3>();
    void Start()
    {
        rosSocket = GameObject.Find("drone").GetComponent<RosConnector>().RosSocket;
        droneBody = GameObject.Find("drone").GetComponent<Rigidbody>();
        publication_id = rosSocket.Advertize(publishTopic, "sensor_msgs/PointCloud2");
        laser = new rayCaster(maxLeftAngle, maxRightAngle, maxTopAngle, maxBottomAngle, verticalIncrement, horizontalIncrement, maxDistance, this, 1 << 8);
        pcl.createPclCloud(0, 0, true);
    }
    void Update()
    {
        laser2.defineParameters(maxLeftAngle, maxRightAngle, maxTopAngle, maxBottomAngle, verticalIncrement, horizontalIncrement, maxDistance, 1 << 8);
        if (run)
        {
            ////este e o que funciona
            //if (laser2.runCaster(ref pcl, this, drawRay))
            //{
            //    SensorPointCloud2 pc = new SensorPointCloud2();
            //    pcl.convertToRosMsgFromCloud(pc, frame_id);
            //    Debug.Log("CloudR->" + pcl.readCloudRosParameters(pc));
            //    //rosSocket.Publish(publication_id, pc);
            //    //pcl.createPclCloud(0, 0, true);
            //}

            /*
            pcl.pushPointToCloud(new Vector3(1, 2, 3));
            SensorPointCloud2 pc = new SensorPointCloud2();
            pcl.convertToRosMsgFromCloud(pc, frame_id);
            Debug.Log("CloudR->" + pcl.readCloudRosParameters(pc));
            pcl.createPclCloud(0, 0, true);
            */
            unsafe
            {
                byte[] data = new byte[0];
                pcl.testByteArray3(ref data);
                Debug.Log("data: "+data[0]+" "+data[1]+ " " +data[3]);
            }


        }

        //    pcl.createPclCloud(0, 0, true);
        //    //get points
        //    //pushPointsToPcl()
        //    //for (int i = 0; i < 20; i++)
        //    //{
        //    //    pcl.pushPointToCloud(new Vector3(1, i, 1));
        //    //}
        //    //pushPointstoPcl(maxLeftAngle, maxRightAngle, maxTopAngle, maxBottomAngle, maxDistance, horizontalIncrement, verticalIncrement);

        //    castToPcl();


        //    if (sequentialMode != 3)
        //    {
        //        if (pcl.cloudHasPoints())
        //        {
        //            SensorPointCloud2 pc = new SensorPointCloud2();
        //            pcl.convertToRosMsgFromCloud(pc, frame_id);
        //            //Debug.Log("pcl cloud->"+pcl.readCloudParameters());
        //            //Debug.Log("Cloud2->" + pcl.readCloud2Parameters());
        //            //Debug.Log("CloudR->" + pcl.readCloudRosParameters(pc));
        //            rosSocket.Publish(publication_id, pc);
        //        }
        //    }
        //    else if (laser.stackDone)
        //    {
        //        Debug.Log("publishing");
        //        SensorPointCloud2 pc = new SensorPointCloud2();
        //        pcl.convertToRosMsgFromCloud(pc, frame_id);
        //        rosSocket.Publish(publication_id, pc);
        //    }

    }

    private void castToPcl()
    {
        if (sequentialMode == 3)
        {
            laser.runRayCaster(pcl);
        }
        else
        {
            laser.runRayCaster(sequentialMode);
            if (laser.stackDone)
            {

                if (drawSpheres)
                    gizStack = rayCaster.Clone(laser.points);
                Debug.Log("after gizStack" + gizStack.Count);

                while (laser.points.Count > 0)
                {
                    Vector3 point = laser.points.Pop();
                    pcl.pushPointToCloud(new Vector3(point.x, point.z, point.y));
                }
            }
        }
    }



    void OnDrawGizmos()
    {
        if (drawSpheres && sequentialMode != 3)
        {
            //Debug.Log("on giz gizStack" + gizStack.Count);
            while (gizStack.Count > 0)
            {
                Gizmos.DrawSphere(gizStack.Pop(), 0.015f);
            }
        }
    }
}

//private void pushPointstoPcl(float leftAngle, float rightAngle, float topAngle, float bottomAngle, float maxDist, float horiIncrement, float verticalIncrement)
//{

//    float currentVertAngle = topAngle;
//    //vertical laser movement
//    while (currentVertAngle >= bottomAngle)
//    {
//        float currentHoriAngle = leftAngle;
//        //horizontal lase movement
//        while (currentHoriAngle < rightAngle)
//        {
//            getPointFromRay(currentHoriAngle, currentVertAngle, maxDist);
//            currentHoriAngle += horiIncrement;
//        }
//        currentVertAngle -= verticalIncrement;
//    }
//}

//public Vector3 getPointFromRay(float yAngle, float zAngle, float maxDist)
//{
//    Vector3 point = new Vector3(0, 0, 0);
//    //Vector3 direction =Quaternion.AngleAxis(zAngle-90, transform.forward)*droneBody.transform.localPosition;
//    //direction = Quaternion.AngleAxis(yAngle-90, transform.up) * direction;     
//    //Vector3 direction = Quaternion.Euler(0, yAngle - 90, zAngle - 90) * droneBody.transform.localPosition;
//    Vector3 direction = this.transform.right;
//    direction = Quaternion.AngleAxis(zAngle, transform.forward) * direction;
//    direction = Quaternion.AngleAxis(yAngle - 90, transform.up) * direction;
//    direction.Normalize();
//    //Debug.Log("zangle: " + zAngle.ToString()+" yangle: "+yAngle.ToString());
//    RaycastHit hit;        
//    if (Physics.Raycast(this.transform.position, direction, out hit, maxDist,1<<8))
//    {
//        point = hit.point;            
//        gizStack.Push(point);
//        //adjust for ros coordinate system
//        Vector3 rosPoint=new Vector3(point.x,point.z,point.y);            
//        pcl.pushPointToCloud(rosPoint);
//    }
//    //Debug.DrawRay(this.transform.position, direction * maxDist, new Color(254, 254, 254, 0.5f));
//    return point;
//}