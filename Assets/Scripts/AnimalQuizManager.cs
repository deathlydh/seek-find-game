using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AnimalQuizManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public AnimalQuestionConfig[] animalQuestions; // Массив с вопросами
    private List<AnimalQuestionConfig> availableQuestions; // Список доступных вопросов
    public Button[] answerButtons;                 // Массив с кнопками для ответов
    public GameObject animalContainer;             // Контейнер для префабов животных
    private GameObject currentAnimalInstance;      // Ссылка на текущий префаб животного                  
    public GameObject gameOverPanel;

    private AnimalQuestionConfig currentQuestion;  // Текущий вопрос
    private string correctAnswer;
    private bool isAnimalFound = false;

    private void Start()
    {
        availableQuestions = new List<AnimalQuestionConfig>(animalQuestions); // Копируем вопросы в список доступных вопросов
        LoadNewQuestion();
        gameOverPanel.SetActive(false);
        SetButtonsActive(false);
    }

    private void Update()
    {
        bool isTouchDetected = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;

        if ((Input.GetMouseButtonDown(0) || isTouchDetected) && !isAnimalFound)
        {
            CheckAnimal();
        }
    }

    void CheckAnimal()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

        // Проверяем, что кликнутый объект совпадает с текущим животным на сцене
        if (hitCollider != null && hitCollider == currentAnimalInstance.GetComponent<Collider2D>())
        {
            isAnimalFound = true;

            SetButtonsActive(true);
            ShuffleAndAssignAnswers();
        }
        else
        {
            Debug.Log("Неправильное животное. Таймер продолжается.");
        }
    }

    private void LoadNewQuestion()
    {
        // Проверяем, остались ли вопросы
        if (availableQuestions.Count == 0)
        {
            EndGame();
            return;
        }

        // Сбрасываем флаг и отключаем кнопки
        isAnimalFound = false;
        SetButtonsActive(false);

        // Выбираем случайный вопрос из доступных и удаляем его из списка
        int questionIndex = Random.Range(0, availableQuestions.Count);
        currentQuestion = availableQuestions[questionIndex];
        correctAnswer = currentQuestion.correctAnswer;
        availableQuestions.RemoveAt(questionIndex);

        // Удаляем предыдущий префаб, если он есть
        if (currentAnimalInstance != null)
        {
            Destroy(currentAnimalInstance);
        }

        // Спавним новый префаб в контейнере
        currentAnimalInstance = Instantiate(currentQuestion.animalPrefab, animalContainer.transform);
    }

    private void SetButtonsActive(bool isActive)
    {
        foreach (Button button in answerButtons)
        {
            button.gameObject.SetActive(isActive);
        }
    }

    private void ShuffleAndAssignAnswers()
    {
        List<string> shuffledAnswers = new List<string>(currentQuestion.answerOptions);
        ShuffleList(shuffledAnswers);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = shuffledAnswers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(shuffledAnswers[index]));
        }
    }

    private void ShuffleList(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            string temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void OnAnswerSelected(string selectedAnswer)
    {
        if (selectedAnswer == correctAnswer)
        {
            scoreManager.score++;
            scoreManager.UpdateScore();
            Debug.Log("Правильно!");
        }
        else
        {
            Debug.Log("Неправильно.");
        }

        // Загружаем следующий вопрос после ответа
        LoadNewQuestion();
    }

    public void EndGame()
    {
        StopAllCoroutines();
        gameOverPanel.SetActive(true);
        scoreManager.finalScoreText.text = scoreManager.score.ToString();
    }
}
