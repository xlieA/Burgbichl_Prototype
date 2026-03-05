using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : LanguageListener
{
    // List of all tutorial tasks
    [SerializeField] private List<Task_Base> taskList = new List<Task_Base>();
    // List of all subtasks per tutorial task
    public List<SubTask> allGoals = new List<SubTask>();

    private int currentIndex = 0;
    public SubTask currentTask => currentIndex < allGoals.Count ? allGoals[currentIndex] : null;
    private bool activeTask = false;
    public bool tutorialRunning = false;

    [SerializeField] public Assistent assistent;
    [SerializeField] private GameObject helpMenu;
    [SerializeField] private GameObject boss;


    void Awake()
    {
        // isn't working?
        //assistent = GameObject.Find("Canvas/HelpMenu").GetComponent<Assistent>();
        //helpMenu = GameObject.Find("Canvas/HelpMenu");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneLoader.instance.tutorial)
        {
            SetStartConditions();
            Debug.Log("Goals: " + allGoals.Count);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialRunning)
        {
            CheckGoals();
        }
    }

    // Creates all subtasks and adds them into a common list
    private void SetStartConditions()
    {
        foreach (Task_Base task in taskList)
        {
            task.OnStart();
            allGoals.AddRange(task.goals);
            Debug.Log("Task added: " + task.taskDescriptions.Count);
        }

        StartTutorial();
    }

    // Makes sure all tasks are completed
    private void CheckGoals()
    {
        Debug.Log("Task: " + currentIndex);
        // There are still tasks
        if (currentTask != null)
        {
            // Task completed
            if ((currentTask.taskCondition.Condition() && !assistent.ActiveWriter()) || currentTask.taskDescription.completed)
            {
                // Reset for next task
                //currentTask.taskCondition.Reset();
                currentTask.taskDescription.completed = true;
                activeTask = false;
                currentIndex++;
            }
            else    // Task not yet completed
            {
                // Show new task
                if (!activeTask)
                {
                    Debug.Log("Task written");

                    string tempDesc = SceneLoader.instance.English ? currentTask.taskDescription.descriptions[0] : currentTask.taskDescription.descriptions[1];
                    assistent.WriteMessage(tempDesc);

                    StartCoroutine(WaitForWriter());
                    activeTask = true;
                }
            }
        }
        else    // All tasks completed
        {
            EndTutorial();
        }

    }

    // Waits until the complete task meassage is displayed before starting with the task set up
    private IEnumerator WaitForWriter()
    {
        while (assistent.ActiveWriter())
        {
            // don't like that I think
            /*
            if (currentTask.taskCondition.Condition())
            {
                assistent.FinishMessage();
                break;
            }
            */

            yield return null;  // Wait for the next frame
        }

        currentTask.taskCondition.SetUp();
    }

    // Starts tutorial by enabling help menu
    private void StartTutorial()
    {
        boss.SetActive(false);
        helpMenu.SetActive(true);
        tutorialRunning = true;

        if (SceneLoader.instance.reloadTutorial)
        {
            SetTasks(false);
            SceneLoader.instance.reloadTutorial = false;
        }
    }

    // Stops/finishes tutorial by closing help menu
    public void EndTutorial()
    {
        // Reset the environment if the tutorial was skipped
        if (currentTask != null)
        {
            currentTask.taskCondition.Reset();
        }

        helpMenu.SetActive(false);
        boss.SetActive(true);
        tutorialRunning = false;
        SceneLoader.instance.tutorial = false;
    }

    // Sets all tasks completed/uncompleted
    private void SetTasks(bool completed)
    {
        foreach (SubTask task in allGoals)
        {
            task.taskDescription.completed = completed;
        }
    }

    protected override void LanguageChanged(bool isEnglish) 
    {
        // Finish current message
        assistent.FinishMessage();

        // Set new language
        string tempDesc = isEnglish ? currentTask.taskDescription.descriptions[0] : currentTask.taskDescription.descriptions[1];
        assistent.WriteMessage(tempDesc);
        assistent.FinishMessage();
    }
}