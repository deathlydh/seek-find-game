using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class AnimalSearch : MonoBehaviour
{
    public GameObject[] photographs;
    public GameObject[] animalButtons; // Кнопки с названиями животных (GameObject для SetActive)
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel; // Панель, показывающая счёт после окончания игры
    public TextMeshProUGUI finalScoreText; // Текст с финальным счётом
    public float timeLimit = 30f;

    public string[][] animalOptions; // Массив массивов с вариантами для каждой фотографии
    public string[] correctAnswers; // Массив с правильными ответами для каждой фотографии
    private int currentPhotoIndex = 0; // Индекс текущей фотографии
    private int score = 0; // Очки
    private float currentTime; // Текущее время таймера
    private bool isAnimalFound = false; // Проверка нахождения животного
    private List<int> shownPhotos = new List<int>(); // Список показанных фотографий
    private Button correctButton; // Ссылка на правильную кнопку для текущего раунда

    void Start()
    {
        animalOptions = new string[][]
   {
        new string[] { "Косуля", "Жираф", "Кабан" },  // Варианты для первой фотографии
        new string[] { "Лиса", "Волк", "Енот" },
        new string[] { "Жираф", "Лев", "Енот" },
        new string[] { "Медведь", "Лиса", "Заяц" },
        new string[] { "Косуля", "Выдра", "Рысь" }// Варианты для второй фотографии
                                                    // Добавьте остальные варианты для других фотографий
   };

        correctAnswers = new string[] { "Олень", "Рысь", "Олень", "Волк", "Лиса" };
        currentTime = timeLimit;
        SetButtonsActive(false);
        StartCoroutine(Timer());
        AssignButtonListeners();
        gameOverPanel.SetActive(false); // Скрываем панель в начале игры
        UpdatePhotograph();
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

        if (hitCollider != null && hitCollider == photographs[currentPhotoIndex].GetComponent<Collider2D>())
        {
            isAnimalFound = true;
            score++;
            UpdateScore();
            PlusTimer();
            SetButtonsActive(true);
            ShuffleAndAssignAnswers();
        }
        else
        {
            Debug.Log("Неправильное животное. Таймер продолжается.");
            MinusTimer();
        }
    }

    void ShuffleAndAssignAnswers()
    {
        // Получаем возможные варианты ответа для текущей фотографии
        string[] currentOptions = animalOptions[currentPhotoIndex];

        // Создаем список из вариантов и правильного ответа
        List<string> shuffledOptions = currentOptions.ToList();
        shuffledOptions.Add(correctAnswers[currentPhotoIndex]);

        // Перемешиваем список
        shuffledOptions = shuffledOptions.OrderBy(a => Random.Range(0, shuffledOptions.Count)).ToList();

        // Присваиваем перемешанные варианты кнопкам
        for (int i = 0; i < animalButtons.Length; i++)
        {
            Button button = animalButtons[i].GetComponent<Button>();
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = shuffledOptions[i];

            // Если это правильный ответ, сохраняем ссылку на правильную кнопку
            if (shuffledOptions[i] == correctAnswers[currentPhotoIndex])
            {
                correctButton = button;
            }
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

        photographs[currentPhotoIndex].SetActive(false);

        if (shownPhotos.Count >= photographs.Length)
        {
            EndGame(); // Закончить игру, если все фотографии показаны
            return;
        }

        do
        {
            currentPhotoIndex = Random.Range(0, photographs.Length);
        }
        while (shownPhotos.Contains(currentPhotoIndex)); // Ищем непоказанную фотографию

        shownPhotos.Add(currentPhotoIndex); // Добавляем её в список показанных

        UpdatePhotograph();
    }

    void UpdatePhotograph()
    {
        // Добавляем первую фотографию в список показанных сразу
        if (!shownPhotos.Contains(currentPhotoIndex))
        {
            shownPhotos.Add(currentPhotoIndex);
        }

        photographs[currentPhotoIndex].SetActive(true);
    }

    void SetButtonsActive(bool active)
    {
        foreach (GameObject button in animalButtons)
        {
            button.SetActive(active);
        }
    }

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

            timerText.text = $"{minutes}:{seconds:D2}";

            if (currentTime <= 0)
            {
                Debug.Log("Время вышло.");
                EndGame(); // Заканчиваем игру при достижении 0
            }
        }
    }

    void EndGame()
    {
        StopAllCoroutines(); // Останавливаем таймер
        gameOverPanel.SetActive(true); // Показываем панель с результатом
        finalScoreText.text = score.ToString(); // Показываем финальный счёт
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
        if (clickedButton == correctButton)
        {
            score++;
            UpdateScore();
            Debug.Log("Правильное животное выбрано!");
            NextPhotograph();
        }
        else
        {
            Debug.Log("Неправильное животное. Попробуйте снова.");
            MinusTimer();
        }
    }
}
