using TMPro;
using UnityEngine;

public class LObjectInfo : MySingleton<LObjectInfo>
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text container;


    public override void UpdateUI()
    {
        if (SceneLoader.instance.English)
        {
            SetUI(title, SceneLoader.instance.objInfo.objectNames[0]);
            SetUI(container, SceneLoader.instance.objInfo.descriptions[0]);
        }
        else
        {
            SetUI(title, SceneLoader.instance.objInfo.objectNames[1]);
            SetUI(container, SceneLoader.instance.objInfo.descriptions[1]);
        }
    }
}
