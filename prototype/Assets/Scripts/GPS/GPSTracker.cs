// https://www.geodatasource.com/resources/tutorials/how-to-calculate-the-distance-between-2-locations-using-c/
// https://youtu.be/CtctLYEyoB4?si=kLE70WVG9_1vTpFU
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GPSTracker : MonoBehaviour
{
    // Unit used to calculate distance between two GPS positions
    // K ... kilometers
    [SerializeField] private char myUnit = 'K';

    [SerializeField] private SpawnableObjectManager spawnableObjectManager;
    // List of all object positions
    private List<SpawnableObject> spawnableObjects;
    public TMP_Text spawnObjectName;

    // How far the player is away from spawn position
    [SerializeField] public float distance = 0.01f;
    public TMP_Text differenceText;
    [HideInInspector] public TMP_Text latitudeDifferenceText;
    [HideInInspector] public TMP_Text longitudeDifferenceText;

    // Indicates if closest object was found and should be spawned now
    [HideInInspector] public bool enterSpawningPhase = false;


    void Awake()
    {
        spawnableObjectManager = GameObject.Find("SpawnableObjectManager").GetComponent<SpawnableObjectManager>();
        spawnableObjects = spawnableObjectManager.excavationSites;
    }

    // Finds closest object to player
    public SpawnableObject FindClosestObject(PlayerLocation playerLocation)
    {
        double closestDist = Double.MaxValue;
        SpawnableObject closestObject = null;

        // Iterate through all possible object positions
        foreach (SpawnableObject s in spawnableObjects)
        {
            if (!s.spawned)
            {
                // Calculate the distance
                double tempDist = LocationDifference(playerLocation.latitudeValue, playerLocation.longitudeValue, 
                    s.latitudeValue, s.longitudeValue, myUnit);

                // Find the closest object
                if (tempDist < closestDist)
                {
                    closestDist = tempDist;
                    closestObject = s;
                }
            }
        }

        // Debugging
        spawnObjectName.text = closestObject.name;
        differenceText.text = closestDist.ToString("F5");

        return closestObject;
    }

    // Check if the closest object is within spawning range
    public bool WithinSpawningRange(SpawnableObject site, PlayerLocation playerLocation)
    {
        if (site == null)
        {
            enterSpawningPhase = false;
            return false;
        }
        
        double objDistance = LocationDifference(playerLocation.latitudeValue, playerLocation.longitudeValue, 
            site.latitudeValue, site.longitudeValue, myUnit);

        if (objDistance < distance)
        {
            enterSpawningPhase = true;
            return true;
        }

        enterSpawningPhase = false;
        return false;
    }

    // Doesn't account for curvature of earth
    private bool LocationDifference(PlayerLocation playerLocation, SpawnableObject sObject)
    {
        // Check if player is close to object
        double latitudeDifference = Math.Abs(playerLocation.latitudeValue - sObject.latitudeValue);
        double longitudeDifference = Math.Abs(playerLocation.longitudeValue - sObject.longitudeValue);
        
        // Debugging
        latitudeDifferenceText.text = latitudeDifference.ToString("F5");
        longitudeDifferenceText.text = longitudeDifference.ToString("F5");

        // distance ... maximum distance a user is allowed to be away form an spawnable position and the object still spawns
        if(latitudeDifference < distance && longitudeDifference < distance)
        {
            Debug.Log("Object GPS is close to player!");
            return true;
        }
        return false;
    }

    // Difference between two GPS location
    private double LocationDifference(double lat1, double lon1, double lat2, double lon2, char unit) {
        if ((lat1 == lat2) && (lon1 == lon2))
        {
            return 0;
        }
        else
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(Deg2rad(lat1)) * Math.Sin(Deg2rad(lat2)) +
                Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) * Math.Cos(Deg2rad(theta));

            dist = Math.Acos(dist);
            dist = Rad2deg(dist);
            dist = dist * 60 * 1.1515;

            if (unit == 'K')
            {
                dist = dist * 1.609344;
            } else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }

            differenceText.text = dist.ToString("F5");
            
            return dist;
        }
    }

    // This function converts decimal degrees to radians
    private double Deg2rad(double deg) 
    {
        return (deg * Math.PI / 180.0);
    }

    // This function converts radians to decimal degrees
    private double Rad2deg(double rad)
    {
        return (rad / Math.PI * 180.0);
    }
}
