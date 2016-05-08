using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Carnival;

public class HandSampleBehaviour : MonoBehaviour
{
	public GameObject baseballPrefab;

	// Thrown baseballs disappear after 5 seconds
	private const float DestroyDelay = 5.0f;
	// In this example we use 20% of openness as threshold for a fist
	private const float HandCloseThreshold = .2f;
	// In this example we use 90% of openness as threshold for a fist
	private const float HandOpenThreshold = .8f;
	// Force which is applied to the baseball
	private const float Force = 500f;

	private GameObject _baseball;
	private Controller _carnivalController;
	// Controller to access frame data
	// For better UX origin is set slightly lower than zero so that the player don't always have to hold their hand high
	private Vector3 _origin = new Vector3(0f, -0.1f, 0f);

	// Use this for initialization
	void Start()
	{
		// Initialize controller
		Debug.Log("HandSampleBehaviour: Initializing sensor");
		_carnivalController = new Controller();
		_carnivalController.Init();
		_carnivalController.Start();
	}

	// Update is called once per frame
	void Update()
	{
		// Get the current frame
		Frame frame = _carnivalController.Frame();
		// Abort if no hands are detected and destroy all balls that are ready to be thrown
		if(frame.Hands.Count == 0)
		{
			GameObject[] balls = GameObject.FindGameObjectsWithTag("ready");
			foreach(GameObject ball in balls)
			{
				GameObject.Destroy(ball);
			}
			return;
		}

		// CarnivalSDK provides data for all detected hands, but we use only the first detected hand in this example
		// The hand position is measured from the sensor point of view, which means it is not a world position.
		// Use the "LinkToMainCamera" script or make your root object a child if the main camera to align to the field of view.
		// Since the sensor is mounted a little higher above the players eyes, we calculate the direction vector
		// a bit lower than the sensor origin.

		// Prepare new baseball if hand is closed and no ball is ready
		if(frame.Hands[0].Openness <= HandCloseThreshold)
		{
			if(GameObject.FindGameObjectsWithTag("ready").Length == 0)
			{
				// Instantiate baseball at hand center position
				_baseball = Instantiate(baseballPrefab) as GameObject;
				_baseball.transform.parent = Camera.main.transform;
				_baseball.tag = "ready";
			}
			_baseball.transform.localPosition = frame.Hands[0].CenterOfGravity;
		}

        // Update ball position while player keep hand closed and is moving his fist
        else
		if(frame.Hands[0].Openness < HandOpenThreshold && GameObject.FindGameObjectsWithTag("ready").Length > 0)
		{
			_baseball.transform.localPosition = frame.Hands[0].CenterOfGravity;
		}

        // Throw baseball with a constant force in the hand direction if hand is open and ball is prepared
        else
		if((frame.Hands[0].Openness >= HandOpenThreshold) && GameObject.FindGameObjectsWithTag("ready").Length > 0)
		{
			_baseball.GetComponent<Rigidbody>().AddForce(Camera.main.transform.rotation *
			(_baseball.transform.localPosition - _origin).normalized *
			Force);
			_baseball.GetComponent<Rigidbody>().useGravity = true;
			_baseball.tag = "thrown";
			Destroy(_baseball, DestroyDelay);
		}
	}

	void OnDestroy()
	{
		Debug.Log("HandSampleBehaviour: OnDestroy");
		_carnivalController.Stop();
	}

	void OnApplicationPause(bool pauseStatus)
	{
		if(_carnivalController == null)
		{
			return;
		}

		Debug.Log("HandSampleBehaviour: OnApplicationPause -> " + pauseStatus);

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
