using UnityEngine;
using System.Collections;

namespace Carnival
{
    /**
     * @brief
     * The LinkToMainCamera class sets the position and rotation of the attached object to the position and rotation of the main camera object.
     * You can also set the attached object as a child of the main camera to achieve the same effect.
     */
    public class LinkToMainCamera : MonoBehaviour
    {
        void Update()
        {
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}