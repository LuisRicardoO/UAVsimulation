using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;

[RequireComponent(typeof(RosConnector))]

public class sensorIMU : MonoBehaviour {

    public string publishTopic = "drone/imu";

    private Rigidbody droneBody;

    private RosSocket rosSocket;
    private int publication_id;

    // Use this for initialization
    void Start () {
        rosSocket = GameObject.Find("drone").GetComponent<RosConnector>().RosSocket;
        droneBody = GameObject.Find("drone").GetComponent<Rigidbody>();
        publication_id = rosSocket.Advertize("drone/imu", "sensor_msgs/Imu");          
    }
	
	// Update is called once per frame
	void Update () {
        		
	}
}
