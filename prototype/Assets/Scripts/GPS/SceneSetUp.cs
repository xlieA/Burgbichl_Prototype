using TMPro;
using UnityEngine;

public class SceneSetUp : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject obj;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;

    [SerializeField] private float rotationAngle = 20f;

    [SerializeField] private bool debugging = false;
    [SerializeField] private SpawnableObject test;


    // Start is called before the first frame update
    void Start()
    {
        if (SceneLoader.instance.objInfo != null)
        {
            SetUp(SceneLoader.instance.objInfo);
        }

        if (debugging)
        {
            SetUp(test);
        }
    }

    // Adapts the scene based on the given object
    public void SetUp(SpawnableObject o)
    {
        title.text = SceneLoader.instance.English ? o.objectNames[0] : o.objectNames[1];
        description.text = SceneLoader.instance.English ? o.descriptions[0] : o.descriptions[1];

        ReplaceObject(o);
    }

    // Replaces placeholder object with real object
    private void ReplaceObject(SpawnableObject newObject)
    {
        Vector3 position = obj.transform.position;
        Quaternion rotation = obj.transform.rotation;
        Destroy(obj);

        obj = Instantiate(newObject.prefab, position, rotation);
        obj.transform.SetParent(parent.transform);

        Debug.Log("Object type: " + newObject.GetType());
        // Object is an excavation site
        if (newObject.GetType() == typeof(SpawnableArtefact))
        {
            obj.transform.localScale = newObject.scaleRateObjScene;
        }
        else
        {
            obj.transform.rotation = Quaternion.Euler(rotationAngle, 0, 0) * obj.transform.rotation;
            obj.transform.localScale = newObject.scaleRateObjScene;
        }
    }
}
