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

    public Image selectionOutline;

    private AnimalQuestionConfig currentQuestion;  // Текущий вопрос
    private string correctAnswer;
    private bool isAnimalFound = false;
    private bool isQuizStarted = false;


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
        isQuizStarted = true; // Устанавливаем флаг, что викторина началась
        LoadNewQuestion();
        SetButtonsInactive(); // Делаем кнопки неактивными при старте
    }

    private void Update()
    {
        if (!isQuizStarted) return; // Если викторина не началась, ничего не делаем

        bool isTouchDetected = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;

        if ((Input.GetMouseButtonDown(0) || isTouchDetected) && !isAnimalFound)
        {
            CheckAnimal();
        }
    }

    // Метод для проверки, было ли найдено животное
    void CheckAnimal()
    {
        Vector2 clickPosition;

        // Определяем позицию клика (мышь или сенсор)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            clickPosition = Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            clickPosition = Input.mousePosition;
        }
        else
        {
            return; // Если ничего не нажато, выходим из метода
        }

        // Переводим позицию клика из экранных координат в луч из камеры
        Ray ray = Camera.main.ScreenPointToRay(clickPosition);

        // Выполняем проверку на попадание по BoxCollider
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                // Проверяем, попали ли мы по текущему животному
                if (hit.collider.gameObject == currentAnimalInstance)
                {
                    isAnimalFound = true;
                    incorrectSelectionOverlay.SetActive(false); // Скрываем неправильный выбор

                    // Обновляем рамку для текущего животного
                    var animalCollider = currentAnimalInstance.GetComponent<BoxCollider>();
                    if (animalCollider != null && selectionOutline != null)
                    {
                        UpdateOutline(selectionOutline.rectTransform, animalCollider);
                        selectionOutline.gameObject.SetActive(true); // Показываем рамку
                    }


                    // Активируем кнопки и присваиваем ответы
                    SetButtonsActive();
                    ShuffleAndAssignAnswers();
                }
                else
                {
                    Debug.Log("Попали не в то животное.");
                    StartCoroutine(ShowIncorrectOverlay());
                }
            }
        }
        else
        {
            Debug.Log("Луч не нашел никаких объектов.");
            StartCoroutine(ShowIncorrectOverlay());
        }
    }

    private IEnumerator ShowIncorrectOverlay()
    {
        incorrectSelectionOverlay.SetActive(true); // Показываем неправильную рамку
        yield return new WaitForSeconds(1f);       // Ждем 1 секунду
        incorrectSelectionOverlay.SetActive(false); // Скрываем неправильную рамку
    }
    private void UpdateOutline(RectTransform outline, BoxCollider collider)
    {
        if (outline == null || collider == null) return;

        // Размер коллайдера (X и Y используются для UI, Z игнорируется)
        Vector3 colliderSize = collider.size;

        // Центр коллайдера в мировых координатах
        Vector3 worldCenter = collider.transform.TransformPoint(collider.center);

        // Позиция рамки в локальных координатах Canvas
        Vector2 canvasLocalPosition = animalContainer.transform.InverseTransformPoint(worldCenter);

        // Устанавливаем размер и позицию рамки
        outline.sizeDelta = new Vector2(colliderSize.x, colliderSize.y);
        outline.anchoredPosition = canvasLocalPosition;
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
        selectionOutline.gameObject.SetActive(false);
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
            var controller = button.GetComponent<QuizButtonController>();
            if (controller != null)
            {
                controller.SetInactive();
            }
        }
    }

    private void SetButtonsActive()
    {
        foreach (Button button in answerButtons)
        {
            var controller = button.GetComponent<QuizButtonController>();
            if (controller != null)
            {
                controller.SetActive();
            }
        }
    }

    private void OnAnswerSelected(string selectedAnswer)
    {
        foreach (Button button in answerButtons)
        {
            var controller = button.GetComponent<QuizButtonController>();
            if (controller == null) continue;

            string buttonAnswer = button.GetComponentInChildren<TextMeshProUGUI>().text;
            if (buttonAnswer == correctAnswer)
            {
                controller.SetCorrect(); // Устанавливаем спрайт правильного ответа
            }
            else if (buttonAnswer == selectedAnswer)
            {
                controller.SetIncorrect(); // Устанавливаем спрайт неправильного ответа
            }
            else
            {
                controller.SetInactive(); // Остальные кнопки возвращаются в неактивное состояние
            }
        }

        if (selectedAnswer == correctAnswer)
        {
            scoreManager.score++;
            Debug.Log("Правильный ответ!");
            // Запускаем эффект частиц в верхней части экрана
            if (correctAnswerParticles != null)
            {
                Vector3 topOfScreen = new Vector3(0, Camera.main.orthographicSize, 0);
                ParticleSystem particles = Instantiate(correctAnswerParticles, topOfScreen, Quaternion.identity);
                Destroy(particles.gameObject, 1.5f); // Уничтожаем систему частиц через 1 секунду
            }
        }
        else
        {
            Debug.Log("Неправильный ответ.");
        }

        // Загружаем новый вопрос с задержкой
        StartCoroutine(LoadNextQuestionWithDelay());
    }

    private IEnumerator LoadNextQuestionWithDelay()
    {
        yield return new WaitForSeconds(1.5f); // Ждем 1.5 секунды
        LoadNewQuestion();
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

   
   

    // Метод для окончания игры
    public void EndGame()
    {
        StopAllCoroutines();
        gameOverPanel.SetActive(true);
        scoreManager.finalScoreText.text = scoreManager.score.ToString();
    }
}
