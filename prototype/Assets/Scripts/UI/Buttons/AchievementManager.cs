using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private MenuController menuController;
    // Temporary list of all buttons in scene (used to create Dict)
    private List<Button> temp = new List<Button>();
    [SerializeField] private GameObject closeButton;

    [SerializeField] private SpawnableObjectManager spawnableObjectManager;
    // List of all excavation sites that can be digged up
    private List<SpawnableObject> excavationSites;
    private List<SpawnableArtefact> artefacts;

    [SerializeField] private int numberOfExcavationSites = 6;

    // List to manage the buttons
    private Dictionary<string, ButtonElement> buttons = new Dictionary<string, ButtonElement>();

    [SerializeField] private Sprite questionMark;
    [SerializeField] private Color markColor;
    [SerializeField] private Color baseColor;
    private Color mColor = new Color(0.803f, 0.718f, 0.612f);

    [SerializeField] private bool debugging;


    void Awake()
    {
        menuController = GameObject.Find("MenuController").GetComponent<MenuController>();

        spawnableObjectManager = GameObject.Find("SpawnableObjectManager").GetComponent<SpawnableObjectManager>();
        excavationSites = spawnableObjectManager.excavationSites;
        artefacts = spawnableObjectManager.artefacts;
    }

    void Start()
    {
        menuController.excludeObjects.Add(closeButton);
        temp = menuController.GetAllButtons(menuController.canvas, menuController.excludeObjects);
        
        Debug.Log($"temp.Count: {temp.Count}, excavationSites.Count: {excavationSites.Count}, artefacts.Count: {artefacts.Count}");
        
        InitialSetUp();
        FillDict();
    }

    void Update()
    {
        if (debugging && Input.GetMouseButtonDown(0))
        {
            foreach (var b in buttons)
            {
                ChangeIcon(b.Value, true);
            }
        }
    }

    // Sets all buttons in the scene inactive at first
    private void InitialSetUp()
    {
        foreach (Button b in temp)
        {
            b.interactable = false;
        }
    }

    private void FillDict()
    {
        if (temp.Count < excavationSites.Count + artefacts.Count)
        {
            Debug.Log("Not enough buttons available in the 'temp' list to match all excavation sites and artefacts!");
            return; // Prevent further execution
        }

        for (int i = 0; i < excavationSites.Count; ++i)
        {
            Debug.Log($"Creating button for excavation site: {excavationSites[i].name}");
            CreateAchievementButton(excavationSites[i], i);
        }

        for (int i = 0; i < artefacts.Count; ++i)
        {
            Debug.Log($"Creating button for artefact: {artefacts[i].name}");
            CreateAchievementButton(artefacts[i], i + numberOfExcavationSites);
        }
    }


    // Matches a free button to each excavation site and artefact
    private void FillDict2()
    {
        for (int i = 0; i < excavationSites.Count; ++i)
        {
            Debug.Log("Create button for excavation site");
            CreateAchievementButton(excavationSites[i], i);
            Debug.Log("Create button for: " + excavationSites[i].name);
            Debug.Log("Button used: " + temp[i].name);
        }

        for (int i = 0; i < artefacts.Count; ++i)
        {
            Debug.Log("Create butoon for artefact");
            CreateAchievementButton(artefacts[i], i + numberOfExcavationSites);
            Debug.Log("Create button for: " + artefacts[i].name);
            Debug.Log("Button used: " + temp[i + numberOfExcavationSites].name);
        }
    }

    private void CreateAchievementButton(SpawnableObject obj, int pos)
    {
        if (pos >= temp.Count)
        {
            Debug.LogError($"Index {pos} is out of bounds for the button list! 'temp' only has {temp.Count} buttons.");
            return; // Prevent further execution
        }

        // Get button at the given position and match it with the object
        buttons.Add(obj.objectName, new ButtonElement(temp[pos].name, obj.img, obj));

        // Set button active if the object is spawned
        if (obj.spawned)
        {
            SetButtonActive(obj.objectName);
        }
    }


    // Assigns an object to a button
    private void CreateAchievementButton2(SpawnableObject obj, int pos)
    {
        // Get button at given position and match it with object
        buttons.Add(obj.objectName, new ButtonElement(temp[pos].name, obj.img, obj));

        // Set button active
        if (obj.spawned)
        {
            SetButtonActive(obj.objectName);
        }

        // Other buttons should already be set to question mark
    }

    // Sets a given buttons inactive
    private void SetButtonInactive(string name)
    {
        buttons[name].button.interactable = false;

        // Change icon
        ChangeIcon(buttons[name], false);
    }

    // Sets a given button active
    public void SetButtonActive(string name)
    {
        buttons[name].button.interactable = true;

        // Change icon
        ChangeIcon(buttons[name], true);
    }

    // Makes sure the correct icon is displayed
    private void ChangeIcon(ButtonElement button, bool active)
    {
        if (active)
        {
            button.button.image.sprite = button.img;
            button.button.image.color = baseColor;
        }
        else
        {
            button.button.image.sprite = questionMark;
            button.button.image.color = markColor;
        }
    }

    // Returns the information of a given button
    public SpawnableObject GetButtonInfo(Button b)
    {
        return buttons[b.name].obj;
    }
}
