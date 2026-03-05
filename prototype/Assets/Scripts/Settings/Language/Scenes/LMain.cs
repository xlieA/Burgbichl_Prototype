using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LMain : MySingleton<LMain>
{
    [SerializeField] private TMP_Text play;
    [SerializeField] private TMP_Text testVersion;
    [SerializeField] private TMP_Text intro;
    [SerializeField] private TMP_Text setting;
    [SerializeField] private TMP_Text exit;

    [SerializeField] private List<string> playText = new List<string>();
    [SerializeField] private List<string> testVersionText = new List<string>();
    [SerializeField] private List<string> introText = new List<string>();
    [SerializeField] private List<string> settingText = new List<string>();
    [SerializeField] private List<string> exitText = new List<string>();


    public override void UpdateUI()
    {
        if (SceneLoader.instance.English)
        {
            SetUI(play, playText[0]);
            SetUI(testVersion, testVersionText[0]);
            SetUI(intro, introText[0]);
            SetUI(setting, settingText[0]);
            SetUI(exit, exitText[0]);
        }
        else
        {
            SetUI(play, playText[1]);
            SetUI(testVersion, testVersionText[1]);
            SetUI(intro, introText[1]);
            SetUI(setting, settingText[1]);
            SetUI(exit, exitText[1]);
        }
    }
}
