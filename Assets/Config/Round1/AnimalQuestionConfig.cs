using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimalQuestion", menuName = "AnimalQuiz/AnimalQuestion")]
public class AnimalQuestionConfig : ScriptableObject
{
    public GameObject animalPrefab;      // ���� ���������
    public string[] answerOptions;  // �������� ������� (����� 4)
    public string correctAnswer;    // ���������� �����
}
