using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct QuestionRound2
{
    public GameObject   imgPref;
    public bool         isTrueAnswer;
    public string       TrueAnswer;
    public string[]     Answers;
    public string       supposedAnimal;
}

public class ImgPoolRound2 : MonoBehaviour
{
    [Serializable]
    private struct PrefabRound2
    {
        public GameObject imgPref;
        public int difficulty;
        public string TrueAnswer;
    }

    [SerializeField]
    private List<PrefabRound2> Pool = new List<PrefabRound2>();
    [SerializeField]
    private List<string> Answers = new List<string>();

    private List<int> PreviosQuestion = new List<int>();

    [SerializeField]
    private int countForHard;
    private int questionsCount = 0;

    private void init()
    {
        PreviosQuestion = new List<int>();
    }

    private string[] GetNonTrueAnswer(string trueAnswer)
    {
        string[] answers = new string[3];
        int actualIndex = 0;

        while (actualIndex < 3)
        {
            int index = UnityEngine.Random.Range(0, Answers.Count);

            if ( !answers.Contains(Answers[index]) && 
                Answers[index] != trueAnswer)
            {
                answers[actualIndex] = Answers[index];
                actualIndex++;
            }
        }
        
        return answers;
    }

    private int GetNewIndexQuestion()
    {
        int index;
        do
        {
            index = UnityEngine.Random.Range(0, Pool.Count);
        } while ( !PreviosQuestion.Contains(index) &&
        (
        (Pool[index].difficulty > 0) && (questionsCount > countForHard)
        ||
        Pool[index].difficulty == 0
        ));

        PreviosQuestion.Add( index );

        return index;
    }

    public QuestionRound1 GetQuestion()
    {
        questionsCount++;
        PrefabRound2 prefabRound1 = Pool[GetNewIndexQuestion()];

        return new QuestionRound1
        {
            imgPref = prefabRound1.imgPref,
            TrueAnswer = prefabRound1.TrueAnswer,
            Answers = GetNonTrueAnswer(prefabRound1.TrueAnswer)
        };
    }
}
