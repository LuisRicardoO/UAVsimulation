using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;

[RequireComponent(typeof(RosConnector))]
public class sensorCamera : MonoBehaviour {

    private CameraImagePublisher2 toPublish;
    public string publishTopic;
    [Range(0, 100)]
    public int qualityLevel = 50;

    private int resolutionWidth;
    private int resolutionHeight;

    private Texture2D texture2D;
    //private RenderTexture renderTexture;
    private Rect rect;

    private Camera _camera;

    void Start()
    {
        //_camera = transform.parent.parent.GetComponent<RosConnector>().RosSocket;_camera = transform.parent.parent.GetComponent<RosConnector>().RosSocket;
        //find the rosconnector used in the drone. it should only exist one connection (i think)
        toPublish = new CameraImagePublisher2(GameObject.Find("drone").GetComponent<RosConnector>().RosSocket,publishTopic,qualityLevel);
        _camera = GetComponent<Camera>();
        resolutionHeight = _camera.targetTexture.height;
        resolutionWidth = _camera.targetTexture.width;

        texture2D = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
        rect = new Rect(0, 0, resolutionWidth, resolutionHeight);
       // renderTexture = new RenderTexture(resolutionWidth, resolutionHeight, 24);
    }

    void Update()
    {
        // get the camera's render texture
        //first safe actual rexnder texture=rendtexture
        RenderTexture rendText = RenderTexture.active;
        //then set the render texture to render the camera target texture
        RenderTexture.active = _camera.targetTexture;

        // render the texture
        _camera.Render();
                
        //get the above rendered image to a texture
        texture2D.ReadPixels(rect, 0, 0);
        texture2D.Apply();

        //restore the other rendertexture
        RenderTexture.active = rendText;

        //publish texture
        toPublish.Publish(texture2D);
    }
}