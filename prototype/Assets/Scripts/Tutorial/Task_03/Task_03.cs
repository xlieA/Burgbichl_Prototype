using UnityEngine;
using UnityEngine.UI;

public class Task_03 : Task_Base
{
    // First condition
    [SerializeField] private Button pickaxe;

    // Second condition
    [SerializeField] private Button analysis;

    // Third condition
    [SerializeField] private Button bag;


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
        return (IsButtonClicked(pickaxe) || buttonManager.IsActive(pickaxe.name)) && analysis.isActiveAndEnabled;
    }

    private void FirstSetUp()
    {
        SetupButtonListener(pickaxe);
        pickaxe.image.color = highlightColor;
    }

    private void FirstReset()
    {
        ResetButtonListener(pickaxe);
        pickaxe.image.color = baseColor;
    }

    private bool SecondCondition()
    {
        return IsButtonClicked(analysis);
    }

    private void SecondSetUp()
    {
        SetupButtonListener(analysis);
        analysis.image.color = highlightColor;
    }
    private void SecondReset()
    {
        ResetButtonListener(analysis);
        analysis.image.color = baseColor;
    }

    private bool ThirdCondition()
    {
        return IsButtonClicked(bag) || buttonManager.IsActive(bag.name);
    }

    private void ThirdSetUp()
    {
        SetupButtonListener(bag);
        bag.image.color = highlightColor;
    }

    private void ThirdReset()
    {
        ResetButtonListener(bag);
        bag.image.color = baseColor;
    }
}
