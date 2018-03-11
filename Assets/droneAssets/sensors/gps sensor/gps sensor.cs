using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;

public class gpssensor : MonoBehaviour {

    public string frameId;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        NavSatFix navFix = new NavSatFix();
        navFix.header.frame_id = frameId;
        navFix.status.status = 0;
        navFix.status.service = 1;      
	}
}
