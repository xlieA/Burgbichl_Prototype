// https://discussions.unity.com/t/spawn-object-at-realtime-location-gps/700920
// https://docs.unity3d.com/ScriptReference/LocationService.Start.html
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    public TMP_Text GPSStatus;
    public TMP_Text latitudeText;
    public TMP_Text longitudeText;
    public TMP_Text altitudeText;

    [HideInInspector] public double latitudeValue;
    [HideInInspector] public double longitudeValue;
    [HideInInspector] public double altitudeValue;


    // Start is called before the first frame update
    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        RequestLocation();
#else
        StartCoroutine(GPSLoc());
#endif
    }

    // Gets player GPS signal
    IEnumerator GPSLoc()
    {
        // Check if user has location service enabled
        if(!Input.location.isEnabledByUser)
        {
            GPSStatus.text = "GPS location not enabled";
            Debug.Log("GPS location not enabled");
            yield break;
        }

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Initialization failed
        if(maxWait < 1)
        {
            GPSStatus.text = "Time out";
            Debug.Log("Timed out");
            yield break;
        }

        // Connection failed
        if(Input.location.status == LocationServiceStatus.Failed)
        {
            GPSStatus.text = "Unable to determine device location";
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted
            GPSStatus.text = "Running";

            // Update GPS location every second
            InvokeRepeating("UpdateGPSData", 0.5f, 1f);
        }
    }

    // Used mainly for debugging
    private void UpdateGPSData()
    {
        if(Input.location.status == LocationServiceStatus.Running)
        {
            // Access granted
            GPSStatus.text = "Running";

              Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + 
                Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " +
                Input.location.lastData.timestamp);

            latitudeText.text = Input.location.lastData.latitude.ToString();
            longitudeText.text = Input.location.lastData.longitude.ToString();
            altitudeText.text = Input.location.lastData.altitude.ToString();

            latitudeValue = Input.location.lastData.latitude;
            longitudeValue = Input.location.lastData.longitude;
            altitudeValue = Input.location.lastData.altitude;
        }
        else
        {
            // Service is stopped
            GPSStatus.text = "Stopped";
        }
    }

    // Allow GPS access for WebGL build
    public void RequestLocation()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GetLocation();
#else
        Debug.Log("RequestLocation() is only available on WebGL.");
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void GetLocation();
#else
    // Dummy version for other platforms so linker doesn't break
    private static void GetLocation() { }
#endif

    // Get location from js script
    public void ReceiveLocation(string location)
    {
        string[] coords = location.Split(',');
        if (coords.Length == 2)
        {
            // Access granted
            GPSStatus.text = "Running";

            latitudeValue = double.Parse(coords[0]);
            longitudeValue = double.Parse(coords[1]);

            latitudeText.text = coords[0];
            longitudeText.text = coords[1];

            Debug.Log("GPS Location: " + latitudeValue + ", " + longitudeValue);
        }
    }
}