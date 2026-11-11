using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransmisionRANK : MonoBehaviour
{
    public bool useTwo = false;

    public ONERytBeatManager oneManager;
    public TWORytBeatManager twoManager;

    public float currentAccuracy;
    public float percentScore;
    public string grade;

    public TextMeshProUGUI scoreText;
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    void Update()
    {
        // Get AccuracyState from selected manager
        if (useTwo && twoManager != null)
            currentAccuracy = twoManager.AccuracyState;
        else if (!useTwo && oneManager != null)
            currentAccuracy = oneManager.AccuracyState;

        // Calculate percentage and grade
        percentScore = currentAccuracy * 100f;
        if (percentScore > 100) { percentScore=100; }
        grade = GetStylizedGrade(percentScore);

        // Display in TMP
        if (scoreText != null)
            scoreText.text = $"<b>Accuracy:</b> {percentScore:F1}%\n<b>Grade:</b> {grade}";
    }

    string GetStylizedGrade(float percent)
    {
        if (percent >= 95f) return "S";
        else if (percent >= 90f) return "A";
        else if (percent >= 80f) return "B";
        else if (percent >= 70f) return "C";
        else if (percent >= 60f) return "D";
        else if (percent >= 50f) return "E";
        else return "F";
    }

    // ?? Restart the current scene
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ? Load a specific scene by name
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}

