using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;

public class sensorCameraDepth : MonoBehaviour {

	private CameraImagePublisher2 toPublish;   // -> /drone/image/compressed
	//private CameraImagePublisher2 toPublishDepth;
	public string publishTopic;
	//public string depthPublishTopic;

	[Range(0, 100)]
	public int qualityLevel = 50;

	private int resolutionWidth;
	private int resolutionHeight;

	private Texture2D texture2D; 
	//public Texture2D depthTexture2D;

	private Rect rect;

	private Camera _camera;

	//for depth image
	public Material mat;

	void Start()
	{
		//_camera = transform.parent.parent.GetComponent<RosConnector>().RosSocket;_camera = transform.parent.parent.GetComponent<RosConnector>().RosSocket;
		//find the rosconnector used in the drone. it should only exist one connection (i think)
		toPublish = new CameraImagePublisher2(GameObject.Find("drone").GetComponent<RosConnector>().RosSocket,publishTopic,qualityLevel);
		//toPublishDepth = new CameraImagePublisher2(GameObject.Find("drone").GetComponent<RosConnector>().RosSocket,depthPublishTopic,qualityLevel);

		_camera = this.GetComponent<Camera>();
		resolutionHeight = _camera.targetTexture.height;
		resolutionWidth = _camera.targetTexture.width;


		texture2D = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);	        
		//depthTexture2D = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);	
		rect = new Rect(0, 0, resolutionWidth, resolutionHeight);

		//for depth image
		_camera.depthTextureMode = DepthTextureMode.Depth; 
	}

	/*
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

		//get the depth version of same render texture by appliyng a depth shader
		RenderTexture depthRenderTexture = new RenderTexture(resolutionWidth,resolutionHeight,16);
		Graphics.Blit(_camera.targetTexture, depthRenderTexture, mat);


		//depthTexture2D.ReadPixels (rect, 0, 0);
		//depthTexture2D.Apply();
		//restore the other rendertexture
		RenderTexture.active = rendText;

		//publish texture
		toPublish.Publish(texture2D);
		//toPublishDepth.Publish(depthTexture2D);
	}
	 */


	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//convert to depth
		Graphics.Blit(source, destination, mat);

		texture2D.ReadPixels(rect, 0, 0);
		texture2D.Apply();  
		toPublish.Publish(texture2D);
		//mat is the material which contains the shader
		//we are passing the destination RenderTexture to
	}
}