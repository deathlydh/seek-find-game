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
        } while ( PreviosQuestion.Contains(index) &&
        (
        ! (Pool[index].difficulty > 0) && ! (questionsCount > countForHard)
        ||
        Pool[index].difficulty != 0
        ));

        PreviosQuestion.Add( index );

        return index;
    }

    public QuestionRound2 GetQuestion()
    {
        Debug.Log("______________________ImgPool________________________");
        questionsCount++;
        PrefabRound2 prefabRound2 = Pool[GetNewIndexQuestion()];
        bool fl = UnityEngine.Random.Range(0, 10) > 5;

        Debug.Log($"{prefabRound2.TrueAnswer}   {prefabRound2.TrueAnswer}");

        return new QuestionRound2
        {
            imgPref = prefabRound2.imgPref,
            TrueAnswer = prefabRound2.TrueAnswer,
            Answers = GetNonTrueAnswer(prefabRound2.TrueAnswer),
            isTrueAnswer = fl,
            supposedAnimal = fl ? prefabRound2.TrueAnswer : GetNonTrueAnswer(prefabRound2.TrueAnswer)[0]
        };
    }
}
