using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeLimit = 30f;
    private float currentTime;

    public AnimalQuizManager animalQuizManager;

    void Start()
    {
        currentTime = timeLimit;
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;

            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            timerText.text = $"{minutes}:{seconds:D2}";

            if (currentTime <= 0)
            {
                Debug.Log("Время вышло.");
                animalQuizManager.EndGame(); // Заканчиваем игру при достижении 0
            }
        }
    }

    
}
