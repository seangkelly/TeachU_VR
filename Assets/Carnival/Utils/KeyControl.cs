using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class KeyControl : MonoBehaviour
{
    void Update()
    {
        // Reload game when R is pressed
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // Quit when ESC is pressed
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
