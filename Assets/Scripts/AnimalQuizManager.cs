using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AnimalQuizManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public AnimalQuestionConfig[] easyQuestions; // ˸���� �������
    public AnimalQuestionConfig[] hardQuestions; // ������� �������
    private List<AnimalQuestionConfig> availableEasyQuestions; // ��������� ����� �������
    private List<AnimalQuestionConfig> availableHardQuestions; // ��������� ������� �������
    private bool isEasyPhase = true; // ������ ��������� ��������
    public Button[] answerButtons;                 // ������ ������ ��� �������
    [SerializeField] private RectTransform animalContainer;             // ��������� ��� �������� ��������
    public GameObject incorrectSelectionOverlay;   // ������ � ������� ������ ��� ������������� ������
    public ParticleSystem correctAnswerParticles;  // ������ ��� ������ ����������� ������
    private GameObject currentAnimalInstance;      // ������ �� ������� ������ ���������
    public GameObject gameOverPanel;

    [SerializeField] private float fixOffsetX = 0f; // �������� �� X ��� �����
    [SerializeField] private float fixOffsetY = 0f; // �������� �� Y ��� �����

    public Image selectionOutline;

    private AnimalQuestionConfig currentQuestion;  // ������� ������
    private string correctAnswer;
    private bool isAnimalFound = false;
    private bool isQuizStarted = false;
    private bool isAnswerSelected = false;

    private BoxCollider2D containerCollider;

    [SerializeField] private BorderSwitcher border;

    private void Start()
    {
        availableEasyQuestions = new List<AnimalQuestionConfig>(easyQuestions);
        availableHardQuestions = new List<AnimalQuestionConfig>(hardQuestions);
        containerCollider = animalContainer.GetComponent<BoxCollider2D>();
        gameOverPanel.SetActive(false);
        //incorrectSelectionOverlay.SetActive(false); // �������� ����� ��� ������
        border.SetWrong(false);

        SetButtonsInactive(); // ������ ������ ����������� ��� ������
        SaveSystem.init();
    }

    // ����� ��� ������ ���������
    public void StartQuiz()
    {
        isQuizStarted = true; // ������������� ����, ��� ��������� ��������
        LoadNewQuestion();
        SetButtonsInactive(); // ������ ������ ����������� ��� ������
    }

    private void Update()
    {
        if (!isQuizStarted) return; // ���� ��������� �� ��������, ������ �� ������

        bool isTouchDetected = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;

        if ((Input.GetMouseButtonDown(0) || isTouchDetected) && !isAnimalFound)
        {
            CheckAnimal();
        }
    }

    // ����� ��� ��������, ���� �� ������� ��������
    void CheckAnimal()
    {
        Vector2 clickPosition;

        // ���������� ������� ����� (���� ��� ������)
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
            return; // ���� ������ �� ������, ������� �� ������
        }

        // ��������� ������� ����� �� �������� ��������� � ��� �� ������
        Ray ray = Camera.main.ScreenPointToRay(clickPosition);

        // ��������� �������� �� ��������� �� BoxCollider
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                // ���������, ������ �� �� �� �������� ���������
                if (hit.collider.gameObject == currentAnimalInstance)
                {
                    isAnimalFound = true;
                    //incorrectSelectionOverlay.SetActive(false); // �������� ������������ �����
                    border.SetWrong(false);

                    // ��������� ����� ��� �������� ���������
                    var animalCollider = currentAnimalInstance.GetComponent<BoxCollider>();
                    if (animalCollider != null && selectionOutline != null)
                    {
                        UpdateOutline(selectionOutline.rectTransform, animalCollider);
                        selectionOutline.gameObject.SetActive(true); // ���������� �����
                    }


                    // ���������� ������ � ����������� ������
                    SetButtonsActive();
                    
                }
                else
                {
                    Debug.Log("������ �� � �� ��������.");
                    StartCoroutine(ShowIncorrectOverlay());
                }
            }
        }
        else
        {
            Debug.Log("��� �� ����� ������� ��������.");
            StartCoroutine(ShowIncorrectOverlay());
        }
    }

    private IEnumerator ShowIncorrectOverlay()
    {
        //incorrectSelectionOverlay.SetActive(true); // ���������� ������������ �����
        border.SetWrong(true);
        yield return new WaitForSeconds(1f);       // ���� 1 �������
        //incorrectSelectionOverlay.SetActive(false); // �������� ������������ �����
        border.SetWrong(false);
    }
    private void UpdateOutline(RectTransform outline, BoxCollider collider)
    {
        if (outline == null || collider == null) return;

        // ����� � ������� ����������
        Vector3 colliderSize = collider.size;
        Vector3 colliderCenter = collider.center;

        // �������� (�������� ����� ���������)
        Vector3 offset = new Vector3(fixOffsetX, fixOffsetY, 0);

        // ����������� ����� � ������ ��������
        Vector3 worldCenter = collider.transform.TransformPoint(colliderCenter + offset);
        Vector3 worldSize = Vector3.Scale(colliderSize, collider.transform.lossyScale);

        // ��������� � ��������� ���������� Canvas
        Vector2 canvasLocalPosition = animalContainer.transform.InverseTransformPoint(worldCenter);

        // ������������� ������ � ������� �����
        outline.sizeDelta = new Vector2(worldSize.x / animalContainer.transform.lossyScale.x, worldSize.y / animalContainer.transform.lossyScale.y);
        outline.anchoredPosition = canvasLocalPosition;
    }

    // ����� ��� �������� ������ �������
    private void LoadNewQuestion()
    {
        // Определяем текущий список доступных вопросов
        List<AnimalQuestionConfig> currentAvailableQuestions = isEasyPhase ? availableEasyQuestions : availableHardQuestions;

        // Проверяем, есть ли еще доступные вопросы
        if (currentAvailableQuestions.Count == 0)
        {
            if (isEasyPhase)
            {
                // Если закончились вопросы для легкой фазы, переключаемся на сложные
                isEasyPhase = false;
                LoadNewQuestion();
                return;
            }
            else
            {
                // Если вопросы закончились полностью, завершить игру
                EndGame();
                return;
            }
        }

        // Флаг, указывающий, нашли ли мы еще неиспользованный вопрос
        bool foundUnusedQuestion = false;
        int questionIndex = 0;

        // Попытка найти вопрос, который еще не был использован
        for (int i = 0; i < currentAvailableQuestions.Count; i++)
        {
            questionIndex = Random.Range(0, currentAvailableQuestions.Count);
            AnimalQuestionConfig potentialQuestion = currentAvailableQuestions[questionIndex];

            if (!UsedQuestionsManager.Instance.IsUsed(potentialQuestion))
            {
                currentQuestion = potentialQuestion;
                UsedQuestionsManager.Instance.MarkAsUsed(currentQuestion); // Отмечаем вопрос как использованный
                currentAvailableQuestions.RemoveAt(questionIndex);
                foundUnusedQuestion = true;
                break;
            }
        }

        if (!foundUnusedQuestion)
        {
            // Если не найден ни один новый вопрос, рекурсивно вызываем метод снова
            EndGame();
            return;
        }

        // Сбрасываем состояние для нового вопроса
        isAnimalFound = false;
        selectionOutline.gameObject.SetActive(false);
        SetButtonsInactive();
        border.SetWrong(false);

        // Устанавливаем правильный ответ
        correctAnswer = currentQuestion.correctAnswer;

        // Удаляем предыдущий экземпляр животного, если он есть
        if (currentAnimalInstance != null)
        {
            Destroy(currentAnimalInstance);
        }

        // Создаем новый экземпляр животного
        Vector3 spawnPosition = containerCollider.bounds.center;
        currentAnimalInstance = Instantiate(currentQuestion.animalPrefab, spawnPosition, Quaternion.identity, animalContainer.transform);

        SaveSystem.setPassPool(currentAnimalInstance.name);

        // Подгоняем размер животного под контейнер
        FitPrefabToCollider(currentAnimalInstance, containerCollider);

        // Предзагружаем ответы для текущего вопроса
        PreloadAnswers();
    }

    // ����� ��� ��������� ������� � ������ ��� ���������
    private void PreloadAnswers()
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

    // ����� ��� �������� ������� ������� ��� ������ ����������
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
        if (isAnswerSelected) return; // Если уже выбран ответ, игнорируем нажатие
        isAnswerSelected = true; // Устанавливаем флаг

        foreach (Button button in answerButtons)
        {
            var controller = button.GetComponent<QuizButtonController>();
            if (controller == null) continue;

            string buttonAnswer = button.GetComponentInChildren<TextMeshProUGUI>().text;
            if (buttonAnswer == correctAnswer)
            {
                controller.SetCorrect(); // Обновляем стиль кнопки как правильной
            }
            else if (buttonAnswer == selectedAnswer)
            {
                controller.SetIncorrect(); // Обновляем стиль кнопки как неправильной
            }
            else
            {
                controller.SetInactive(); // Деактивируем остальные кнопки
            }
        }

        if (selectedAnswer == correctAnswer)
        {
            scoreManager.score++;
            SaveSystem.Save(scoreManager.score);
            Debug.Log("Правильный ответ!");
            if (correctAnswerParticles != null)
            {
                Vector3 topOfScreen = new Vector3(0, Camera.main.orthographicSize, 0);
                ParticleSystem particles = Instantiate(correctAnswerParticles, topOfScreen, Quaternion.identity);
                Destroy(particles.gameObject, 1.5f);
            }
        }
        else
        {
            Debug.Log("Неправильный ответ.");
        }

        // Загружаем следующий вопрос с задержкой
        StartCoroutine(LoadNextQuestionWithDelay());
    }

    private IEnumerator LoadNextQuestionWithDelay()
    {
        yield return new WaitForSeconds(1.5f); // Ждем 1.5 секунды
        isAnswerSelected = false; // Сбрасываем флаг
        LoadNewQuestion(); // Загружаем новый вопрос
    }


    // ����� ��� ������������� ������
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

   
   

    // ����� ��� ��������� ����
    public void EndGame()
    {
        SaveSystem.Save(scoreManager.score);
        StopAllCoroutines();
        gameOverPanel.SetActive(true);
        scoreManager.finalScoreText.text = scoreManager.score.ToString();
    }
}
