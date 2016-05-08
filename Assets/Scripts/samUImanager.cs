using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class samUImanager : MonoBehaviour {

	public Image[] images;

	private bool one;
	private bool two;
	private bool three;
	private bool four;

	// Use this for initialization
	void Start () {
		images [0].color = Color.black;
		images [1].color = Color.black;
		images [2].color = Color.black;
		images [3].color = Color.black;
		images [4].color = Color.black;
		images [5].color = Color.black;
		images [6].color = Color.black;

	}
	
	// Update is called once per frame
	void Update () {

		if (DrumBehavior.snareCounter >= 1) {
			images [0].color = Color.white;

		}

		if (DrumBehavior.snareCounter >= 1 && DrumBehavior.tomCounter >= 1) {
			images [1].color = Color.white;
			
		}

		if (DrumBehavior.snareCounter >= 2 && DrumBehavior.tomCounter >= 1) {
			images [2].color = Color.white;
			
		}

		if (DrumBehavior.snareCounter >= 1 && DrumBehavior.tomCounter >= 2) {
			images [3].color = Color.white;
			
		}

		if (DrumBehavior.snareCounter >= 3 && DrumBehavior.tomCounter >= 2) {
			images [4].color = Color.white;
		}

		if (DrumBehavior.snareCounter >= 4 && DrumBehavior.tomCounter >= 2) {
			images [5].color = Color.white;
		}

		if (DrumBehavior.snareCounter >= 4 && DrumBehavior.tomCounter >= 3) {
			images [6].color = Color.white;
		}
	
	}
}
