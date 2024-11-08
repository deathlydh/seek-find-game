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

   

    private AnimalQuestionConfig currentQuestion;  // ������� ������
    private string correctAnswer;
    private bool isAnimalFound = false;

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
        LoadNewQuestion();
        SetButtonsInactive(); // ������ ������ ����������� ��� ������
    }

    private void Update()
    {
        bool isTouchDetected = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;

        if ((Input.GetMouseButtonDown(0) || isTouchDetected) && !isAnimalFound)
        {
            CheckAnimal();
        }
    }

    // ����� ��� ��������, ���� �� ������� ��������
    void CheckAnimal()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

        // ��������, ��������� �� ������� � ������� �������� �� �����
        if (hitCollider != null && hitCollider == currentAnimalInstance.GetComponent<Collider2D>())
        {
            isAnimalFound = true;
            incorrectSelectionOverlay.SetActive(false); // �������� ������������ �����
            var animalOutline = currentAnimalInstance.GetComponent<AnimalOutline>();
            if (animalOutline != null)
            {
                animalOutline.SetOutlineActive(true);
            }
            SetButtonsActive(); // ������ ������ ��������� � ������ �� ����
            ShuffleAndAssignAnswers();
        }
        else
        {
            Debug.Log("������������ ��������. ������ ������������.");
            StartCoroutine(ShowIncorrectOverlay());
        }
    }

    private IEnumerator ShowIncorrectOverlay()
    {
        incorrectSelectionOverlay.SetActive(true); // ���������� ������������ �����
        yield return new WaitForSeconds(1f);       // ���� 1 �������
        incorrectSelectionOverlay.SetActive(false); // �������� ������������ �����
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

    // ����� ��� ��������� ������ ������
    private void OnAnswerSelected(string selectedAnswer)
    {
        if (selectedAnswer == correctAnswer)
        {
            scoreManager.score++;
            Debug.Log("���������� �����!");

            // ��������� ������ ������ � ������� ����� ������
            if (correctAnswerParticles != null)
            {
                Vector3 topOfScreen = new Vector3(0, Camera.main.orthographicSize, 0);
                ParticleSystem particles = Instantiate(correctAnswerParticles, topOfScreen, Quaternion.identity);
                Destroy(particles.gameObject, 1f); // ���������� ������� ������ ����� 1 �������
            }
        }
        else
        {
            Debug.Log("������������ �����.");
        }

        LoadNewQuestion();
    }

    // ����� ��� ��������� ����
    public void EndGame()
    {
        StopAllCoroutines();
        gameOverPanel.SetActive(true);
        scoreManager.finalScoreText.text = scoreManager.score.ToString();
    }
}
