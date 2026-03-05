using TMPro;
using UnityEngine;

public class LanguageManager : LanguageListener
{
    protected override void LanguageChanged(bool isEnglish)
    {
        UpdateUI();
    }

    public void SetEnglish()
    {
        AudioManager.instance.Button();
        SceneLoader.instance.English = true;
        Debug.Log("Language set English");
        UpdateUI();
    }

    public void SetGerman()
    {
        AudioManager.instance.Button();
        SceneLoader.instance.English = false;
        Debug.Log("Language set German");
        UpdateUI();
    }

    // Sets the text of a given object to a given string
    public void SetUI(TMP_Text textObj, string newText)
    {
        textObj.text = newText;
    }

    // Updates UI based on set language
    public virtual void UpdateUI() { }
}