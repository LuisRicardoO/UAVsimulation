using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class pointcloudTest : MonoBehaviour
{

    [DllImport("pclUnity", CharSet = CharSet.Unicode)]
    static extern IntPtr pclConnectionConstructor();

    [DllImport("pclUnity", CharSet = CharSet.Unicode)]
    static extern int get(IntPtr api);

    [DllImport("pclUnity", CharSet = CharSet.Unicode)]
    static extern void setCloudWidth(IntPtr api, float width);

    [DllImport("pclUnity", CharSet = CharSet.Unicode)]
    static extern float getCloudWidth(IntPtr api);

    [DllImport("pclUnity", CharSet = CharSet.Unicode)]
    static extern void setCloudHeight(IntPtr api, float height);

    [DllImport("pclUnity", CharSet = CharSet.Unicode)]
    static extern float getCloudHeight(IntPtr api);

    [DllImport("pclUnity", CharSet = CharSet.Unicode)]
    static extern void setPointAt(IntPtr api, float x, float y, float z, int at);

    [DllImport("pclUnity", CharSet = CharSet.Unicode)]
    static extern float[] getPointAt(IntPtr api, int at);

    [DllImport("pclUnity", CharSet = CharSet.Unicode)]
    static extern float getPointAtX(IntPtr api, int at);

    [DllImport("pclUnity", CharSet = CharSet.Unicode)]
    static extern void pushPoint(IntPtr api, float x, float y, float z);

    [DllImport("pclUnity", CharSet = CharSet.Unicode)]
    static extern void setCloudDense(IntPtr api, bool dense);

    [DllImport("pclUnity", CharSet = CharSet.Unicode)]
    static extern float getCloudDense(IntPtr api);


    // Use this for initialization
    void Start()
    {
        IntPtr shared = pclConnectionConstructor();
        Debug.Log("Mega teste:************************");
        Debug.Log("1: set '10' e get the width and height");
        setCloudWidth(shared, 2);
        setCloudHeight(shared, 2);
        Debug.Log("r: " + getCloudWidth(shared) + " and " + getCloudHeight(shared));
        Debug.Log("2: set and get X point");
        pushPoint(shared, 29, 2, 3);
        Debug.Log("r: " + getPointAtX(shared, 0));





        //Debug.LogWarning("fdx ganda bomba " + get(shared));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
