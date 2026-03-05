using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Task_02 : Task_Base
{
    // First condition
    [SerializeField] private Button shovel;

    // Second & third condition
    [SerializeField] private PlaneObjectSpawner planeObjectSpawner;
    private int objectCounter;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "GPS")
        {
            planeObjectSpawner = GameObject.Find("XR Origin").GetComponent<PlaneObjectSpawner>();
        }
    }

    public override void OnStart()
    {
        // Subtask 01
        var goal01 = new SubTask();
        goal01.AddCondition(FirstCondition, FirstSetUp, FirstReset);
        goal01.taskDescription = taskDescriptions[0];

        goals.Add(goal01);

        // Subtask 02
        var goal02 = new SubTask();
        goal02.AddCondition(SecondCondition, SecondSetUp, SecondReset);
        goal02.taskDescription = taskDescriptions[1];
        
        goals.Add(goal02);

        // Subtask 03
        var goal03 = new SubTask();
        goal03.AddCondition(ThirdCondition, ThirdSetUp, ThirdReset);
        goal03.taskDescription = taskDescriptions[2];
        
        goals.Add(goal03);
    }

    private bool FirstCondition()
    {
        return IsButtonClicked(shovel) || buttonManager.IsActive(shovel.name);
    }

    private void FirstSetUp()
    {
        SetupButtonListener(shovel);
        shovel.image.color = highlightColor;
    }

    private void FirstReset()
    {
        ResetButtonListener(shovel);
        shovel.image.color = baseColor;
    }

    private bool SecondCondition()
    {
        return planeObjectSpawner.arPlaneManager.trackables.count > 0 ? true : false;
    }

    private void SecondSetUp(){ }
    private void SecondReset() { }

    private bool ThirdCondition()
    {
        // doesn't always work -> dig counter reset too fast
        //return planeObjectSpawner.digCounter >= 3 ? true : false;
        //return planeObjectSpawner.spawnObject.spawned ? true : false;
        return planeObjectSpawner.spawnedObjects.Count > objectCounter ? true : false;
    }

    private void ThirdSetUp() 
    {
        objectCounter = planeObjectSpawner.spawnedObjects.Count;
    }
    private void ThirdReset() { }
}
