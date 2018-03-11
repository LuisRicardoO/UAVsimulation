using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;

/// <summary>
/// Sensor Camera renders image on the update() function.
/// the image is ther on OnRenderImage copied into two different texture.
/// On texture gives an normal RGB image and the other o depth image.
/// Those render Texture are then converted to texture2D and sent 
/// to ros by cameraImagePublish2 class
/// </summary>
public class sensorCamera : MonoBehaviour
{

	private CameraImagePublisher2 toPublish;
	// -> /drone/image/compressed
	private CameraImagePublisher2 toPublishDepth;
	public string publishTopic;
	public string depthPublishTopic;

	[Range (0, 100)]
	public int qualityLevel;

	private int resolutionWidth;
	private int resolutionHeight;

	public RenderTexture render1;
	private Texture2D tex1;
	public RenderTexture render2;
	private Texture2D tex2;
    

	private Rect rect;

	private Camera _camera;

	//for depth image
	public Material mat;

	void Start ()
	{
		//_camera = transform.parent.parent.GetComponent<RosConnector>().RosSocket;_camera = transform.parent.parent.GetComponent<RosConnector>().RosSocket;
		//find the rosconnector used in the drone. it should only exist one connection (i think)
		toPublish = new CameraImagePublisher2 (GameObject.Find ("drone").GetComponent<RosConnector> ().RosSocket, publishTopic, qualityLevel);
		toPublishDepth = new CameraImagePublisher2 (GameObject.Find ("drone").GetComponent<RosConnector> ().RosSocket, depthPublishTopic, qualityLevel);

		_camera = this.GetComponent<Camera> ();
		resolutionHeight = _camera.targetTexture.height;
		resolutionWidth = _camera.targetTexture.width;
		rect = new Rect (0, 0, resolutionWidth, resolutionHeight);

		tex1=new Texture2D (resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
		tex2=new Texture2D (resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
		//texture2D = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);	        
		//depthTexture2D = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);	


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
		//RenderTexture depthRenderTexture = new RenderTexture(resolutionWidth,resolutionHeight,16);
		//Graphics.Blit(_camera.targetTexture, depthRenderTexture, mat);

	
		//depthTexture2D.ReadPixels (rect, 0, 0);
		//depthTexture2D.Apply();
        //restore the other rendertexture
        RenderTexture.active = rendText;


		toPublish.Publish(texture2D);
        //publish texture
        //toPublish.Publish(texture2D);
		//toPublishDepth.Publish(depthTexture2D);
    }
    */

	void Update ()
	{
		// get the camera's render texture
		//first safe actual rexnder texture=rendtexture
		RenderTexture actText = RenderTexture.active;
		//then set the render texture to render the camera target texture
		RenderTexture.active = _camera.targetTexture;

		// render the texture
		_camera.Render ();                

		RenderTexture.active = render1;
		tex1.ReadPixels (rect, 0, 0);
		tex1.Apply ();

		RenderTexture.active = render2;
		tex2.ReadPixels (rect, 0, 0);
		tex2.Apply ();
			
		RenderTexture.active = actText;
			
		toPublish.Publish (tex1);
		toPublishDepth.Publish (tex2);
	}

	/*void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, mat);
		//mat is the material which contains the shader
		//we are passing the destination RenderTexture to
	}*/

	void OnRenderImage (RenderTexture src, RenderTexture dest)
	{
		// This is the default "passthrough" output of the camera
		//Graphics.Blit(src, dest);
		// Also copy output to any additional rendertextures
		Graphics.Blit (null, render1);
		Graphics.Blit (null, render2, mat);
	}

	/*void OnpostRender(){
		tex1=toTexture2D (render1);
		tex2=toTexture2D (render2);
	}
	*/

	/*
	Texture2D toTexture2D (RenderTexture rTex)
	{
		Texture2D tex = new Texture2D (resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
		RenderTexture.active = rTex;
		tex.ReadPixels (rect, 0, 0);
		tex.Apply ();
		return tex;
	}
	*/

}