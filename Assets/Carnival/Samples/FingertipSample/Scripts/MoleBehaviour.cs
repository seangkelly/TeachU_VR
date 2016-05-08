using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MoleBehaviour : MonoBehaviour
{
	 float _initY;

	public  AudioClip impact;
	AudioSource audio;

	public PlayMusic other;

	public string notename;
	public Text textref;

    void Start()
    {


        // Remember initial position
        _initY = transform.position.y;

		audio = GetComponent<AudioSource>();
		//gameObject.GetComponent<Renderer> ().material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        // Tag mole object according to its position

        // Arrived its initial position?
        if (transform.position.y >= _initY)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.tag = "aboveGround";

			//test this
			//Debug.Log("going underground");
        }

//        // Underneath ground?
//		if (GetComponent<BoxCollider>().bounds.max.y < 0)
//        {
//            gameObject.tag = "underGround";
//        }
    }

    void OnTriggerEnter(Collider col)
    {
		
        if (gameObject.tag == "aboveGround")
        {
			audio.PlayOneShot(impact, 0.7F);

			other.gameObject.GetComponent("PlayMusic");
			other.PlaySound(impact);
			textref.text = notename;

			//test color changing on collider enter
			//gameObject.GetComponent<Renderer> ().material.color = Color.blue;

            // Mole falls down when it is hit by collider
            GetComponent<Rigidbody>().useGravity = true;
            gameObject.tag = "movingDown";

			gameObject.tag = "underGround";

        }
    }

}
