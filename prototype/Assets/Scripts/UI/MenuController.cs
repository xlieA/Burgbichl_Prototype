// https://discussions.unity.com/t/how-to-get-an-array-of-all-buttons-attached-to-a-panel/143504
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] public GameObject canvas;

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject menuPanel;
    [HideInInspector] public List<GameObject> excludeObjects = new List<GameObject>();

    [HideInInspector] public List<Button> buttons;


    // Start is called before the first frame update
    void Start()
    {
        excludeObjects.Add(settingsMenu);
        buttons = GetAllButtons(canvas, excludeObjects);

        foreach (Button b in buttons)
        {
            Debug.Log("Button added: " + b.name);
        }
    }

    public void OpenSettingsMenuFromMain()
    {
        AudioManager.instance.Button();

        menuPanel.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettingsMenuFromMain()
    {
        AudioManager.instance.Button();

        menuPanel.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void OpenSettingsMenu()
    {
        AudioManager.instance.Button();

        settingsMenu.SetActive(true);
        Debug.Log("Main Menu opened");

        foreach (Button b in buttons)
        {
            b.interactable = false;
            Debug.Log("Button inactive: " + b);
        }
    }

    public void CloseSettingsMenu(bool playAudio)
    {
        AudioManager.instance.Button();
        
        settingsMenu.SetActive(false);
        Debug.Log("Main Menu closed");

        foreach (Button b in buttons)
        {
            b.interactable = true;
            Debug.Log("Button active: " + b);
        }
    }

    public void SetActive(List<Button> buttons)
    {
        foreach (Button b in buttons)
        {
            b.interactable = true;
        }
    }

    public void SetInactive(List<Button> buttons)
    {
        foreach (Button b in buttons)
        {
            b.interactable = false;
        }
    }
    
    // Returns a list of all buttons in the scene that are not part of the exclude object
    public List<Button> GetAllButtons(GameObject canvas, List<GameObject> excludeObjects)
    {
        List<Button> buttons = new List<Button>();
        Button[] allButtons = canvas.GetComponentsInChildren<Button>(true);
        List<Button> excludeButtons = new List<Button>();

        foreach (GameObject obj in excludeObjects)
        {
            Button[] buttonsInObject = obj.GetComponentsInChildren<Button>(true);
            foreach (Button button in buttonsInObject)
            {
                if (button != null)
                {
                    excludeButtons.Add(button);
                }
            }
        }

        foreach (Button b in allButtons)
        {
            if (b.gameObject.scene.isLoaded && !excludeButtons.Contains(b))
            {
                buttons.Add(b);
            }
        }

        return buttons;
    }
}
