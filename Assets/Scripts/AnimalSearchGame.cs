using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalSearch : MonoBehaviour
{
    public GameObject[] photographs;
    public GameObject[] animalButtons; // ������ � ���������� �������� (GameObject ��� SetActive)
    public TextMeshProUGUI timerText; 
    public TextMeshProUGUI scoreText; 
    public float timeLimit = 30f; 
    public Button correctAnimalName; // ��� ����������� ��������� ��� ������� ����������


    private int currentPhotoIndex = 0; // ������ ������� ����������
    private int score = 0; // ����
    private float currentTime; // ������� ����� �������
    private bool isAnimalFound = false; // �������� ���������� ���������

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

        // ���������, ����� �� ���� � ������� ���� �������� �������
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
            Debug.Log("������������ ��������. ������ ������������.");
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

        // ��������� ������� ����������
        photographs[currentPhotoIndex].SetActive(false);

        // ��������� � ��������� ����������
        currentPhotoIndex = (currentPhotoIndex + 1) % photographs.Length;

        // �������� ����� ����������
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

    // ����� �������
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

            // ��������� ����� ������� � ������� 0:30
            timerText.text = $"{minutes}:{seconds:D2}";

            if (currentTime <= 0)
            {
                Debug.Log("����� �����.");
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
            score++; // ��������� ���� �� ���������� �����
            UpdateScore();
            Debug.Log("���������� �������� �������!");
            NextPhotograph(); // ������������� �� ��������� ����������
        }
        else
        {
            Debug.Log("������������ ��������. ���������� �����.");
            MinusTimer(); // ��������� ������ �� ������������ �����
        }
    }
}
