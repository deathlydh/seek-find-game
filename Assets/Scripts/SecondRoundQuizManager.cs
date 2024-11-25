using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SecondRoundQuizManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public AnimalQuestionConfig[] easyQuestions; // ˸���� �������
    public AnimalQuestionConfig[] hardQuestions; // ������� �������
    private List<AnimalQuestionConfig> availableEasyQuestions; // ��������� ����� �������
    private List<AnimalQuestionConfig> availableHardQuestions; // ��������� ������� �������
    private bool isEasyPhase = true;

    public Button yesButton;     // ������ "��"
    public Button noButton;      // ������ "���"
    public Button[] answerButtons; // ������ ������ ��� ������ ���������
    public TMP_Text questionText;  // ����� ��� ������� "�������� ����"
    public RectTransform animalContainer;
    public GameObject gameOverPanel;

    [SerializeField] private float fixOffsetX = 0f; // �������� �� X ��� �����
    [SerializeField] private float fixOffsetY = 0f; // �������� �� Y ��� �����

    private AnimalQuestionConfig currentQuestion;
    private GameObject currentAnimalInstance;
    private bool isAnswerPhase = false; // ���� ��� ����� ������ �� ������ ������
    private string correctAnswer;

    private BoxCollider2D containerCollider;

    private bool isQuizStarted = false;

    private void Start()
    {
        availableEasyQuestions = new List<AnimalQuestionConfig>(easyQuestions);
        availableHardQuestions = new List<AnimalQuestionConfig>(hardQuestions);
        containerCollider = animalContainer.GetComponent<BoxCollider2D>();
        gameOverPanel.SetActive(false);
        SetButtonsInactive();
        LoadNewQuestion();
        InitializeButtons();
    }

    public void StartQuiz()
    {
        isQuizStarted = true; // ������������� ����, ��� ��������� ��������
        LoadNewQuestion();
        SetButtonsInactive(); // ������ ������ ����������� ��� ������
    }

    private void LoadNewQuestion()
    {
        List<AnimalQuestionConfig> currentAvailableQuestions = isEasyPhase ? availableEasyQuestions : availableHardQuestions;

        if (currentAvailableQuestions.Count == 0)
        {
            if (isEasyPhase)
            {
                isEasyPhase = false;
                LoadNewQuestion();
                return;
            }
            else
            {
                EndGame();
                return;
            }
        }

        int questionIndex = Random.Range(0, currentAvailableQuestions.Count);
        currentQuestion = currentAvailableQuestions[questionIndex];
        correctAnswer = currentQuestion.correctAnswer;
        currentAvailableQuestions.RemoveAt(questionIndex);

        if (currentAnimalInstance != null)
        {
            Destroy(currentAnimalInstance);
        }

        Vector3 spawnPosition = containerCollider.bounds.center;
        currentAnimalInstance = Instantiate(currentQuestion.animalPrefab, spawnPosition, Quaternion.identity, animalContainer.transform);

        if (Random.value < 0.5f)
        {
            questionText.text = $"� ��������� �������: {correctAnswer}";
        }
        else
        {
            List<string> options = new List<string>(currentQuestion.answerOptions);
            options.Remove(correctAnswer);
            string randomOption = options[Random.Range(0, options.Count)];
            questionText.text = $"� ��������� �������: {randomOption}";
        }

        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
        SetButtonsInactive();
        isAnswerPhase = false;

        FitPrefabToCollider(currentAnimalInstance, containerCollider);
    }

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

    public void OnYesButtonClicked()
    {
        if (isAnswerPhase) return;

        if (correctAnswer == currentQuestion.correctAnswer)
        {
            scoreManager.score++;
            Debug.Log("���������� �����! ���� ��������.");
            StartCoroutine(LoadNextQuestionWithDelay());
        }
        else
        {
            Debug.Log("������������ �����.");
            StartCoroutine(LoadNextQuestionWithDelay());
        }
    }

    public void OnNoButtonClicked()
    {
        if (isAnswerPhase) return;

        Debug.Log("����� ������ '���'. ������� � ������ �� 4 ���������.");
        isAnswerPhase = true;
        SetAnswerButtonsActive();

        List<string> answerOptions = new List<string>(currentQuestion.answerOptions);
        ShuffleList(answerOptions);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].gameObject.SetActive(true);
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = answerOptions[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnimalSelected(answerOptions[index]));
        }

        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }

    private void InitializeButtons()
    {
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(OnYesButtonClicked);

        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(OnNoButtonClicked);
    }

    private void OnAnimalSelected(string selectedAnswer)
    {
        foreach (Button btn in answerButtons)
        {
            btn.GetComponent<QuizButtonController>().SetInactive();
        }

        var selectedButton = System.Array.Find(answerButtons, b => b.GetComponentInChildren<TMP_Text>().text == selectedAnswer);
        var buttonController = selectedButton.GetComponent<QuizButtonController>();

        if (selectedAnswer == correctAnswer)
        {
            scoreManager.score++;
            buttonController.SetCorrect();
            Debug.Log("���������� �����! ���� ��������.");
        }
        else
        {
            buttonController.SetIncorrect();
            Debug.Log("������������ �����.");
        }

        StartCoroutine(LoadNextQuestionWithDelay());
    }

    private IEnumerator LoadNextQuestionWithDelay()
    {
        yield return new WaitForSeconds(1.5f);
        LoadNewQuestion();
    }

    private void EndGame()
    {
        StopAllCoroutines();
        Debug.Log("���� ��������.");
        gameOverPanel.SetActive(true);
    }

    private void SetButtonsInactive()
    {
        foreach (Button button in answerButtons)
        {
            button.gameObject.SetActive(false); // ��������� �������� ������
        }
    }

    private void SetAnswerButtonsActive()
    {
        foreach (Button button in answerButtons)
        {
            button.gameObject.SetActive(true); // ������ ������ ������� � ��������
        }

        yesButton.gameObject.SetActive(false); // �������� ������ "��" � "���"
        noButton.gameObject.SetActive(false);
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
}
