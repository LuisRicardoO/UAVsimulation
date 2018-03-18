using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using System;
using System.Runtime.InteropServices;
using pclInterfaceName;
//using pcLib;

public class laserSensor : MonoBehaviour
{
    pclInterface pcl = new pclInterface();

    public string publishTopic = "drone/laserScan";
    public string frame_id = "laserFrame";

    private Rigidbody droneBody;

    private RosSocket rosSocket;
    private int publication_id;

    public bool run;

    void Start()
    {
        rosSocket = GameObject.Find("drone").GetComponent<RosConnector>().RosSocket;
        droneBody = GameObject.Find("drone").GetComponent<Rigidbody>();
        publication_id = rosSocket.Advertize(publishTopic, "sensor_msgs/PointCloud2");       
    }
    void Update()
    {
        if (run)
        {
            pcl.createPclCloud(0, 0, true);

            //get points
            //pushPointsToPcl()
            for (int i = 0; i < 20; i++)
            {
                pcl.pushPointToCloud(new Vector3(1, i, 1));
            }       
            
            
            
                 
            SensorPointCloud2 pc = new SensorPointCloud2();
            pcl.convertToRosMsgFromCloud(pc, frame_id);
            //Debug.Log("pcl cloud->"+pcl.readCloudParameters());
            //Debug.Log("Cloud2->" + pcl.readCloud2Parameters());
            Debug.Log("CloudR->" + pcl.readCloudRosParameters(pc));                        
            rosSocket.Publish(publication_id, pc);
        }
    }

    private void pushPointstoPcl()
    {

    }

}