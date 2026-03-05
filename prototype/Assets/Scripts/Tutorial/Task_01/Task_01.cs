using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Task_01 : Task_Base
{
    // First condition
    [SerializeField] private Button compass;

    // Second condition
    [SerializeField] private GameObject errorObject;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private Color baseColorText;
    [SerializeField] private Color highlightColorText;


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
    }

    private bool FirstCondition()
    {
        if (IsButtonClicked(compass) || buttonManager.IsActive(compass.name))
        {
            Debug.Log("Button clicked: " + IsButtonClicked(compass));
            return true;
        }

        return false;
    }

    private void FirstSetUp()
    {
        SetupButtonListener(compass);
        compass.image.color = highlightColor;
    }

    private void FirstReset()
    {
        ResetButtonListener(compass);
        compass.image.color = baseColor;
    }

    private bool SecondCondition()
    {
        string temp = SceneLoader.instance.English ? languageManager.reachedSiteText[0] : languageManager.reachedSiteText[1];
        if (errorText.text == temp && !errorObject.activeSelf)
        {
            SecondReset();
            return true;
        }

        return false;
    }

    private void SecondSetUp()
    {
        string temp = SceneLoader.instance.English ? languageManager.reachedSiteText[0] : languageManager.reachedSiteText[1];
        if (errorText.text == temp)
        {
            errorObject.GetComponent<Image>().color = highlightColorText;
        }
    }

    private void SecondReset()
    {
        errorObject.GetComponent<Image>().color = baseColorText;
    }
}
