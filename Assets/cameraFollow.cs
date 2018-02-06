using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour {


    public Transform target;
    public Vector3 offset;
	// Update is called once per frame
	void Update () {
		//Debug.LogWarning(transform.position);

		Vector3 desiredPosition = target.position + offset;		
		transform.position = desiredPosition;
        transform.LookAt(target.transform);

    }
}
