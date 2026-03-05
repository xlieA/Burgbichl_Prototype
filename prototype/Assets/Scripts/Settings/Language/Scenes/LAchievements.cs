using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LAchievements : MySingleton<LAchievements>
{
    [SerializeField] private TMP_Text sites;
    [SerializeField] private TMP_Text artefacts;

    [SerializeField] private List<string> sitesText = new List<string>();
    [SerializeField] private List<string> artefactsText = new List<string>();


    public override void UpdateUI()
    {
        if (SceneLoader.instance.English)
        {
            SetUI(sites, sitesText[0]);
            SetUI(artefacts, artefactsText[0]);
        }
        else
        {
            SetUI(sites, sitesText[1]);
            SetUI(artefacts, artefactsText[1]);
        }
    }
}
