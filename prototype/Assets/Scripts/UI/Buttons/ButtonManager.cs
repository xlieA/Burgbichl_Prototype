// https://docs.unity3d.com/6000.0/Documentation/ScriptReference/SceneManagement.SceneManager.html
// https://community.gamedev.tv/t/how-do-i-make-the-quit-button-work-for-webgl/40403/5
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    // List of all tool buttons
    [HideInInspector] public Dictionary<string, ButtonElement> buttonElements = new Dictionary<string, ButtonElement>();

    [SerializeField] public Color bgColor;
    [SerializeField] public Color highlightColor;

    [SerializeField] private ToolManager toolManager;
    [SerializeField] private AchievementManager achievementManager;
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private TutorialSceneSetUp tutorialSceneSetUp;


    void Awake()
    {
        Debug.Log("Scene name: " + SceneManager.GetActiveScene().name);
        // Tool buttons are only in the GPS scene
        if (SceneManager.GetActiveScene().name == "GPS")
        {
            buttonElements.Add("Compass", new ButtonElement("Compass"));
            buttonElements.Add("Shovel", new ButtonElement("Shovel"));
            buttonElements.Add("Pickaxe", new ButtonElement("Pickaxe"));

            toolManager = GetComponent<ToolManager>();
            taskManager = GameObject.Find("TaskManager").GetComponent<TaskManager>();
        }

        if (SceneManager.GetActiveScene().name == "Achievements")
        {
            achievementManager = GetComponent<AchievementManager>();
            tutorialSceneSetUp = GameObject.Find("TutorialSetUp").GetComponent<TutorialSceneSetUp>();
        }

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            tutorialSceneSetUp = GameObject.Find("TutorialSetUp").GetComponent<TutorialSceneSetUp>();
        }
    }

    // Loads a scene by name
    public void LoadSceneByName(string sceneName)
    {
        AudioManager.instance.Button();
        SceneLoader.instance.SkipTutorial();

        if (AudioManager.instance.IsTalking())
        {
            AudioManager.instance.StopTalking();
        }

        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneWithObject(Button b)
    {
        AudioManager.instance.Button();
        SceneLoader.instance.SkipTutorial();

        SpawnableObject obj = achievementManager.GetButtonInfo(b);

        if (obj != null)
        {
            SceneLoader.instance.LoadSceneWithObject("ObjectInfo", obj);
        }
    }

    public void LoadTestVersion()
    {
        AudioManager.instance.Button();
        SceneLoader.instance.SetUpTestVersion();
        SceneManager.LoadScene("GPS");
    }

    public void LoadFullVersion()
    {
        AudioManager.instance.Button();
        SceneLoader.instance.SetUpFullVersion();
        SceneManager.LoadScene("GPS");
    }

    // Exits the game (handles both the Unity Editor and the build)
    public void ExitGame()
    {
        AudioManager.instance.Button();

#if UNITY_EDITOR
        // Stop playing the scene in the editor
        EditorApplication.isPlaying = false;
#elif (UNITY_WEBGL)
            Application.OpenURL("about:blank");
#else
            // Quit the application
            Application.Quit();
#endif
    }

    public void SelectTool(Button button)
    {
        AudioManager.instance.Button();

        SelectTool(button.name);
    }

    // Selects tool and deselects all other tools
    public void SelectTool(string name)
    {
        foreach (var kvp in buttonElements)
        {
            string key = kvp.Key;
            ButtonElement element = kvp.Value;

            if (key.Contains(name))
            {
                if (element.isActive)
                {
                    Deselect(element);
                    toolManager.DisableTool(key);
                }
                else
                {
                    Select(element);
                    toolManager.EnableTool(key);
                }
            }
            else
            {
                Deselect(element);
                toolManager.DisableTool(key);

                if (taskManager.tutorialRunning && !taskManager.currentTask.taskCondition.Condition())
                {
                    taskManager.currentTask.taskCondition.SetUp();
                }
            }
        }
    }

    // Shows if a given button is currently selected
    public bool IsActive(string name)
    {
        if (buttonElements.ContainsKey(name))
        {
            if (buttonElements[name].isActive)
            {
                return true;
            }
        }

        return false;
    }

    // Deselects all tools
    public void DeselctAllTools()
    {
        foreach (var kvp in buttonElements)
        {
            string key = kvp.Key;
            ButtonElement element = kvp.Value;

            Deselect(element);
            toolManager.DisableTool(key);
        }
    }

    // Selects given button by highlighting it
    private void Select(ButtonElement b)
    {
        b.isActive = true;
        ChangeColor(b.button, highlightColor);
    }

    // Deselects given button by turning BG color back
    private void Deselect(ButtonElement b)
    {
        b.isActive = false;
        ChangeColor(b.button, bgColor);
    }

    // Changes the BG color of a given button to a new color
    public void ChangeColor(Button button, Color newColor)
    {
        button.image.color = newColor;
    }

    // Gets rid off the pop up effect
    public void SendArtefact(GameObject artefact)
    {
        AudioManager.instance.Button();
        Destroy(artefact);
    }

    // Sets all task of the tutorial
    public void ResetTasks(bool completed)
    {
        AudioManager.instance.Button();
        tutorialSceneSetUp.ResetTasks(completed);
        SceneLoader.instance.ResetTutorial();
        SceneManager.LoadScene("GPS");
    }

    // Reset tutorial to given task
    public void ResetTasks(int taskNumber)
    {
        AudioManager.instance.Button();

        if (tutorialSceneSetUp.tutorialBoxes[taskNumber].GetComponentInChildren<TMP_Text>().text != "--?--")
        {
            tutorialSceneSetUp.ResetTasks(taskNumber);
            SceneLoader.instance.ResetTutorial();
            SceneManager.LoadScene("GPS");
        }
       
    }

    // Rest tutorial to last completed task
    public void ResetTasks()
    {
        AudioManager.instance.Button();
        tutorialSceneSetUp.ResetTasks(tutorialSceneSetUp.getTaskCount());
        Debug.Log("Task Count: " + tutorialSceneSetUp.getTaskCount());
        SceneManager.LoadScene("GPS");
    }
}
