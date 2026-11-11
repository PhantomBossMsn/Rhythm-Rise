using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeWriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    [TextArea]
    public string fullText = "This is typing text...";
    public float typingSpeed = 0.05f;
    public float startDelay = 1f;

    public AudioSource typingAudioSource;
    public AudioClip typingSound;

    public bool startTyping = false; // Trigger to begin typing
    public GameObject objectToActivate; // Object to activate after delay

    private bool hasStarted = false;

    void Update()
    {
        if (startTyping && !hasStarted)
        {
            hasStarted = true;
            StartCoroutine(TypeText());
        }
    }

    IEnumerator TypeText()
    {
        
        yield return new WaitForSeconds(startDelay);
        textComponent.text = "";
        if (objectToActivate != null)
            objectToActivate.SetActive(true); // Activate object after delay

        foreach (char letter in fullText)
        {
            textComponent.text += letter;

            if (typingSound && typingAudioSource)
            {
                typingAudioSource.PlayOneShot(typingSound);
            }

            yield return new WaitForSeconds(typingSpeed);
        }
    }
}