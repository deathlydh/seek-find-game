using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimalQuestion", menuName = "AnimalQuiz/AnimalQuestion")]
public class AnimalQuestionConfig : ScriptableObject
{
    public GameObject animalPrefab;      // Фото животного
    public string[] answerOptions;  // Варианты ответов (всего 4)
    public string correctAnswer;    // Правильный ответ
}
