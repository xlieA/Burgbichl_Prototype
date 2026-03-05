using TMPro;
using UnityEngine;

public class Assistent : MonoBehaviour
{
    [SerializeField] private TextWriter textWriter;
    private TextWriterSingle textWriterSingle;
    [SerializeField] public TMP_Text helpMessage;
    [SerializeField] private float timePerCharacter = .1f;
    [SerializeField] private bool invisibleCharacters = true;
    [SerializeField] private bool removeWriterBeforeAdd = true;


    void Awake()
    {
        textWriter = GameObject.Find("TextWriter").GetComponent<TextWriter>();
    }


    // Sets message that assistent should display
    public void WriteMessage(string message)
    {
        textWriterSingle = textWriter.AddWriter(helpMessage, message, timePerCharacter, invisibleCharacters, removeWriterBeforeAdd);
    }

    // Shows if the TypeWriter effect is currently executed
    public bool ActiveWriter()
    {
        if (textWriterSingle != null)
        {
            return textWriterSingle.IsActive();
        }

        return false;
    }

    // Displays complete meassage when clicked on text field
    public void FinishMessage()
    {
        if (textWriterSingle != null)
        {
            textWriterSingle.WriteAll();
        }
    }
}
