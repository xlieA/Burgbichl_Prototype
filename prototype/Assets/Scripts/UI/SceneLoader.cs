using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader _instance;
    public static SceneLoader instance
    {
        get
        {
            if (_instance == null)
            {
                //GameObject obj = new GameObject("SceneLoader");
                //_instance = obj.AddComponent<SceneLoader>();
                //DontDestroyOnLoad(obj);
                _instance = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
            }
            return _instance;
        }
    }

    // Loading info per object
    [HideInInspector] public SpawnableObject objInfo;

    // Test version settings
    [SerializeField] public SpawnableObject testVersionObj;
    [HideInInspector] public double originalLat;
    [HideInInspector] public double originalLong;
    private bool setOriginalCoords = false;
    [HideInInspector] public bool testVersion = false;

    // Tutorial settings
    [HideInInspector] public bool tutorial = false;
    [HideInInspector] public bool reloadTutorial = false;

    // Language settings
    // Event that gets triggered when the language changes
    public static event Action<bool> OnLanguageChanged;

    private bool english = true;

    public bool English
    {
        get { return english; }
        set
        {
            if (english != value) // Only trigger if the value actually changes
            {
                english = value;
                OnLanguageChanged?.Invoke(english); // Trigger event
            }
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize()
    {
        if (!setOriginalCoords && SceneManager.GetActiveScene().name == "GPS" && testVersionObj != null)
        {
            originalLat = testVersionObj.latitudeValue;
            originalLong = testVersionObj.longitudeValue;
            setOriginalCoords = true;
        }
    }

    public void LoadSceneWithObject(string sceneName, SpawnableObject obj)
    {
        objInfo = obj;
        SceneManager.LoadScene(sceneName);
    }

    public void SetUpTestVersion()
    {
        testVersion = true;
        tutorial = true;
        reloadTutorial = true;
    }

    public void SetUpFullVersion()
    {
        if (testVersionObj != null)
        {
            testVersionObj.latitudeValue = originalLat;
            testVersionObj.longitudeValue = originalLong;
        }

        testVersion = false;
        tutorial = true;
        reloadTutorial = true;
    }

    public void SkipTutorial()
    {
        tutorial = false;
    }

    public void ResetTutorial()
    {
        tutorial = true;
    }
}