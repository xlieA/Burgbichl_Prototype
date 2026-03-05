using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LTutorial : MySingleton<LIntro>
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text reset;

    [SerializeField] private List<string> titleText = new List<string>();
    [SerializeField] private List<string> resetText = new List<string>();


    public override void UpdateUI()
    {
        if (SceneLoader.instance.English)
        {
            SetUI(title, titleText[0]);
            SetUI(reset, resetText[0]);
        }
        else
        {
            SetUI(title, titleText[1]);
            SetUI(reset, resetText[1]);
        }
    }
}

