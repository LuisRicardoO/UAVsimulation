using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pclInterfaceName;
using System.Runtime.InteropServices;
using RosSharp.RosBridgeClient;

public class test : MonoBehaviour
{

    pclInterface pcl = new pclInterface();

    // Use this for initialization
    void Start()
    {
        
        pcl.createPclCloud(0, 0, true);
        pcl.pushPointToCloud(new Vector3(1, 2, 3));        
        pcl.convertToCloud2();
        SensorPointCloud2 pc = new SensorPointCloud2();
        pcl.convertToRosCloud(ref pc,"ok");
        Debug.Log("data: " + pc.data[0] + " " + pc.data[1] + " " + pc.data[2] + " " + pc.data[3] + " " + pc.data[4]);        
        //byte[] data= new byte[0];
        //pcl.testByteArray222(ref data);
        //Debug.Log("data: "+data[0]+" "+data[1] + " " + data[2] + " " + data[3] + " " + data[4]);                                    
    }


}
