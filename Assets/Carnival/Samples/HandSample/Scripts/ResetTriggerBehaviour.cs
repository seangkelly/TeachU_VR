using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResetTriggerBehaviour : MonoBehaviour
{
	// Reload scene when baseball leaves reset trigger
	void OnTriggerExit(Collider col)
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}