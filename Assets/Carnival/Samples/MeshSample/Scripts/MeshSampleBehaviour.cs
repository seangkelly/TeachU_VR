using UnityEngine;
using System.Collections;
using Carnival;

public class MeshSampleBehaviour : MonoBehaviour
{
	public MeshRenderer hintImageRenderer;
	public MeshFilter handMesh;
	// Mesh that can be updated at real time via Carnival SDK

	private Controller _carnivalController;
	// Controller to access frame data
	private Texture[] _hintImages;
	// Hint images with hand shadows
	private int _imageCounter = 0;

	// Use this for initialization
	void Start()
	{
		// Initialize controller
		Debug.Log("MeshSampleBehaviour: Initializing sensor");
		_carnivalController = new Controller();
		//_carnivalController.Init();
		_carnivalController.Start();

		// Vertices positions of mesh is relative to sensor, which is docked with main camera here
		//handMesh.gameObject.transform.parent = Camera.main.transform;

		// Load all hint image sprites
		_hintImages = Resources.LoadAll<Texture>("ShadowImages");

		// Repeat method every 10 seconds
		InvokeRepeating("nextHintImage", 0, 10F);
	}

	// Update is called once per frame
	void Update()
	{
		// Get the current frame
		Frame frame = _carnivalController.Frame();

		//        if(Time.frameCount < 10)
		//        {
		//            for(int i = 5000; i < 5500; ++i)
		//            {
		//                Debug.Log(frame.DepthConfidence[i]);
		//            }
		//        }
		//return;

		// In this sample we used a slightly changed standard material that is opaque so that it can cast shadow.
		// Note that shadow quality is also dependant on quality settings for current platform. Of course you
		// can use your own cooler material to have a beautiful visualization of mesh.
		handMesh.mesh.Clear();

		if(frame.meshData.vertices.Length > 0)
		{
			// You need vertices positions and a triangle indicies array to render mesh in real time
			handMesh.mesh.vertices = frame.meshData.vertices;
			handMesh.mesh.triangles = frame.meshData.triangles;
			handMesh.mesh.RecalculateNormals();
		}

		// Back button on Android is mapped to Escape
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			_carnivalController.Stop();
			Application.Quit();
		}
	}

	void nextHintImage()
	{
		hintImageRenderer.material.SetTexture("_MainTex", _hintImages[_imageCounter % _hintImages.Length]);
		_imageCounter++;
	}

	void OnDestroy()
	{
		Debug.Log("MeshSampleBehaviour: OnDestroy");
		_carnivalController.Stop();
	}

	void OnApplicationPause(bool pauseStatus)
	{
		if(_carnivalController == null)
		{
			return;
		}

		Debug.Log("MeshSampleBehaviour: OnApplicationPause -> " + pauseStatus);

		if(pauseStatus)
		{
			_carnivalController.Stop();
		}
		else
		{
			_carnivalController.Start();
		}
	}
}
