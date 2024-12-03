using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Round2ImgPool : MonoBehaviour
{
    [SerializeField]
    private AnimalQuestionConfig[] easyQuestions;

    [SerializeField]
    private AnimalQuestionConfig[] hardQuestions;

    private List<AnimalQuestionConfig> availableEasyQuestions;
    private List<AnimalQuestionConfig> availableHardQuestions;

    [SerializeField]
    private int countForHard;

    private int countPassFrame;

    private void Start()
    {
        availableEasyQuestions = new List<AnimalQuestionConfig>(easyQuestions);
        availableHardQuestions = new List<AnimalQuestionConfig>(hardQuestions);
    }

    public AnimalQuestionConfig GetNewQuestion()
    {
        var currentPool = countPassFrame >= countForHard ? availableHardQuestions : availableEasyQuestions;

        if (currentPool == null || currentPool.Count == 0)
        {
            Debug.LogWarning("Нет доступных вопросов!");
            Round2StateMahine.EndGame?.Invoke();
            return null;
        }

        var unusedQuestions = currentPool.Where(q => !UsedQuestionsManager.Instance.IsUsed(q)).ToList();

        if (unusedQuestions.Count == 0)
        {
            Debug.LogWarning("Все вопросы в текущем пуле уже использованы!");
            Round2StateMahine.EndGame?.Invoke();
            return null;
        }

        var selectedQuestion = unusedQuestions[UnityEngine.Random.Range(0, unusedQuestions.Count)];
        UsedQuestionsManager.Instance.MarkAsUsed(selectedQuestion);
        currentPool.Remove(selectedQuestion);
        countPassFrame++;

        return selectedQuestion;
    }

    public string[] GetFalseAnswers(string correctAnswer)
    {
        var allAnswers = easyQuestions.SelectMany(q => q.answerOptions)
            .Concat(hardQuestions.SelectMany(q => q.answerOptions))
            .Where(a => a != correctAnswer)
            .Distinct()
            .OrderBy(_ => UnityEngine.Random.value)
            .Take(3)
            .ToArray();

        return allAnswers;
    }

    public PrefabWithAnswers? GetNewFrameInfo()
    {
        if (Round2StateMahine.IsInputBlocked)
        {
            Debug.LogWarning("Ввод заблокирован!");
            return null;
        }

        Round2StateMahine.IsInputBlocked = true; // Блокируем ввод

        var question = GetNewQuestion();
        if (question == null)
        {
            Round2StateMahine.IsInputBlocked = false; // Разблокируем ввод при ошибке
            return null;
        }

        var falseAnswers = GetFalseAnswers(question.correctAnswer);

        bool _answerIsTrue = UnityEngine.Random.value > 0.2f;
        var frameInfo = new PrefabWithAnswers
        {
            imgPrefabs = question.animalPrefab,
            TrueAnswer = question.correctAnswer,
            OtherAnswers = falseAnswers,
            AnswerIsTrue = _answerIsTrue,
            supAnswer = _answerIsTrue ? question.correctAnswer : falseAnswers[0]
        };

        Round2StateMahine.IsInputBlocked = false; // Разблокируем ввод после обработки
        return frameInfo;
    }
}
