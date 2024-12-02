using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedQuestionsManager : MonoBehaviour
{
    public static UsedQuestionsManager Instance { get; private set; }

    private HashSet<AnimalQuestionConfig> usedQuestions = new HashSet<AnimalQuestionConfig>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // ��������� ����� �������
    }

    public bool IsUsed(AnimalQuestionConfig question)
    {
        return usedQuestions.Contains(question);
    }

    public void MarkAsUsed(AnimalQuestionConfig question)
    {
        usedQuestions.Add(question);
    }

    public void ResetUsedQuestions()
    {
        Debug.Log("����� ��������...");
        usedQuestions.Clear();
        Debug.Log($"����� �������� ����� ������: {usedQuestions.Count}");
    }
}
