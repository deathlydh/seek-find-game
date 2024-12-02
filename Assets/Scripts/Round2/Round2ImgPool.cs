using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Round2ImgPool : MonoBehaviour
{
    [SerializeField]
    private AnimalQuestionConfig[] easyQuestions; // Лёгкие вопросы
    [SerializeField]
    private AnimalQuestionConfig[] hardQuestions; // Сложные вопросы

    private List<AnimalQuestionConfig> availableEasyQuestions;
    private List<AnimalQuestionConfig> availableHardQuestions;

    [SerializeField]
    private List<Transform> borders = new List<Transform>(); // Границы для спавна животных

    [SerializeField]
    private int countForHard; // Количество проходов до сложных вопросов
    private int countPassFrame = 0;

    void Start()
    {
        // Создаём копии массивов вопросов для работы
        availableEasyQuestions = new List<AnimalQuestionConfig>(easyQuestions);
        availableHardQuestions = new List<AnimalQuestionConfig>(hardQuestions);
    }

    /// <summary>
    /// Получает новый вопрос с учётом сложности и проверяет, использовался ли он.
    /// </summary>
    public AnimalQuestionConfig GetNewQuestion()
    {
        List<AnimalQuestionConfig> currentPool = countPassFrame >= countForHard ? availableHardQuestions : availableEasyQuestions;

        if (currentPool == null || currentPool.Count == 0)
        {
            Debug.LogWarning("Нет доступных вопросов!");
            Round2StateMahine.EndGame?.Invoke(); // Добавлена проверка на null
            return null;
        }

        for (int attempts = 0; attempts < currentPool.Count; attempts++)
        {
            int index = UnityEngine.Random.Range(0, currentPool.Count);
            AnimalQuestionConfig selectedQuestion = currentPool[index];

            if (!UsedQuestionsManager.Instance.IsUsed(selectedQuestion))
            {
                UsedQuestionsManager.Instance.MarkAsUsed(selectedQuestion);
                currentPool.RemoveAt(index);
                countPassFrame++;
                return selectedQuestion;
            }
        }

        Debug.LogWarning("Все вопросы в текущем пуле уже использованы!");
        Round2StateMahine.EndGame?.Invoke(); // Добавлена проверка на null
        return null;
    }

    /// <summary>
    /// Получает три неверных ответа.
    /// </summary>
    public string[] GetFalseAnswers(string correctAnswer)
    {
        List<string> allAnswers = new List<string>();

        // Собираем все возможные ответы из всех вопросов
        allAnswers.AddRange(easyQuestions.SelectMany(q => q.answerOptions));
        allAnswers.AddRange(hardQuestions.SelectMany(q => q.answerOptions));

        // Убираем правильный ответ
        allAnswers.RemoveAll(a => a == correctAnswer);

        // Выбираем три случайных ответа
        return allAnswers
            .OrderBy(_ => UnityEngine.Random.value)
            .Take(3)
            .ToArray();
    }

    /// <summary>
    /// Получает данные для следующего кадра.
    /// </summary>
    public PrefabWithAnswers? GetNewFrameInfo()
    {
        if (availableEasyQuestions.Count + availableHardQuestions.Count == 2)
        {
            SaveSystem.Save(Round2Controller.CountAnswers);
            Round2StateMahine.EndGame.Invoke();
        }

        AnimalQuestionConfig question = GetNewQuestion();
        if (question == null)
        {
            return null;
        }

        string[] falseAnswers = GetFalseAnswers(question.correctAnswer);
        bool isTrueAnswerShown = UnityEngine.Random.Range(0, 15) > 5;

        return new PrefabWithAnswers
        {
            imgPrefabs = question.animalPrefab,
            TrueAnswer = question.correctAnswer,
            OtherAnswers = falseAnswers,
            AnswerIsTrue = isTrueAnswerShown,
            supAnswer = isTrueAnswerShown ? question.correctAnswer : falseAnswers[0]
        };
    }
}
