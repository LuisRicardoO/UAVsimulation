using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using controlNameSpace;
using contolVarsNamespace;
using functionNameSpace;
using RosSharp.RosBridgeClient;

//[RequireComponent(typeof(RosConnector))]
public class drone : MonoBehaviour
{
	//ros connection
	RosSocket rosSocket;
	int publication_id;


	private Rigidbody droneBody;
    
	//public Camera cam1;
	//public Camera cam2;

	private droneController c;
	private functions f = new functions ();

	private Vector4 pidVelY = new Vector4 (5, 4, 0, 50);
	private Vector4 pidVelX = new Vector4 (5, 2.5f, 0, 10);
	private Vector4 pidVelZ = new Vector4 (5, 2.5f, 0, 10);
	private Vector4 pidAngVelY = new Vector4 (0.01f, 0, 0, 0);


	public Vector3 velRef;
	public float angVelRef;


	public float maxit;
	public float lerpValue;



    void Start()
    {
        //ros connection
        //rosSocket = new RosSocket("ws://192.168.1.10:9090");
        rosSocket = transform.GetComponent<RosConnector>().RosSocket;

        publication_id = rosSocket.Advertize("/drone/pos", "geometry_msgs/Pose");

        droneBody = this.GetComponent<Rigidbody>();
        c = new droneController(new controlVars(pidVelX), new controlVars(pidVelY), new controlVars(pidVelZ), new controlVars(pidAngVelY), Time.fixedDeltaTime);

		//cam1.enabled = false;
        //cam2.enabled = true;
    }

    //Update is called once per frame
    void FixedUpdate()
    {

        c.defineControlVarsSimplified(new controlVars(pidVelX), new controlVars(pidVelY), new controlVars(pidVelZ), new controlVars(pidAngVelY));

        //run controller
        Vector4 output = c.controlByForce2(velRef, angVelRef, droneBody);
        Vector3 force = f.copyVector4toVector3(output);
        float torque = output.w;

        //limit max force
        Vector3 minForce = new Vector3(-5, 0, -5);
        Vector3 maxForce = new Vector3(5, 15, 5);

        force = f.vlimitv(force, minForce, maxForce);
        torque = f.limit(torque, -2, 2);

        droneBody.AddForce(force);
        droneBody.AddRelativeTorque(new Vector3(0, torque, 0));

        c.visualRepresentationSimplified(force, 1, 0.1f, droneBody, minForce, maxForce, new Vector3(-30,0,-30), new Vector3(30,30,30));

        //publishToRos();

        if (Input.GetKeyDown(KeyCode.C))
        {
            //cam1.enabled = !cam1.enabled;
            //cam2.enabled = !cam2.enabled;
        }
    }

    private void publishToRos()
    {
        publishPositionAndRotation();
    }

    private void publishPositionAndRotation()
    {        
        GeometryPose msg = new GeometryPose();
        msg.position.x = droneBody.transform.position.x;
        msg.position.y = droneBody.transform.position.y;
        msg.position.z = droneBody.transform.position.z;

        msg.orientation.x = droneBody.transform.rotation.x;
        msg.orientation.y = droneBody.transform.rotation.y;
        msg.orientation.z = droneBody.transform.rotation.z;
        msg.orientation.w = droneBody.transform.rotation.w;

        rosSocket.Publish(publication_id, msg);
    }  
}
