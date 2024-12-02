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
        List<AnimalQuestionConfig> currentPool;

        // Выбираем пул вопросов в зависимости от сложности
        if (countPassFrame >= countForHard)
        {
            currentPool = availableHardQuestions;
        }
        else
        {
            currentPool = availableEasyQuestions;
        }

        // Проверяем, есть ли доступные вопросы
        if (currentPool.Count == 0)
        {
            Debug.LogWarning("Нет доступных вопросов!");
            Round2StateMahine.EndGame.Invoke();
            return null;
        }

        // Пытаемся найти новый вопрос, который ещё не был использован
        AnimalQuestionConfig selectedQuestion = null;
        for (int attempts = 0; attempts < currentPool.Count; attempts++)
        {
            int index = UnityEngine.Random.Range(0, currentPool.Count);
            selectedQuestion = currentPool[index];

            if (!UsedQuestionsManager.Instance.IsUsed(selectedQuestion))
            {
                // Отмечаем вопрос как использованный и удаляем его из пула
                UsedQuestionsManager.Instance.MarkAsUsed(selectedQuestion);
                currentPool.RemoveAt(index);
                countPassFrame++;
                return selectedQuestion;
            }
        }

        // Если все вопросы в текущем пуле уже использованы
        Debug.LogWarning("Все вопросы в текущем пуле уже использованы!");
        Round2StateMahine.EndGame.Invoke();
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
    public PrefabWithAnswers GetNewFrameInfo()
    {
        // Проверка на конец игры
        if (availableEasyQuestions.Count + availableHardQuestions.Count == 2)
        {
            SaveSystem.Save(Round2Controller.CountAnswers);
            Round2StateMahine.EndGame.Invoke();
        }

        // Получаем новый вопрос
        AnimalQuestionConfig question = GetNewQuestion();
        if (question == null)
        {
            // Возвращаем пустой объект, если вопросов нет
            return new PrefabWithAnswers()
            {
                imgPrefabs = null,
                TrueAnswer = string.Empty,
                OtherAnswers = new string[0],
                AnswerIsTrue = false,
                supAnswer = string.Empty
            };
        }

        // Получаем неверные ответы
        string[] falseAnswers = GetFalseAnswers(question.correctAnswer);

        // Решаем, будет ли правильный ответ отображаться сразу
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
