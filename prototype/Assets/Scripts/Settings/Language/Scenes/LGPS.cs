using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LGPS : MySingleton<LGPS>
{
    [SerializeField] private TMP_Text analysis;
    [SerializeField] private TMP_Text tutorial;

    [SerializeField] private List<string> analysisText = new List<string>();
    [SerializeField] private List<string> tutorialText = new List<string>();
    [SerializeField] public List<string> gpsText = new List<string>();
    [SerializeField] public List<string> reachedSiteText = new List<string>();
    [SerializeField] public List<string> excavatedAllText = new List<string>();
    [SerializeField] public List<string> couldntFindText = new List<string>();


    public override void UpdateUI()
    {
        if (SceneLoader.instance.English)
        {
            SetUI(analysis, analysisText[0]);
            SetUI(tutorial, tutorialText[0]);
        }
        else
        {
            SetUI(analysis, analysisText[1]);
            SetUI(tutorial, tutorialText[1]);
        }
    }
}
