using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct QuestionRound1
{
    public GameObject imgPref;
    public string TrueAnswer;
    public string[] Answers;
}

public class ImgPoolRound1 : MonoBehaviour
{
    [Serializable]
    private struct PrefabRound1
    {
        public GameObject imgPref;
        public int difficulty;
        public string TrueAnswer;
    }

    [SerializeField]
    private List<PrefabRound1> Pool = new List<PrefabRound1>();
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

            if ( !answers.Contains(Answers[index]) && Answers[index] != trueAnswer)
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
        } while ( !PreviosQuestion.Contains(index) );
        PreviosQuestion.Add( index );

        return index;
    }

    public QuestionRound1 GetQuestion()
    {

        PrefabRound1 prefabRound1 = Pool[GetNewIndexQuestion()];

        return new QuestionRound1
        {
            imgPref = prefabRound1.imgPref,
            TrueAnswer = prefabRound1.TrueAnswer,
            Answers = GetNonTrueAnswer(prefabRound1.TrueAnswer)
        };
    }
}
