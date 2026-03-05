using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSceneSetUp : MonoBehaviour
{
    [SerializeField] public List<GameObject> tutorialBoxes = new List<GameObject>();
    [SerializeField] private List<TaskDescription> taskDescriptions = new List<TaskDescription>();

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (tutorialBoxes.Count == taskDescriptions.Count)
            {
                FillTutorialBoxes();
            }
        }
    }

    public int getTaskCount()
    {
        int result = 0;

        for (int i = 0; i < tutorialBoxes.Count; ++i)
        {
            if (taskDescriptions[i].completed)
            {
                result++;
            }
            else
            {
                break;
            }
        }

        return result;
    }

    // Fills boxes with completed tasks
    private void FillTutorialBoxes()
    {
        for (int i = 0; i < tutorialBoxes.Count; ++i)
        {
            if (taskDescriptions[i].completed)
            {
                string temp = SceneLoader.instance.English ? taskDescriptions[i].descriptions[0] : taskDescriptions[i].descriptions[1];
                tutorialBoxes[i].GetComponentInChildren<TMP_Text>().text = temp;
            }

            // don't like that
            //SetActive(tutorialBoxes[i], taskDescriptions[i].completed);
        }
    }

    // Sets boxes active
    private void SetActive(GameObject box, bool active)
    {
        box.SetActive(active);
    }

    // Sets all task of the tutorial
    public void ResetTasks(bool completed)
    {
       foreach (TaskDescription task in taskDescriptions)
        {
            task.completed = completed;
        }
    }

    // Resets tutorial to a certain task
    public void ResetTasks(int taskNumber)
    {
        for (int i = 0; i < taskNumber; ++i)
        {
            taskDescriptions[i].completed = true;
        }

        for (int i = taskNumber; i < taskDescriptions.Count; ++i)
        {
            taskDescriptions[i].completed = false;
        }
    }
}
