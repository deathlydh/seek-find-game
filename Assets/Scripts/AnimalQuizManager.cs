using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AnimalQuizManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public AnimalQuestionConfig[] animalQuestions; // Массив вопросов
    private List<AnimalQuestionConfig> availableQuestions; // Список доступных вопросов
    public Button[] answerButtons;                 // Массив кнопок для ответов
    public GameObject animalContainer;             // Контейнер для префабов животных
    public GameObject incorrectSelectionOverlay;   // Объект с красной рамкой для неправильного выбора
    public ParticleSystem correctAnswerParticles;  // Префаб для частиц правильного ответа
    private GameObject currentAnimalInstance;      // Ссылка на текущий префаб животного
    public GameObject gameOverPanel;

   

    private AnimalQuestionConfig currentQuestion;  // Текущий вопрос
    private string correctAnswer;
    private bool isAnimalFound = false;

    private BoxCollider2D containerCollider;

    private void Start()
    {
        availableQuestions = new List<AnimalQuestionConfig>(animalQuestions);
        containerCollider = animalContainer.GetComponent<BoxCollider2D>();
        gameOverPanel.SetActive(false);
        incorrectSelectionOverlay.SetActive(false); // Скрываем рамку при старте
       
        SetButtonsInactive(); // Делаем кнопки неактивными при старте
    }

    // Метод для начала викторины
    public void StartQuiz()
    {
        LoadNewQuestion();
        SetButtonsInactive(); // Делаем кнопки неактивными при старте
    }

    private void Update()
    {
        bool isTouchDetected = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;

        if ((Input.GetMouseButtonDown(0) || isTouchDetected) && !isAnimalFound)
        {
            CheckAnimal();
        }
    }

    // Метод для проверки, было ли найдено животное
    void CheckAnimal()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

        // Проверка, совпадает ли нажатие с текущим животным на сцене
        if (hitCollider != null && hitCollider == currentAnimalInstance.GetComponent<Collider2D>())
        {
            isAnimalFound = true;
            incorrectSelectionOverlay.SetActive(false); // Скрываем неправильный выбор
            var animalOutline = currentAnimalInstance.GetComponent<AnimalOutline>();
            if (animalOutline != null)
            {
                animalOutline.SetOutlineActive(true);
            }
            SetButtonsActive(); // Делаем кнопки активными и меняем их цвет
            ShuffleAndAssignAnswers();
        }
        else
        {
            Debug.Log("Неправильное животное. Таймер продолжается.");
            StartCoroutine(ShowIncorrectOverlay());
        }
    }

    private IEnumerator ShowIncorrectOverlay()
    {
        incorrectSelectionOverlay.SetActive(true); // Показываем неправильную рамку
        yield return new WaitForSeconds(1f);       // Ждем 1 секунду
        incorrectSelectionOverlay.SetActive(false); // Скрываем неправильную рамку
    }

    // Метод для загрузки нового вопроса
    private void LoadNewQuestion()
    {
        // Проверяем, есть ли еще вопросы
        if (availableQuestions.Count == 0)
        {
            EndGame();
            return;
        }

        // Сбрасываем флаг и деактивируем кнопки
        isAnimalFound = false;
        SetButtonsInactive(); // Делаем кнопки неактивными
        incorrectSelectionOverlay.SetActive(false); // Скрываем неправильную рамку

        // Выбираем случайный вопрос из доступных
        int questionIndex = Random.Range(0, availableQuestions.Count);
        currentQuestion = availableQuestions[questionIndex];
        correctAnswer = currentQuestion.correctAnswer;
        availableQuestions.RemoveAt(questionIndex);

        // Удаляем предыдущий префаб животного, если он был
        if (currentAnimalInstance != null)
        {
            Destroy(currentAnimalInstance);
        }

        // Спавним новый префаб животного в центре контейнера
        Vector3 spawnPosition = containerCollider.bounds.center;
        currentAnimalInstance = Instantiate(currentQuestion.animalPrefab, spawnPosition, Quaternion.identity, animalContainer.transform);

        // Подгоняем размер префаба под контейнер
        FitPrefabToCollider(currentAnimalInstance, containerCollider);

    }

    // Метод для подгонки размера префаба под размер коллайдера
    private void FitPrefabToCollider(GameObject prefab, BoxCollider2D containerCollider)
    {
        SpriteRenderer spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Vector2 spriteSize = spriteRenderer.bounds.size;
            Vector2 colliderSize = containerCollider.bounds.size;

            Vector3 scale = prefab.transform.localScale;
            scale.x *= colliderSize.x / spriteSize.x;
            scale.y *= colliderSize.y / spriteSize.y;

            prefab.transform.localScale = scale;
        }
    }



    private void SetButtonsInactive()
    {
        foreach (Button button in answerButtons)
        {
            button.gameObject.SetActive(true); // Buttons remain visible
            button.interactable = false; // Disable button interaction
            button.GetComponent<Image>().color = Color.white; // Set to neutral color (could be glowing)
        }
    }

    private void SetButtonsActive()
    {
        foreach (Button button in answerButtons)
        {
            button.interactable = true; // Enable button interaction
            button.GetComponent<Image>().color = Color.blue; // Set to blue color when active
        }
    }

    // Метод для перемешивания списка
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

    // Метод для перемешивания списка
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

    // Метод для обработки выбора ответа
    private void OnAnswerSelected(string selectedAnswer)
    {
        if (selectedAnswer == correctAnswer)
        {
            scoreManager.score++;
            Debug.Log("Правильный ответ!");

            // Запускаем эффект частиц в верхней части экрана
            if (correctAnswerParticles != null)
            {
                Vector3 topOfScreen = new Vector3(0, Camera.main.orthographicSize, 0);
                ParticleSystem particles = Instantiate(correctAnswerParticles, topOfScreen, Quaternion.identity);
                Destroy(particles.gameObject, 1f); // Уничтожаем систему частиц через 1 секунду
            }
        }
        else
        {
            Debug.Log("Неправильный ответ.");
        }

        LoadNewQuestion();
    }

    // Метод для окончания игры
    public void EndGame()
    {
        StopAllCoroutines();
        gameOverPanel.SetActive(true);
        scoreManager.finalScoreText.text = scoreManager.score.ToString();
    }
}
