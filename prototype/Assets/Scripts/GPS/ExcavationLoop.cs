using System.Collections;
using TMPro;
using UnityEngine;

public class ExcavationLoop : MonoBehaviour
{
    // Manages excavation sites and artefacts
    [SerializeField] private SpawnableObjectManager spawnableObjectManager;
    // Contains the player's position (constantly updated)
    [SerializeField] private PlayerLocation playerLocation;
    // Keeps track of the closest POI to player
    [SerializeField] private GPSTracker tracker;
    // Points player in direction of POI
    [SerializeField] private CompassManager compass;
    // Spawns excavation site
    [SerializeField] private PlaneObjectSpawner planeObjectSpawner;
    [SerializeField] private TMP_Text objLat;
    [SerializeField] private TMP_Text objLong;
    // Spawns an artefact
    [SerializeField] private OnClickSpawner onClickSpawner;
    [SerializeField] private ButtonManager buttonManager;

    [SerializeField] private TMP_Text debugText;

    [SerializeField] private GameObject errorObject;
    [SerializeField] private TMP_Text errorText;
    private bool canShowError = true;

    private SpawnableObject testVersionObj;
    [SerializeField] private double coordOffset = 0.05d;
    private bool setUpComplete = false;

    private bool canRun = false;


    void Awake()
    {
        spawnableObjectManager = GameObject.Find("SpawnableObjectManager").GetComponent<SpawnableObjectManager>();

        playerLocation = GameObject.Find("PlayerLocation").GetComponent<PlayerLocation>();
        tracker = GetComponent<GPSTracker>();
        compass = GetComponent<CompassManager>();

        planeObjectSpawner = GameObject.Find("XR Origin").GetComponent<PlaneObjectSpawner>();
        onClickSpawner = GameObject.Find("XR Origin").GetComponent<OnClickSpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayExecution());
    }

    // Update is called once per frame
    void Update()
    {
        if (!canRun)
        {
            return;
        }

        // Only executed afer a few minutes
        if (playerLocation.GPSStatus.text == "Running")
        {
            // Play full version
            if (!SceneLoader.instance.testVersion)
            {
                if (!setUpComplete)
                {
                    PrepareForFullVersion();
                }

                GameLoop();
            }
            else
            {
                // Play test version
                if (!setUpComplete)
                {
                    PrepareForTestVersion();
                }

                GameLoopTestVersion();
            }
        }
        else
        {
            // Error handling: GPS not working
            Debug.Log("Your GPS is not working");
            //errorText.text = "Your GPS is not working";
            errorText.text = SceneLoader.instance.English ? LGPS.instance.gpsText[0] : LGPS.instance.gpsText[1];
            ErrorHandling();
        }
        

        if (planeObjectSpawner.debugging)
        {
            // Just spawn random object
            planeObjectSpawner.TogglePlaneDetection(true);
            planeObjectSpawner.SpawnOnTouchDebug(spawnableObjectManager.excavationSites[5]);
        }
    }

    // Waits a few seconds before Update() is called -> gives phone a bit of time to connect to GPS
    private IEnumerator DelayExecution()
    {
        // Wait for a few seconds
        yield return new WaitForSeconds(3f);
        canRun = true;
    }

    private void GameLoop()
    {
        Debug.Log("GPS works! Ready for spawning objects!");

        HandleCompass();
        HandleShovel();
        HandlePickaxe();
    }

    private void GameLoopTestVersion()
    {
        // Compass tool is selected
        if (buttonManager.buttonElements["Compass"].isActive)
        {
            if (!testVersionObj.spawned)
            {
                // Set the object for spawning
                planeObjectSpawner.spawnObject = testVersionObj;
                tracker.spawnObjectName.text = planeObjectSpawner.spawnObject.ToString();
                objLat.text = planeObjectSpawner.spawnObject.latitudeValue.ToString();
                objLong.text = planeObjectSpawner.spawnObject.longitudeValue.ToString();

                if (!tracker.enterSpawningPhase)
                {
                    compass.CalculateDirection(playerLocation, planeObjectSpawner.spawnObject);
                    tracker.WithinSpawningRange(planeObjectSpawner.spawnObject, playerLocation);
                }
                else if (tracker.enterSpawningPhase)
                {
                    // Deselect compass
                    buttonManager.DeselctAllTools();
                    //errorText.text = "You've reached the site";
                    errorText.text = SceneLoader.instance.English ? LGPS.instance.reachedSiteText[0] : LGPS.instance.reachedSiteText[1];
                    ErrorHandling();
                }
            }
            else
            {
                // Error handling: All sites excavated
                Debug.Log("You've excavted all sites");
                //errorText.text = "You've excavted all sites";
                    errorText.text = SceneLoader.instance.English ? LGPS.instance.excavatedAllText[0] : LGPS.instance.excavatedAllText[1];
                ErrorHandling();
            }
        }

        HandleShovel();
        HandlePickaxe();
    }

    // Handles player interaction when compass tool selected
    private void HandleCompass()
    {
         // Compass tool is selected
        if (buttonManager.buttonElements["Compass"].isActive)
        {
            if (planeObjectSpawner.spawnedObjects.Count < spawnableObjectManager.excavationSites.Count)
            {
                   
                // Check if a object is near the player
                planeObjectSpawner.spawnObject = tracker.FindClosestObject(playerLocation);
                tracker.spawnObjectName.text = planeObjectSpawner.spawnObject.ToString();
                objLat.text = planeObjectSpawner.spawnObject.latitudeValue.ToString();
                objLong.text = planeObjectSpawner.spawnObject.longitudeValue.ToString();

                // Not yet spawning, still searching for location
                if (planeObjectSpawner.spawnObject == null)
                {
                    // Error handling
                    Debug.Log("Couldn't find excatation site");
                    //errorText.text = "Couldn't find excavation site";
                    errorText.text = SceneLoader.instance.English ? LGPS.instance.couldntFindText[0] : LGPS.instance.couldntFindText[1];

                    ErrorHandling();
                }
                else if (!tracker.enterSpawningPhase)
                {
                    compass.CalculateDirection(playerLocation, planeObjectSpawner.spawnObject);
                    tracker.WithinSpawningRange(planeObjectSpawner.spawnObject, playerLocation);
                }
                else if (tracker.enterSpawningPhase)
                {
                    // Deselect compass
                    buttonManager.DeselctAllTools();
                    //errorText.text = "You've reached the site";
                    errorText.text = SceneLoader.instance.English ? LGPS.instance.reachedSiteText[0] : LGPS.instance.reachedSiteText[1];
                    ErrorHandling();
                }
            }
            else
            {
                // Error handling: All sites excavated
                Debug.Log("You've excavted all sites");
                //errorText.text = "You've excavted all sites";
                    errorText.text = SceneLoader.instance.English ? LGPS.instance.excavatedAllText[0] : LGPS.instance.excavatedAllText[1];
                ErrorHandling();
            }
        }
    }

    // Handles player interaction when shovel tool selected
    private void HandleShovel()
    {
        // Digging up excavation side only works with shovel
        if (buttonManager.buttonElements["Shovel"].isActive && 
            tracker.WithinSpawningRange(planeObjectSpawner.spawnObject, playerLocation))
        {
            //planeObjectSpawner.UseShovel();

            // Spawn actual object
            planeObjectSpawner.SpawnObject();
        }
        else if (buttonManager.buttonElements["Shovel"].isActive)
        {
            planeObjectSpawner.UseShovel();
        }
    }

    // Handles player interaction when pickaxe tool selected
    private void HandlePickaxe()
    {
        // Digging up artefact only works with pickaxe
        if (buttonManager.buttonElements["Pickaxe"].isActive)
        {
            onClickSpawner.UsePickaxe();
            //onClickSpawner.SpawnObjectOnTouch();
        }
    }

    private void ErrorHandling()
    {
        if (canShowError)
        {
            StartCoroutine(ShowErrorMessage());
        }
    }

    // Shows error text for five seconds
    private IEnumerator ShowErrorMessage()
    {
        canShowError = false;

        errorObject.SetActive(true);
        yield return new WaitForSeconds(5);
        errorObject.SetActive(false);

        // Time between error messages
        yield return new WaitForSeconds(20);

        canShowError = true;
    }

    // Set the excavation site used in the test version
    private void PrepareForTestVersion()
    {
        testVersionObj = SceneLoader.instance.testVersionObj;

        // Set GPS coords
        testVersionObj.latitudeValue = playerLocation.latitudeValue + coordOffset;
        testVersionObj.longitudeValue = playerLocation.longitudeValue;

        setUpComplete = true;
    }

    // Resets coords for excavation site used in test version
    private void PrepareForFullVersion()
    {
        SceneLoader.instance.testVersionObj.latitudeValue = SceneLoader.instance.originalLat;
        SceneLoader.instance.testVersionObj.longitudeValue = SceneLoader.instance.originalLong;

        setUpComplete = true;
    }
}
