using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AnimalQuizManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public AnimalQuestionConfig[] animalQuestions; // ������ ��������
    private List<AnimalQuestionConfig> availableQuestions; // ������ ��������� ��������
    public Button[] answerButtons;                 // ������ ������ ��� �������
    public GameObject animalContainer;             // ��������� ��� �������� ��������
    public GameObject incorrectSelectionOverlay;   // ������ � ������� ������ ��� ������������� ������
    public ParticleSystem correctAnswerParticles;  // ������ ��� ������ ����������� ������
    private GameObject currentAnimalInstance;      // ������ �� ������� ������ ���������
    public GameObject gameOverPanel;

    public Image selectionOutline;

    private AnimalQuestionConfig currentQuestion;  // ������� ������
    private string correctAnswer;
    private bool isAnimalFound = false;
    private bool isQuizStarted = false;


    private BoxCollider2D containerCollider;

    private void Start()
    {
        availableQuestions = new List<AnimalQuestionConfig>(animalQuestions);
        containerCollider = animalContainer.GetComponent<BoxCollider2D>();
        gameOverPanel.SetActive(false);
        incorrectSelectionOverlay.SetActive(false); // �������� ����� ��� ������

        SetButtonsInactive(); // ������ ������ ����������� ��� ������
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
                    incorrectSelectionOverlay.SetActive(false); // �������� ������������ �����

                    // ��������� ����� ��� �������� ���������
                    var animalCollider = currentAnimalInstance.GetComponent<BoxCollider>();
                    if (animalCollider != null && selectionOutline != null)
                    {
                        UpdateOutline(selectionOutline.rectTransform, animalCollider);
                        selectionOutline.gameObject.SetActive(true); // ���������� �����
                    }


                    // ���������� ������ � ����������� ������
                    SetButtonsActive();
                    ShuffleAndAssignAnswers();
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
        incorrectSelectionOverlay.SetActive(true); // ���������� ������������ �����
        yield return new WaitForSeconds(1f);       // ���� 1 �������
        incorrectSelectionOverlay.SetActive(false); // �������� ������������ �����
    }
    private void UpdateOutline(RectTransform outline, BoxCollider collider)
    {
        if (outline == null || collider == null) return;

        // ������ ���������� (X � Y ������������ ��� UI, Z ������������)
        Vector3 colliderSize = collider.size;

        // ����� ���������� � ������� �����������
        Vector3 worldCenter = collider.transform.TransformPoint(collider.center);

        // ������� ����� � ��������� ����������� Canvas
        Vector2 canvasLocalPosition = animalContainer.transform.InverseTransformPoint(worldCenter);

        // ������������� ������ � ������� �����
        outline.sizeDelta = new Vector2(colliderSize.x, colliderSize.y);
        outline.anchoredPosition = canvasLocalPosition;
    }

    // ����� ��� �������� ������ �������
    private void LoadNewQuestion()
    {
        // ���������, ���� �� ��� �������
        if (availableQuestions.Count == 0)
        {
            EndGame();
            return;
        }

        // ���������� ���� � ������������ ������
        isAnimalFound = false;
        selectionOutline.gameObject.SetActive(false);
        SetButtonsInactive(); // ������ ������ �����������
        incorrectSelectionOverlay.SetActive(false); // �������� ������������ �����

        // �������� ��������� ������ �� ���������
        int questionIndex = Random.Range(0, availableQuestions.Count);
        currentQuestion = availableQuestions[questionIndex];
        correctAnswer = currentQuestion.correctAnswer;
        availableQuestions.RemoveAt(questionIndex);

        // ������� ���������� ������ ���������, ���� �� ���
        if (currentAnimalInstance != null)
        {
            Destroy(currentAnimalInstance);
        }

        // ������� ����� ������ ��������� � ������ ����������
        Vector3 spawnPosition = containerCollider.bounds.center;
        currentAnimalInstance = Instantiate(currentQuestion.animalPrefab, spawnPosition, Quaternion.identity, animalContainer.transform);

        // ��������� ������ ������� ��� ���������
        FitPrefabToCollider(currentAnimalInstance, containerCollider);

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
        foreach (Button button in answerButtons)
        {
            var controller = button.GetComponent<QuizButtonController>();
            if (controller == null) continue;

            string buttonAnswer = button.GetComponentInChildren<TextMeshProUGUI>().text;
            if (buttonAnswer == correctAnswer)
            {
                controller.SetCorrect(); // ������������� ������ ����������� ������
            }
            else if (buttonAnswer == selectedAnswer)
            {
                controller.SetIncorrect(); // ������������� ������ ������������� ������
            }
            else
            {
                controller.SetInactive(); // ��������� ������ ������������ � ���������� ���������
            }
        }

        if (selectedAnswer == correctAnswer)
        {
            scoreManager.score++;
            Debug.Log("���������� �����!");
            // ��������� ������ ������ � ������� ����� ������
            if (correctAnswerParticles != null)
            {
                Vector3 topOfScreen = new Vector3(0, Camera.main.orthographicSize, 0);
                ParticleSystem particles = Instantiate(correctAnswerParticles, topOfScreen, Quaternion.identity);
                Destroy(particles.gameObject, 1.5f); // ���������� ������� ������ ����� 1 �������
            }
        }
        else
        {
            Debug.Log("������������ �����.");
        }

        // ��������� ����� ������ � ���������
        StartCoroutine(LoadNextQuestionWithDelay());
    }

    private IEnumerator LoadNextQuestionWithDelay()
    {
        yield return new WaitForSeconds(1.5f); // ���� 1.5 �������
        LoadNewQuestion();
    }

    // ����� ��� ������������� ������
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
        StopAllCoroutines();
        gameOverPanel.SetActive(true);
        scoreManager.finalScoreText.text = scoreManager.score.ToString();
    }
}
