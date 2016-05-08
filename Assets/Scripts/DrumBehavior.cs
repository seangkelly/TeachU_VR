using UnityEngine;
using System.Collections;

public class DrumBehavior : MonoBehaviour
{
	public AudioSource audioSource;
	public AudioClip tom;
	public AudioClip snare;

	public static int tomCounter;
	public static int snareCounter;

	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		
	}

	void OnTriggerEnter(Collider col)
	{
		Debug.Log("Drum collider entered");
		//// TOM
		if (!audioSource.isPlaying && gameObject.tag == "tom") {
			audioSource.PlayOneShot (tom);
			tomCounter++;
		} 

		//// SNARE
		if (!audioSource.isPlaying && gameObject.tag == "snare") {
			audioSource.PlayOneShot (snare);
			snareCounter++;

		} 
	}

	IEnumerator waitAndPlay(float waitTime){
		yield return new WaitForSeconds (waitTime);

	}
		

}
