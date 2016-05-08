using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Carnival;

public class FingertipSampleBehaviour : MonoBehaviour
{
	public GameObject hammerPrefab;
	// A hammer object with collider to hit moles

	private Controller _carnivalController;
	// Controller to access frame data

	// Use this for initialization
	void Start()
	{
		// Initialize controller
		Debug.Log("FingertipSampleBehaviour: Initializing sensor");
		_carnivalController = new Controller();
		_carnivalController.Init();
		_carnivalController.Start();

		//Bring keys back up every second
		// Repeat method every 1 seconds
		InvokeRepeating("showRandomMoles", .1F, .1F);
	}

	// Update is called once per frame
	void Update()
	{
		foreach(GameObject hammer in GameObject.FindGameObjectsWithTag("hammer"))
		{
			Destroy(hammer);
		}

		// Get the current frame
		Frame frame = _carnivalController.Frame();

		// Fingertips are always linked to a certain Hand.
		foreach(Hand hand in frame.Hands)
		{
			foreach(Fingertip tip in hand.Fingertips)
			{
				GameObject hammer = Instantiate(hammerPrefab) as GameObject;
				hammer.transform.parent = Camera.main.transform;

				// 3D loaction of fingertip is relative position to sensor. Here we keep it simple since sensor is mounted
				// quite close to your eyes which is main camera. For better UX you should actually take the distance into
				// account
				hammer.transform.localPosition = tip.Center3D;
			}
		}
	}

	// Randomly show moles above the ground
	void showRandomMoles()
	{
		GameObject[] moles = GameObject.FindGameObjectsWithTag("underGround");
		System.Random rand = new System.Random();


		foreach(GameObject mole in moles)
		{
//			// 50% chance to move moles beneath ground upwards
			if(rand.NextDouble() >= 0.5)
			{
				mole.GetComponent<Rigidbody>().useGravity = false;
				//Vector3 temp = new Vector3(0,1.0f,0);
				//mole.transform.position += temp;


				mole.GetComponent<Rigidbody>().velocity = Vector3.up;
				gameObject.tag = "movingUp";
			}
//			//it's not called "hackathon" for nothing
//			//....
//			else{
//				mole.GetComponent<Rigidbody>().useGravity = false;
//				mole.GetComponent<Rigidbody>().velocity = Vector3.up;
//				gameObject.tag = "movingUp";
//			}
		}
	}

	void OnDestroy()
	{
		Debug.Log("FingertipSampleBehaviour: OnDestroy");
		_carnivalController.Stop();
	}

	void OnApplicationPause(bool pauseStatus)
	{
		if(_carnivalController == null)
		{
			return;
		}

		Debug.Log("FingertipSampleBehaviour: OnApplicationPause -> " + pauseStatus);

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
