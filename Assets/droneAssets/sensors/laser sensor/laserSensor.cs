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
        //unsafe
        //{
        //    byte[] ok = { 1, 2 };
        //    fixed (byte* ptr = &ok[0])
        //    {
        //        byte[] apresentar = new byte[2];
        //        int size = 2;
        //        for (int i = 0; i < size; i++)
        //        {
        //            apresentar[0] = ptr[0];
        //            apresentar[1] = ptr[1];

        //        }

        //        Debug.LogWarning("aqui os byets |"+apresentar[0]+"|"+ apresentar[1]+"|"+(byte)10);
        //    }
        //}

        rosSocket = GameObject.Find("drone").GetComponent<RosConnector>().RosSocket;
        droneBody = GameObject.Find("drone").GetComponent<Rigidbody>();
        publication_id = rosSocket.Advertize(publishTopic, "sensor_msgs/PointCloud2");

        //pcl.createPclCloud(0, 0, true);
        //Debug.Log("Cloud->" + pcl.readCloudParameters());
        //pcl.pushPointToCloud(new Vector3(1, 2, 3));
        //pcl.pushPointToCloud(new Vector3(4, 5.875685f, 6));
        //pcl.pushPointToCloud(new Vector3(7, 8, 9));
        //pcl.pushPointToCloud(new Vector3(10, 11, 12));
        //Debug.Log("after adding points cloud->" + pcl.readCloudParameters());
        //SensorPointCloud2 pc = new SensorPointCloud2();
        //pcl.convertToRosMsgFromCloud(pc, "sensorLaserFrame");
        //Debug.Log("Cloud2->" + pcl.readCloud2Parameters());
        //Debug.Log("CloudR->" + pcl.readCloudRosParameters(pc));
    }
    void Update()
    {
        if (run)
        {
            pcl.createPclCloud(0, 0, true);
            pcl.pushPointToCloud(new Vector3(112, 12, 33));
            //pcl.pushPointToCloud(new Vector3(4, 5, 6));
            //pcl.pushPointToCloud(new Vector3(7, 8, 9));
            SensorPointCloud2 pc = new SensorPointCloud2();
            pcl.convertToRosMsgFromCloud(pc, frame_id);
            //Debug.Log("Cloud2->" + pcl.readCloud2Parameters());
            Debug.Log("CloudR->" + pcl.readCloudRosParameters(pc));
            pcl.compareDataCloud2andCloudRos(pc);
            rosSocket.Publish(publication_id, pc);
        }
    }

    //   [DllImport("pcLibUn", EntryPoint = "okokok")]
    //   public static extern int adding(int a, int b);

    //   [DllImport("pcLibUn", CharSet = CharSet.Unicode)]
    //   static extern IntPtr createrConnectorClass(int ID);

    //   [DllImport("pcLibUn", CharSet = CharSet.Unicode)]
    //   static extern IntPtr retornaAtributo(IntPtr api);

    //   public bool run;
    //   public string publishTopic = "drone/imu";
    //   public string frame_id;
    //   private Rigidbody droneBody;
    //   private RosSocket rosSocket;
    //   private int publication_id;

    //   private SensorPointCloud2 sensor;
    //   private SensorPointField[] pointField;

    //   public int height;
    //   public int width;


    //   // Use this for initialization
    //   void Start () {
    //       rosSocket = GameObject.Find("drone").GetComponent<RosConnector>().RosSocket;
    //       droneBody = GameObject.Find("drone").GetComponent<Rigidbody>();
    //       publication_id = rosSocket.Advertize("drone/pointcloud2", "sensor_msgs/PointCloud2");

    //       //pointCloudCreation ok = new pointCloudCreation(25);
    //       //Debug.LogWarning("aqui tens "+ adding(2,3));
    //       IntPtr shared = createrConnectorClass(456);
    //       Debug.LogWarning("fdx ganda bomba " + retornaAtributo(shared));
    //   }

    //// Update is called once per frame
    //void Update () {

    //       if (run)
    //       {
    //           updatePointcloud();
    //       }
    //}

    //   private SensorPointCloud2 updatePointcloud()
    //   {
    //       SensorPointCloud2 pc = new SensorPointCloud2();
    //       pc.header.frame_id = frame_id;
    //       pc.height = height;
    //       pc.width = width;

    //       //fields
    //       updatePointField();
    //       pc.fields = pointField;
    //       pc.is_bigendian = false;




    //       return pc;
    //   }

    //   /// <summary>
    //   /// constructs point field with new points from enviroment
    //   /// </summary>
    //   private void updatePointField()
    //   {

    //   }
}