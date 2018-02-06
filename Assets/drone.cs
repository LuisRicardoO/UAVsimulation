using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using controlNameSpace;
using contolVarsNamespace;
using functionNameSpace;

public class drone : MonoBehaviour
{

    private Rigidbody droneBody;
    
    //public Camera cam1;
    //public Camera cam2;

    private droneController c;
    private functions f = new functions();

    private Vector4 pidVelY = new Vector4(5, 4, 0, 50);
    private Vector4 pidVelX = new Vector4(5, 2.5f, 0, 10);
    private Vector4 pidVelZ = new Vector4(5, 2.5f, 0, 10);
    private Vector4 pidAngVelY = new Vector4(0.01f, 0, 0, 0);


    public Vector3 velRef;
    public float angVelRef;


    public float maxit;
    public float lerpValue;    



    void Start()
    {
        droneBody = this.GetComponent<Rigidbody>();
        c = new droneController(new controlVars(pidVelX), new controlVars(pidVelY), new controlVars(pidVelZ), new controlVars(pidAngVelY), Time.fixedDeltaTime);

        //cam1.enabled = true;
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            //cam1.enabled = !cam1.enabled;
            //cam2.enabled = !cam2.enabled;
        }
    }

    //void OnDrawGizmos()
    //{
    //    Vector3 paraLinVel = Quaternion.AngleAxis(-droneBody.rotation.eulerAngles.y, Vector3.up) * droneBody.velocity;

    //    Vector3 vx = droneBody.transform.TransformDirection(new Vector3(paraLinVel.x, 0, 0));
            
    //    Vector3 vy = droneBody.transform.TransformDirection(new Vector3(0, paraLinVel.y, 0));
    //    Vector3 vz = droneBody.transform.TransformDirection(new Vector3(0, 0, paraLinVel.z));

    //    DrawHelperAtCenter(vx, Color.red, 0.1f);
    //    DrawHelperAtCenter(vy, Color.green, 0.1f);
    //    DrawHelperAtCenter(vz, Color.blue, 0.1f);
    //    //Color color;
    //    //color = Color.green;
    //    //// local up
    //    //DrawHelperAtCenter(this.transform.up, color, 2f);

    //    //color.g -= 0.5f;
    //    //// global up
    //    //DrawHelperAtCenter(Vector3.up, color, 1f);

    //    //color = Color.blue;
    //    //// local forward
    //    //DrawHelperAtCenter(this.transform.forward, color, 2f);

    //    //color.b -= 0.5f;
    //    //// global forward
    //    //DrawHelperAtCenter(Vector3.forward, color, 1f);

    //    //color = Color.red;
    //    //// local right
    //    //DrawHelperAtCenter(this.transform.right, color, 2f);

    //    //color.r -= 0.5f;
    //    //// global right
    //    //DrawHelperAtCenter(Vector3.right, color, 1f);
    //}

    //private void DrawHelperAtCenter(
    //                   Vector3 direction, Color color, float scale)
    //{
    //    Gizmos.color = color;
    //    Vector3 destination = transform.position + direction * scale;
    //    Gizmos.DrawLine(transform.position, destination);
    //}
}
