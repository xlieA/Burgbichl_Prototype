// https://discussions.unity.com/t/ar-foundation-proper-way-to-turn-off-planes-after-basic-placement/722634
// https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Handheld.Vibrate.html
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.XR.ARSubsystems;
using System.Collections;

public class PlaneObjectSpawner : MonoBehaviour
{
    // Object that is currently spawning
    [HideInInspector] public SpawnableObject spawnObject = null;
    // List of all spawned objects
    [HideInInspector] public List<GameObject> spawnedObjects = new List<GameObject>();

    private ARRaycastManager arRaycastManager;
    [HideInInspector] public ARPlaneManager arPlaneManager;
    // Detects objects infront of camera
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Player needs to dig up the model
    [HideInInspector] public int digCounter = 0;
    public TMP_Text digCounterText;
    // Flag to control digCounter updates
    private bool canUpdateCounter = true;

    [SerializeField] public bool debugging;

    [SerializeField] private ShakeDetector shakeDetector;
    [SerializeField] private MoveShovel moveShovel;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CompassManager compassManager;
    

    void Awake()
    {
        RequestCameraAccess();
        
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();

        shakeDetector = GetComponent<ShakeDetector>();

        moveShovel = GameObject.Find("Camera Offset/Main Camera/Tools/shovel").GetComponent<MoveShovel>();

        playerInput = GameObject.Find("PlayerInput").GetComponent<PlayerInput>();

        compassManager = GameObject.Find("ExcavationLoop").GetComponent<CompassManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Start with plane detetction turned off
        TogglePlaneDetection(false);
    }

    public void SpawnObject()
    {
        if (spawnObject != null)
        {
            // Activate plane detection
            if (arPlaneManager.enabled != true)
            {
                TogglePlaneDetection(true);
            }

            // Spawn object
            Debug.Log("Call the spawner");
            if (shakeDetector.MotionDetected() && canUpdateCounter)
            {
                StartCoroutine(UpdateDigCounter());
            }

            if (digCounter >= 3)
            {
                //SpawnOnTouch();
                RandomSpawn();
            }
        }
    }

    // Spawns a object on the plane the user touches
    private void SpawnOnTouch()
    {
        // Check for user input
        if(!playerInput.TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        // Raycast to find plane
        if(arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            // Only update the digCounter if allowed
            if (canUpdateCounter)
            {
                StartCoroutine(UpdateDigCounter());
            }

            // Get the first hit from the ARRaycastManager
            var hitPose = hits[0].pose;

            if(spawnObject != null) {
                // Spawn the object at the hit position on the plane
                GameObject spawnedObject = Instantiate(spawnObject.prefab, hitPose.position, hitPose.rotation);
                spawnedObjects.Add(spawnedObject);
                spawnObject.spawned = true;

                // Reset
                digCounter = 0;
                digCounterText.text = digCounter.ToString();

                spawnObject = null;

                TogglePlaneDetection(false);
            }
        }
    }

    // Spawns a mushroom at the first plane a ray hits
    void RandomSpawn()
    {
        // Raycast to find plane
        if(arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon))
        {   
            Debug.Log("Plane detected!");
            var hitPose = hits[0].pose;

            if(spawnObject != null) {
                // Spawn the object at the hit position on the plane
                Quaternion xRotation = Quaternion.Euler(90f, 0f, 0f);
                // Rotate the object to face north
                Quaternion northRotation = Quaternion.Euler(0f, -compassManager.trueNorth, 0f);
                GameObject spawnedObject = Instantiate(spawnObject.prefab, hitPose.position, xRotation * northRotation);

                DontDestroyOnLoad(spawnedObject);
                spawnedObjects.Add(spawnedObject);
                spawnObject.spawned = true;

                // Reset
                digCounter = 0;
                digCounterText.text = digCounter.ToString();

                spawnObject = null;

                TogglePlaneDetection(false);
            }
        }
    }

    // Toggle plane detection on/off
    public void TogglePlaneDetection(bool enable)
    {
        if (arPlaneManager != null)
        {
            arPlaneManager.enabled = enable;

            // Set all existing planes to active/inactive based on the toggle state
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(enable);
            }
        }
    }

    // Handle digCounter update delay (avoid spamming)
    private IEnumerator UpdateDigCounter()
    {
        // Prevent further updates
        canUpdateCounter = false;

        // Increment the digCounter
        digCounter++;
        digCounterText.text = digCounter.ToString();

        StartCoroutine(moveShovel.Lerp());

        //Handheld.Vibrate();   // Doesen't work
        //Vibration.Vibrate(500); // Doesn't work either?

        // Wait for 1 second before allowing another update
        yield return new WaitForSeconds(1f);

        // Allow counter to be updated again
        canUpdateCounter = true;
    }

    // Moves shovel when phone is shaken
    public void UseShovel()
    {
        if (shakeDetector.MotionDetected())
        {
            StartCoroutine(moveShovel.Lerp());
        }
    }

    // Only used for debugging
    public void SpawnOnTouchDebug(SpawnableObject objectToSpawn)
    {
         // Check for mouse click (left button)
        if (Input.GetMouseButtonDown(0))
        {
            // Get the mouse position
            Vector2 mousePosition = Input.mousePosition;

            // Raycast to find plane
            if (arRaycastManager.Raycast(mousePosition, hits, TrackableType.PlaneWithinPolygon))
            {
                // Only update the digCounter if allowed
                if (canUpdateCounter)
                {
                    StartCoroutine(UpdateDigCounter());
                }

                Debug.Log("Dig counter: " + digCounter);

                // Get the first hit from the ARRaycastManager
                var hitPose = hits[0].pose;

                spawnObject = objectToSpawn;
                if (spawnObject != null && digCounter == 3) 
                {
                    // Spawn the object at the hit position on the plane
                    Quaternion extraRotation = Quaternion.Euler(90f, 0f, 0f);
                    GameObject spawnedObject = Instantiate(spawnObject.prefab, hitPose.position, hitPose.rotation * extraRotation);

                    DontDestroyOnLoad(spawnedObject);
                    spawnedObjects.Add(spawnedObject);
                    spawnObject.spawned = true;

                    // Reset dig counter
                    digCounter = 0;
                }
            }
        }
    }

    // Allow camera access for WebGL build
    public void RequestCameraAccess()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        RequestCamera();
#else
        Debug.Log("RequestCamera() is only available on WebGL.");
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void RequestCamera();
#else
    // Dummy version for other platforms so linker doesn't break
    private static void RequestCamera() { }
#endif
}