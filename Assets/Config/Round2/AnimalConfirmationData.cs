using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimalConfirmationData", menuName = "Quiz/AnimalConfirmationData")]
public class AnimalConfirmationData : ScriptableObject
{
    [Header("ƒанные дл€ второго раунда")]
    [Tooltip("»зображение животного дл€ подтверждени€")]
    public Sprite animalImage;

    [Tooltip("Ќазвание животного дл€ подтверждени€ на фото (например, 'на фото лев')")]
    public string animalOnPhotoName;

    [Tooltip("ѕравильный ответ на вопрос 'на фото *название животного*'?")]
    public bool isCorrectAnimal;

    public string[] answerOptions;  // ¬арианты ответов (всего 4)
    public string correctAnswer;    // ѕравильный ответ
}

