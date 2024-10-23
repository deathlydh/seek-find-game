using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalQuizManager : MonoBehaviour
{
    public AnimalQuestionConfig[] animalQuestions; // ������ � ���������
    public Button[] answerButtons;                 // ������ � �������� ��� �������
    public Sprite animalSprite;                      // UI ������� ��� ����������� ����������

    private AnimalQuestionConfig currentQuestion;  // ������� ������
    private string correctAnswer;                  // ���������� �����

    private void Start()
    {
        LoadNewQuestion();
    }

    private void LoadNewQuestion()
    {
        // �������� ��������� ������ �� �������
        currentQuestion = animalQuestions[Random.Range(0, animalQuestions.Length)];
        correctAnswer = currentQuestion.correctAnswer;

        // ������������� ����������� ���������
       // animalImage.sprite = currentQuestion.animalPhoto;

        // �������� �������� ������� � ������������ ��
        List<string> shuffledAnswers = new List<string>(currentQuestion.answerOptions);
        ShuffleList(shuffledAnswers);

        // ������������ ������������ ������ �� �������
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;  // ��������� ����� ���������� ��� ���������
            answerButtons[i].GetComponentInChildren<Text>().text = shuffledAnswers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(shuffledAnswers[index]));
        }
    }

    // ������� ������������� ������ (�� ������ ��������� ������-�����)
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
            Debug.Log("���������!");
            // ������ ��� ����������� ������ (��������, ���������� �����)
        }
        else
        {
            Debug.Log("�����������.");
            // ������ ��� ������������� ������ (��������, ������� � ���������� �������)
        }

        // ��������� ��������� ������ ����� ������
        LoadNewQuestion();
    }
}
