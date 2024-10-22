using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalSearch : MonoBehaviour
{
    public GameObject[] photographs;
    public GameObject[] animalButtons; // Кнопки с названиями животных (GameObject для SetActive)
    public TextMeshProUGUI timerText; 
    public TextMeshProUGUI scoreText; 
    public float timeLimit = 30f; 
    public Button correctAnimalName; // Имя правильного животного для текущей фотографии


    private int currentPhotoIndex = 0; // Индекс текущей фотографии
    private int score = 0; // Очки
    private float currentTime; // Текущее время таймера
    private bool isAnimalFound = false; // Проверка нахождения животного

    void Start()
    {
        currentTime = timeLimit;
        SetButtonsActive(false);
        StartCoroutine(Timer());
        UpdatePhotograph();
        AssignButtonListeners();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAnimalFound)
        {
            CheckAnimal(); 
        }
    }

    void CheckAnimal()
    {

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

        // Проверяем, попал ли клик в триггер зоны текущего спрайта
        if (hitCollider != null && hitCollider == photographs[currentPhotoIndex].GetComponent<Collider2D>())
        {
            isAnimalFound = true;
            score++;
            UpdateScore();
            PlusTimer();
            SetButtonsActive(true);
        }
        else
        {
            Debug.Log("Неправильное животное. Таймер продолжается.");
            MinusTimer();
        }
    }

    
    void UpdateScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    
    void NextPhotograph()
    {
        isAnimalFound = false; 
        SetButtonsActive(false);

        // Выключаем текущую фотографию
        photographs[currentPhotoIndex].SetActive(false);

        // Переходим к следующей фотографии
        currentPhotoIndex = (currentPhotoIndex + 1) % photographs.Length;

        // Включаем новую фотографию
        UpdatePhotograph();
        ResetTimer();
    }

    
    void UpdatePhotograph()
    {
        photographs[currentPhotoIndex].SetActive(true);
    }

  
    void SetButtonsActive(bool active)
    {
        foreach (GameObject button in animalButtons)
        {
            button.SetActive(active); 
        }
    }

    // Сброс таймера
    void ResetTimer()
    {
        currentTime = timeLimit;
    }

    void PlusTimer()
    {
        currentTime += 10;
    }
    void MinusTimer()
    {
        currentTime -= 5;
    }



    IEnumerator Timer()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;

            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            // Обновляем текст таймера в формате 0:30
            timerText.text = $"{minutes}:{seconds:D2}";

            if (currentTime <= 0)
            {
                Debug.Log("Время вышло.");
                NextPhotograph(); 
            }
        }
    }

    void AssignButtonListeners()
    {
        foreach (GameObject buttonObj in animalButtons)
        {
            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => OnAnimalButtonClick(button));
        }
    }

    void OnAnimalButtonClick(Button clickedButton)
    {
        if (clickedButton == correctAnimalName)
        {
            score++; // Увеличить счет за правильный выбор
            UpdateScore();
            Debug.Log("Правильное животное выбрано!");
            NextPhotograph(); // Переключаемся на следующую фотографию
        }
        else
        {
            Debug.Log("Неправильное животное. Попробуйте снова.");
            MinusTimer(); // Уменьшаем таймер за неправильный выбор
        }
    }
}
