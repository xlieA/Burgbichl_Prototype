// https://www.youtube.com/watch?v=ZVh4nH8Mayg&t=248s
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    private static TextWriter instance;
    private List<TextWriterSingle> textWriterSingleList = new List<TextWriterSingle>();


    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Text Writer instances: " + textWriterSingleList.Count);

        StartCoroutine(ProcessTextWritersList());
    }

    public TextWriterSingle AddWriter(TMP_Text message, string textToWrite, float timePerCharacter, bool invisibleCharacters, bool removeWriterBeforeAdd)
    {
        if (removeWriterBeforeAdd)
        {
            RemoveWriter(message);
        }

        // Start audio
        AudioManager.instance.StartTalking();

        TextWriterSingle textWriterSingle = new TextWriterSingle(message, textToWrite, timePerCharacter, invisibleCharacters);
        textWriterSingleList.Add(textWriterSingle);
        return textWriterSingle;
    }

    public static void RemoveWriter_Static(TMP_Text message)
    {
        instance.RemoveWriter(message);
    }

    private void RemoveWriter(TMP_Text message)
    {
        for (int i = 0; i < textWriterSingleList.Count; ++i)
        {
            if (textWriterSingleList[i].GetMessage() == message)
            {
                textWriterSingleList.RemoveAt(i);
                --i;
            }
        }
    }

    private IEnumerator ProcessTextWritersList()
    {
        for (int i = 0; i < textWriterSingleList.Count; ++i)
        {
            bool destroyInstance = textWriterSingleList[i].TypeWriter();

            while (!destroyInstance)
            {
                yield return null;
            }

            Debug.Log("Finished writing");
            textWriterSingleList.RemoveAt(i);
            --i;
        }
    }
}


// Represents a single TextWriter instance
public class TextWriterSingle
{
    private TMP_Text message;
    private string textToWrite;
    private int characterIndex;
    private bool invisibleCharacters;

    private float timePerCharacter;
    private float timer;


    public TextWriterSingle(TMP_Text message, string textToWrite, float timePerCharacter, bool invisibleCharacters)
    {
        this.message = message;
        this.textToWrite = textToWrite;
        this.invisibleCharacters = invisibleCharacters;
        this.timePerCharacter = timePerCharacter;
        this.timer = 0;
    }

    // Writes message charcacter per character
    public bool TypeWriter()
    {
        timer -= Time.deltaTime;
        while (timer <= 0f)
        {
            // Reset timer
            timer += timePerCharacter;

            // Display next character
            characterIndex++;

            if (characterIndex > textToWrite.Length)
            {
                AudioManager.instance.StopTalking();
                return true;
            }

            string text = textToWrite.Substring(0, characterIndex);

            // Characters appear at final position (no shifting while reading)
            if (invisibleCharacters)
            {
                // Add rest of the text "invisible" by setting color of the text
                text += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
            }

            message.text = text;
        }
        return false;
    }

    // Returns the text that is currently written
    public TMP_Text GetMessage()
    {
        return message;
    }

    // Shows if the TypeWriter effect is currently executed
    public bool IsActive()
    {
        return characterIndex < textToWrite.Length;
    }

    // Skip type writer effect and display text completely
    public void WriteAll()
    {
        if (IsActive())
        {
            message.text = textToWrite;
            characterIndex = textToWrite.Length;
            TextWriter.RemoveWriter_Static(message);
            AudioManager.instance.StopTalking();
        }
    }
}