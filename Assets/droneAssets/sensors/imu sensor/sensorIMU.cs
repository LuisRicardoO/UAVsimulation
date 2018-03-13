using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;

public class sensorIMU : MonoBehaviour
{
    public bool run;

    public string publishTopic = "drone/imu";

    public string frame_id;

    private Rigidbody droneBody;

    private RosSocket rosSocket;
    private int publication_id;

    private Imu sensor;

    private Vector3 lastLinearVel=new Vector3(0,0,0);

    // Use this for initialization
    void Start()
    {
        rosSocket = GameObject.Find("drone").GetComponent<RosConnector>().RosSocket;
        droneBody = GameObject.Find("drone").GetComponent<Rigidbody>();
        publication_id = rosSocket.Advertize("drone/imu", "sensor_msgs/Imu");
    }

    // Update is called once per frame
    void Update()
    {
        if (run)
        {
            //get Imuvalue
            sensor = updateImu();
            rosSocket.Publish(publication_id, sensor);
        }
    }

    private Imu updateImu()
    {
        Imu sensorImu = new Imu();

        //setup frame
        sensorImu.header.frame_id = frame_id;

        //setting covarience unknown
        float[] matrixUnknown = { 1, 0, 0, 0, 1, 0, 0, 0, 1 };
        sensorImu.orientation_covariance = matrixUnknown;
        sensorImu.angular_velocity_covariance = matrixUnknown;
        sensorImu.linear_acceleration_covariance = matrixUnknown;

        //get from drone rotation, velocity, accelaration
        sensorImu.orientation.x = droneBody.rotation.x;
        sensorImu.orientation.y = droneBody.rotation.y;
        sensorImu.orientation.z = droneBody.rotation.z;
        sensorImu.orientation.w = droneBody.rotation.w;

        sensorImu.angular_velocity.x = droneBody.angularVelocity.x;
        sensorImu.angular_velocity.y = droneBody.angularVelocity.y;
        sensorImu.angular_velocity.z = droneBody.angularVelocity.z;

        Vector3 droneAcc = calculateAcc(lastLinearVel, droneBody.velocity, Time.deltaTime);
        sensorImu.linear_acceleration.x = droneAcc.x;
        sensorImu.linear_acceleration.y = droneAcc.y;
        sensorImu.linear_acceleration.z = droneAcc.z;
        //Debug.Log("acc"+droneAcc);
        lastLinearVel = droneBody.velocity;
        return sensorImu;
    }

    private static Vector3 calculateAcc(Vector3 lastVel, Vector3 newVel, float elapsedTime)
    {
        Vector3 acc = new Vector3(0,0,0);
        acc.x = (lastVel.x - newVel.x) / elapsedTime;
        acc.y = (lastVel.y - newVel.y) / elapsedTime;
        acc.z = (lastVel.z - newVel.z) / elapsedTime;
        return acc;
    }


}
