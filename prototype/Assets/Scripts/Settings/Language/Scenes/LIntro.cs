using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LIntro : MySingleton<LIntro>
{
    [SerializeField] private TMP_Text boss;
    [SerializeField] private TMP_Text skip;

    [SerializeField] private List<string> bossText = new List<string>();
    [SerializeField] private List<string> skipText = new List<string>();
    [SerializeField] public List<string> contText = new List<string>();


    public override void UpdateUI()
    {
        if (SceneLoader.instance.English)
        {
            SetUI(boss, bossText[0]);
            SetUI(skip, skipText[0]);
        }
        else
        {
            SetUI(boss, bossText[1]);
            SetUI(skip, skipText[1]);
        }
    }
}
