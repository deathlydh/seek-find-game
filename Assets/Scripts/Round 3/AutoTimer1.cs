using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public Image timerCircle; // Circular timer image
    public float timeLimit = 30f;
    private float currentTime;
    private bool timerRunning = false;

    public AutoQuizManager autoQuizManager;

    void Start()
    {
        currentTime = timeLimit;
        UpdateTimerText();
        UpdateTimerCircle();
    }

    public void StartTimer()
    {
        if (!timerRunning)
        {
            timerRunning = true;
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;

            UpdateTimerText();
            UpdateTimerCircle();

            if (currentTime <= 0)
            {
                Debug.Log("Время вышло.");
                autoQuizManager.EndAutoRound();
                timerRunning = false;
            }
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"{minutes}:{seconds:D2}";
    }

    private void UpdateTimerCircle()
    {
        if (timerCircle != null)
        {
            timerCircle.fillAmount = currentTime / timeLimit;
        }
    }
}
