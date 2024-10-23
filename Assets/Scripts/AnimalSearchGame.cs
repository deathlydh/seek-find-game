using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class AnimalSearch : MonoBehaviour
{
    public GameObject[] photographs;
    public GameObject[] animalButtons; // ������ � ���������� �������� (GameObject ��� SetActive)
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel; // ������, ������������ ���� ����� ��������� ����
    public TextMeshProUGUI finalScoreText; // ����� � ��������� ������
    public float timeLimit = 30f;

    public string[][] animalOptions; // ������ �������� � ���������� ��� ������ ����������
    public string[] correctAnswers; // ������ � ����������� �������� ��� ������ ����������
    private int currentPhotoIndex = 0; // ������ ������� ����������
    private int score = 0; // ����
    private float currentTime; // ������� ����� �������
    private bool isAnimalFound = false; // �������� ���������� ���������
    private List<int> shownPhotos = new List<int>(); // ������ ���������� ����������
    private Button correctButton; // ������ �� ���������� ������ ��� �������� ������

    void Start()
    {
        animalOptions = new string[][]
   {
        new string[] { "������", "�����", "�����" },  // �������� ��� ������ ����������
        new string[] { "����", "����", "����" },
        new string[] { "�����", "���", "����" },
        new string[] { "�������", "����", "����" },
        new string[] { "������", "�����", "����" }// �������� ��� ������ ����������
                                                    // �������� ��������� �������� ��� ������ ����������
   };

        correctAnswers = new string[] { "�����", "����", "�����", "����", "����" };
        currentTime = timeLimit;
        SetButtonsActive(false);
        StartCoroutine(Timer());
        AssignButtonListeners();
        gameOverPanel.SetActive(false); // �������� ������ � ������ ����
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
            Debug.Log("������������ ��������. ������ ������������.");
            MinusTimer();
        }
    }

    void ShuffleAndAssignAnswers()
    {
        // �������� ��������� �������� ������ ��� ������� ����������
        string[] currentOptions = animalOptions[currentPhotoIndex];

        // ������� ������ �� ��������� � ����������� ������
        List<string> shuffledOptions = currentOptions.ToList();
        shuffledOptions.Add(correctAnswers[currentPhotoIndex]);

        // ������������ ������
        shuffledOptions = shuffledOptions.OrderBy(a => Random.Range(0, shuffledOptions.Count)).ToList();

        // ����������� ������������ �������� �������
        for (int i = 0; i < animalButtons.Length; i++)
        {
            Button button = animalButtons[i].GetComponent<Button>();
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = shuffledOptions[i];

            // ���� ��� ���������� �����, ��������� ������ �� ���������� ������
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
            EndGame(); // ��������� ����, ���� ��� ���������� ��������
            return;
        }

        do
        {
            currentPhotoIndex = Random.Range(0, photographs.Length);
        }
        while (shownPhotos.Contains(currentPhotoIndex)); // ���� ������������ ����������

        shownPhotos.Add(currentPhotoIndex); // ��������� � � ������ ����������

        UpdatePhotograph();
    }

    void UpdatePhotograph()
    {
        // ��������� ������ ���������� � ������ ���������� �����
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
                Debug.Log("����� �����.");
                EndGame(); // ����������� ���� ��� ���������� 0
            }
        }
    }

    void EndGame()
    {
        StopAllCoroutines(); // ������������� ������
        gameOverPanel.SetActive(true); // ���������� ������ � �����������
        finalScoreText.text = score.ToString(); // ���������� ��������� ����
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
            Debug.Log("���������� �������� �������!");
            NextPhotograph();
        }
        else
        {
            Debug.Log("������������ ��������. ���������� �����.");
            MinusTimer();
        }
    }
}
