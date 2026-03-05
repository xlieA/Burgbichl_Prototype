using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MA/SpawnableObject")]
public class SpawnableObject : ScriptableObject
{
    public string objectName;
    public List<string> objectNames = new List<string>();
    public double latitudeValue;
    public double longitudeValue;
    public double altitudeValue;

    public GameObject prefab;
    public Sprite img;
    [TextArea] public List<string> descriptions = new List<string>();

    public bool spawned = false;

    public Vector3 scaleRateObjScene = new Vector3(0.03f, 0.03f, 0.03f);
}
