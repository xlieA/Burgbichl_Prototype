// https://www.youtube.com/watch?v=CtctLYEyoB4&t=10s

using System;
using UnityEngine;

public class CompassManager : MonoBehaviour
{
    // Point towards POI
    [SerializeField] public GameObject arrow;
    [SerializeField] private Vector3 startingRotation = new Vector3(0, 180, 90);

    [SerializeField] private GameObject debuggingArrow;
    [SerializeField] private bool debugging;

    public float trueNorth;
    // Reduces jitter
    private float timeDelay = 0.25f;


    // Start is called before the first frame update
    void Start()
    {
        // Make sure arrow is correctly rotated
        arrow.transform.localRotation = Quaternion.Euler(startingRotation);
        Input.compass.enabled = true;

        if (debugging)
        {
            debuggingArrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            debuggingArrow.SetActive(true);
        }
    }

    public void CalculateDirection(PlayerLocation playerLocation, SpawnableObject spawnObject)
    {
        timeDelay -= Time.deltaTime;

        if (timeDelay < 0)
        {
            // Reset timer
            timeDelay = 0.25f;

            trueNorth = Math.Abs(Input.compass.trueHeading);
        }

        double bearing = GetBearing(playerLocation.latitudeValue, playerLocation.longitudeValue,
            spawnObject.latitudeValue, spawnObject.longitudeValue);

        float waypointDir = (float)bearing - trueNorth;
        arrow.transform.localRotation = Quaternion.Euler(startingRotation.x, startingRotation.y, waypointDir + 90);     // For some reason it is 90° off with the compass prefab?

        if (debugging)
        {
            debuggingArrow.transform.localRotation = Quaternion.Euler(startingRotation.x, startingRotation.y, -waypointDir);
        }
    }

    private double GetBearing(double lat, double lon, double lat2, double lon2)
    {
        double dy = lat2 - lat;
        double dx = Math.Cos(Math.PI / 180 * lat) * (lon2 -lon);
        double angle = Math.Atan2(dy, dx);

        angle = angle * 180 / Math.PI;
        angle = 360 - Math.Abs(angle);

        return angle;
    }
}
