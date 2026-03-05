using UnityEngine;

public class LanguageListener : MonoBehaviour
{
    private void OnEnable()
    {
        SceneLoader.OnLanguageChanged += LanguageChanged;
    }

    private void OnDisable()
    {
        SceneLoader.OnLanguageChanged -= LanguageChanged;
    }

    protected virtual void LanguageChanged(bool isEnglish) { }
}
