using UnityEngine;
using System.Collections;
using Carnival;

public class GestureSampleBehaviour : MonoBehaviour
{
    public GameObject ringPrefab;
    public LineRenderer forceBar;   // A line that indicates force direction, its length is used to calculate force value
    public Rigidbody wheel;

    private const float destroyDelay = 5.0f;    // Thrown ring disappear after 5 seconds
    private Controller _carnivalController;
    private GameObject _ring;
    private Vector3 _startPosition;     // Position where clamp gesture is detected
    private Vector3 _releasePosition;   // Position where clamp gesture is released
    //private Vector3 _origin = new Vector3(0f, -0.5f, 0.5f); // Relativ position of created ring
    private Vector3 _origin = Vector3.zero;

    private MeshFilter _handMeshVertices;
    private int[] _indices;

    // Use this for initialization
    void Start ()
    {
        // Initialize controller
        Debug.Log("GestureSampleBehaviour: Initializing sensor");
        _carnivalController = new Controller();
        _carnivalController.Init();
        _carnivalController.Start();

        _handMeshVertices = GameObject.Find("handMeshVertices").GetComponent<MeshFilter>();

        // Quit the app when
        //Input.backButtonLeavesApp = true;
    }

    // Update is called once per frame
    void Update ()
    {
        // Back button on Android is mapped to Escape
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Get the current frame
        Frame frame = _carnivalController.Frame();

        #region Use point cloud to make player easier recognize if hand is inside sensor field of view
        _handMeshVertices.mesh.Clear();
        _handMeshVertices.mesh.vertices = frame.PointCloud;

        _indices = new int[frame.PointCloud.Length];

        for (int i = 0; i < frame.PointCloud.Length; i++)
        {
            // Use depth confidence to filter noise points, this is very useful if you want distinguish hand
            // from other objects in background
            if (frame.DepthConfidence[i] > 0 )
                _indices[i] = i;
        }

        _handMeshVertices.mesh.SetIndices(_indices, MeshTopology.Points, 0);
        #endregion

        // No gestures are detected ? We suppose the player has released his gesture here. However gesture can also be lost
        // in one or two frames due to e.g. fast movement of hand.
        if (frame.Gestures.Count == 0)
        {
            if (GameObject.FindGameObjectsWithTag("ready").Length > 0)
            {
				// CALL OUR UNIQUE FUNCTION
                ThrowRing();
            }
            return;
        }


        // For simplicity, we suppose here there is only gesture in frame
        switch (frame.Gestures[0].Type)
        {
            case GestureType.Clamp:
                // Cast first gesture object as clamp gesture to get its position
                UpdateRingPosition(((ClampGesture)frame.Gestures[0]).Midpoint);
                break;
            case GestureType.Swipe:
                // Use SwipeGesture class to access more data
                SwipeGesture swipe = (SwipeGesture)frame.Gestures[0];
                int direction = 1;
                // Four directions can be detected
                if (swipe.Direction == SwipeDirection.Left || swipe.Direction == SwipeDirection.Down)
                {
                    direction = -1;
                }

                // Use swipe speed to add torque on wheel
                Vector3 torque = new Vector3(0, 0,  direction * 2000);
                wheel.AddTorque(torque, ForceMode.Force);
                break;
            default:
                break;
        }

    }

    public void UpdateRingPosition(Vector3 clampPosition)
    {
        // Is there a ring?
        if (GameObject.FindGameObjectsWithTag("ready").Length == 0)
        {
            // Instantiate ring at clamp gesture position
            _ring = Instantiate(ringPrefab) as GameObject;
            _ring.transform.parent = Camera.main.transform;
            _ring.tag = "ready";
            _ring.transform.localPosition = _origin + clampPosition;
            _startPosition = _ring.transform.position;

            // Set start point of force bar
            forceBar.SetPosition(0, _startPosition);
        }
        else
        {
            // Update ring position and save it as release position
            // For better UX, origin is set lower in front of camera so that player can see ring better
            _ring.transform.localPosition = _origin + clampPosition;
            _releasePosition = _ring.transform.position;

            // Update end point of force bar
            forceBar.SetPosition(1, _releasePosition);
        }
    }

    // Throw when there is one ring ready
    public void ThrowRing()
    {
        if (_ring == null)
            return;

        _ring.transform.parent = null;

        const float maxDistance = 0.3f; //maximal distance that a clamp gesture can be detected

        // the longer distance, the greater force value
        float force = Mathf.Clamp((_releasePosition - _startPosition).magnitude, 0, maxDistance) / maxDistance * 1500f;

        //throw ring in release direction with force
        _ring.GetComponent<Rigidbody>().AddForce((_releasePosition - _startPosition).normalized * force);
        _ring.GetComponent<Rigidbody>().useGravity = true;
        _ring.tag = "thrown";
        Destroy(_ring, destroyDelay);

        forceBar.SetPosition(0, Vector3.zero);
        forceBar.SetPosition(1, Vector3.zero);
    }

    void OnDestroy()
    {
        Debug.Log("GestureSampleBehaviour: OnDestroy");
        _carnivalController.Stop();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (_carnivalController == null)
        {
            return;
        }

        Debug.Log("GestureSampleBehaviour: OnApplicationPause -> " + pauseStatus);

        if (pauseStatus)
        {
            _carnivalController.Stop();
        }
        else
        {
            _carnivalController.Start();
        }
    }
}
