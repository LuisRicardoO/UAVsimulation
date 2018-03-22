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
    public int repetitions;

    private laserRayCaster laser2 = new laserRayCaster();

    private rayCaster laser;

    pclInterface pcl = new pclInterface();
    SensorPointCloud2 pc = new SensorPointCloud2();

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

        pc = new SensorPointCloud2();
    }
    void Update()
    {
        laser2.defineParameters(maxLeftAngle, maxRightAngle, maxTopAngle, maxBottomAngle, verticalIncrement, horizontalIncrement, maxDistance, 1 << 8,repetitions);

        if (run)
        {            
            //this function is resposible for push the correct points to the pcl cloud
            if (laser2.runCasterSequentialRep(ref pcl, this, drawRay))
            {
                //convert from pcl cloud to pcl cloud2
                pcl.convertToCloud2();

                //create a new mensage to be send
                SensorPointCloud2 pc = new SensorPointCloud2();

                //from the Pcl cloud2 converted to Ros cloud2 using marshal for better performance
                pcl.convertToRosCloud(ref pc, frame_id);

                //publish the ros cloud mensage
                rosSocket.Publish(publication_id, pc);

                //restart pcl cloud
                pc = new SensorPointCloud2();
                pcl.createPclCloud(0, 0, true);
            }    
        }        
    }
}

/*
if (run)
        {            
            //this function is resposible for push the correct points to the pcl cloud
            if (laser2.runCasterSequentialRep2(ref pcl, this, drawRay,))
            {
    //convert from pcl cloud to pcl cloud2
    pcl.convertToCloud2();

    //create a new mensage to be send
    SensorPointCloud2 pc = new SensorPointCloud2();

    //from the Pcl cloud2 converted to Ros cloud2 using marshal for better performance
    pcl.copyToSensorPointCloud2Ptr(ref pc, frame_id);

    //publish the ros cloud mensage
    rosSocket.Publish(publication_id, pc);

    //restart pcl cloud
    pcl.createPclCloud(0, 0, true);
}
}
*/