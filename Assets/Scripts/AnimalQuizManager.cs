using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalQuizManager : MonoBehaviour
{
    public AnimalQuestionConfig[] animalQuestions; // Массив с вопросами
    public Button[] answerButtons;                 // Массив с кнопками для ответов
    public Sprite animalSprite;                      // UI элемент для отображения фотографии

    private AnimalQuestionConfig currentQuestion;  // Текущий вопрос
    private string correctAnswer;                  // Правильный ответ

    private void Start()
    {
        LoadNewQuestion();
    }

    private void LoadNewQuestion()
    {
        // Выбираем случайный вопрос из массива
        currentQuestion = animalQuestions[Random.Range(0, animalQuestions.Length)];
        correctAnswer = currentQuestion.correctAnswer;

        // Устанавливаем изображение животного
       // animalImage.sprite = currentQuestion.animalPhoto;

        // Получаем варианты ответов и перемешиваем их
        List<string> shuffledAnswers = new List<string>(currentQuestion.answerOptions);
        ShuffleList(shuffledAnswers);

        // Распределяем перемешанные ответы по кнопкам
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;  // Локальная копия переменной для замыкания
            answerButtons[i].GetComponentInChildren<Text>().text = shuffledAnswers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(shuffledAnswers[index]));
        }
    }

    // Функция перемешивания списка (на основе алгоритма Фишера-Йетса)
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
            Debug.Log("Правильно!");
            // Логика для правильного ответа (например, добавление очков)
        }
        else
        {
            Debug.Log("Неправильно.");
            // Логика для неправильного ответа (например, переход к следующему вопросу)
        }

        // Загружаем следующий вопрос после ответа
        LoadNewQuestion();
    }
}
