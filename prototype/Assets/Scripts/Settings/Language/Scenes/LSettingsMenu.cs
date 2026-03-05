using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LSettingsMenu : MySingleton<LSettingsMenu>
{
    [SerializeField] private TMP_Text settings;
    [SerializeField] private TMP_Text volume;
    [SerializeField] private TMP_Text master;
    [SerializeField] private TMP_Text music;
    [SerializeField] private TMP_Text effects;
    [SerializeField] private TMP_Text language;

    [SerializeField] private List<string> settingsText = new List<string>();
    [SerializeField] private List<string> volumeText = new List<string>();
    [SerializeField] private List<string> masterText = new List<string>();
    [SerializeField] private List<string> musicText = new List<string>();
    [SerializeField] private List<string> effectsText = new List<string>();
    [SerializeField] private List<string> languageText = new List<string>();


    public override void UpdateUI()
    {
        if (SceneLoader.instance.English)
        {
            SetUI(settings, settingsText[0]);
            SetUI(volume, volumeText[0]);
            SetUI(master, masterText[0]);
            SetUI(music, musicText[0]);
            SetUI(effects, effectsText[0]);
            SetUI(language, languageText[0]);
        }
        else
        {
            SetUI(settings, settingsText[1]);
            SetUI(volume, volumeText[1]);
            SetUI(master, masterText[1]);
            SetUI(music, musicText[1]);
            SetUI(effects, effectsText[1]);
            SetUI(language, languageText[1]);
        } 
    }
}
