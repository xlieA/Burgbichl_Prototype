using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Task_Base : MonoBehaviour
{
    [SerializeField] public Color baseColor;
    [SerializeField] public Color highlightColor;

    [SerializeField] public List<TaskDescription> taskDescriptions = new List<TaskDescription>();
    [HideInInspector] public List<SubTask> goals = new List<SubTask>();

    [SerializeField] public ButtonManager buttonManager;

    private Dictionary<Button, bool> buttonClicks = new Dictionary<Button, bool>();

    [SerializeField] public LGPS languageManager;


    void Awake()
    {
        buttonManager = GameObject.Find("ButtonManager").GetComponent<ButtonManager>();
        languageManager = GameObject.Find("LanguageManager").GetComponent<LGPS>();
    }

    // Sets the goals per task
    public virtual void OnStart() { }

    // Checks if a given button was clicked
    public bool IsButtonClicked(Button b)
    {
        return buttonClicks.ContainsKey(b) && buttonClicks[b];
    }


    // Attaches listener to button
    public void SetupButtonListener(Button b)
    {
        if (!buttonClicks.ContainsKey(b))
        {
            buttonClicks[b] = false; // Initialize button tracking

            b.onClick.AddListener(() =>
            {
                buttonClicks[b] = true;
                Debug.Log("Button clicked: " + b.name);
            });
        }
    }

    // Reset after task is complete
    public void ResetButtonListener(Button b)
    {
        if (buttonClicks.ContainsKey(b))
        {
            buttonClicks[b] = false;
        }
    }
}
