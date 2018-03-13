using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;

public class gpssensor : MonoBehaviour
{
    public bool run;

    private NavSatFix sensor;
    public string frameId;
    public float startingLatitude;
    public float startingLongitude;
    public float startingAltitude;

    private Rigidbody droneBody;
    private RosSocket rosSocket;
    private int publication_id;

    // Use this for initialization
    void Start()
    {
        rosSocket = GameObject.Find("drone").GetComponent<RosConnector>().RosSocket;
        droneBody = GameObject.Find("drone").GetComponent<Rigidbody>();
        publication_id = rosSocket.Advertize("drone/gps", "sensor_msgs/NavSatFix");
    }


    // Update is called once per frame
    void Update()
    {
        if (run)
        {
            sensor = updateGPS();
            rosSocket.Publish(publication_id, sensor);
        }
    }

    private NavSatFix updateGPS()
    {
        NavSatFix navFix = new NavSatFix();
        navFix.header.frame_id = frameId;
        navFix.status.status = 0;
        navFix.status.service = 1;
        navFix.position_covariance_type = 0;
        float[] matrixUnknown = { 1, 0, 0, 0, 1, 0, 0, 0, 1 };
        navFix.position_covariance = matrixUnknown;

        //TODO get and convert UTM to GPS

        return navFix;
    }
}
