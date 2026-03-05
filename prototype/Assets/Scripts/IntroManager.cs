using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [SerializeField] public Assistent assistent;
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject helpMenu;
    [SerializeField] private TMP_Text buttonText;

    // Story
    [SerializeField] [TextArea] private List<string> introTextE = new List<string>();
    [SerializeField] [TextArea] private List<string> introTextG = new List<string>();
    private List<string> introText = new List<string>();
    
    private int index = 0;
    private bool started = false;
    

    void Awake()
    {
        // isn't working?
        //assistent = GameObject.Find("Canvas/HelpMenu").GetComponent<Assistent>();

        if (SceneLoader.instance.English)
        {
            introText = introTextE;
        }
        else
        {
            introText = introTextG;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!started && helpMenu.activeSelf)
        {
            StartStory();
        }

        ContinueToMain();
    }

    // NPC introduces story
    public void AcceptCall()
    {
        controls.SetActive(false);
        helpMenu.SetActive(true);
        AudioManager.instance.StopRing();
    }

    // Directly starts story
    private void StartStory()
    {
        if (index == 0 && index < introText.Count)
        {
            assistent.WriteMessage(introText[index]);
            index++;
            started = true;
        }
    }

    // Continues story
    public void ContinueStory()
    {
        if (assistent.ActiveWriter())
        {
            assistent.FinishMessage();
        }
        else if (index < introText.Count && started)    // Wait till writer finishes current text
        {
            assistent.WriteMessage(introText[index]);
            index++;
        }
    }

    // Sets button to continue to main menu
    public void ContinueToMain()
    {
        if (index >= introText.Count)
        {
            buttonText.text = SceneLoader.instance.English ? 
                LIntro.instance.contText[0] 
                : LIntro.instance.contText[1];
        }
    }

}
