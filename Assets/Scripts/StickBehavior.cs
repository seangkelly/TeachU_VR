using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Carnival;

public class StickBehavior : MonoBehaviour
{
	public GameObject stickPrefab;
	// A stick object with collider to hit moles

	private List<GameObject> sticks;

	private Controller _carnivalController;

	private GameObject debugHand;
	// Controller to access frame data

	// Use this for initialization
	void Start()
	{
		// Initialize controller
		Debug.Log("FingertipSampleBehaviour: Initializing sensor");
		_carnivalController = new Controller();
		_carnivalController.Init();
		_carnivalController.Start();

		sticks = new List<GameObject> ();

		//debugHand = GameObject.Find ("debug hand");
		//debugHand.transform.SetParent (Camera.main.transform);

	}

	// Update is called once per frame
	void Update()
	{
		foreach(GameObject stick in sticks)
		{
			stick.SetActive (false);

		}

		// Get the current frame
		Frame frame = _carnivalController.Frame();

		//if (frame.Hands.Count > 0) {
		//	debugHand.transform.localPosition = frame.Hands [0].CenterOfGravity;
		//}

		int sticksNeeded = 0;
		foreach (Hand hand in frame.Hands)
		{

			if (hand.Fingertips.Count > 0) {
				sticksNeeded++;
			}


			Debug.Log ("sticks needed: " + sticksNeeded);

		}

		for (int i = sticks.Count; i < sticksNeeded; i++) 
		{
			GameObject stick = Instantiate(stickPrefab) as GameObject;
			sticks.Add (stick);
		}

		int currentStick = 0;
		foreach (Hand hand in frame.Hands) {
			foreach (Fingertip tip in hand.Fingertips) {

				GameObject stick = sticks [currentStick];
				stick.SetActive (true);
				//currentStick++;


				stick.transform.parent = Camera.main.transform;
				//stick.transform.SetParent (Camera.main.transform, false);

				//stick.transform.localPosition = hand.CenterOfGravity;
				//stick.transform.LookAt (tip.Center3D);

				stick.transform.localPosition = tip.Center3D;


			}
		}
	}

	void OnDestroy()
	{

		if (_carnivalController == null) {
			return;
		}
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