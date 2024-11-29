using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Round2ImgPool : MonoBehaviour
{
    [Serializable]
    public struct Animal{
        public GameObject imgPrefabs;
        public string TrueAnswer;
        public bool difficulty;
    }

    [SerializeField]
    private List<Animal> Pool = new List<Animal>();
    private List<Animal> _pool = new List<Animal>();
    [SerializeField]
    private List<string> Answers = new List<string>();
    private List<string> _answers = new List<string>();

    [SerializeField]
    private int CountForHard;

    private int CountPassFrame = 0;

    void Start()
    {
        _pool = Pool;
        _answers = Answers;
    }

    public Animal GetNewFrame()
    {
        Animal anim;
        if( _pool.Count == 0 )
            _pool = new List<Animal>(Pool.ToArray());
        do
        {   
            int index = UnityEngine.Random.Range(0,  _pool.Count-1);
            anim = _pool[index];
        }
        while(!(!anim.difficulty || ( anim.difficulty && CountPassFrame > CountForHard)));

        _pool.Remove(anim);
        return anim;
    }

    public string[] GetUnTrueAnswers(string trueAnswer){
        _answers = Answers;

        string[] answers = new string[3];
        int index = 0;

        for (int i = 0; i < 3; i++)
        {
            do
            {
                index = UnityEngine.Random.Range(0, _answers.Count-1);
                Debug.Log($"{_answers.Count} {index} {trueAnswer == _answers[index]}");
            }
            while(trueAnswer == _answers[index] && answers.Contains<string>(_answers[index]));

            answers[i] = _answers[index];
        }

        _answers = Answers;
        return answers;
    }

    public PrefabWithAnswers GetNewFrameInfo(){
        Animal Frame  = GetNewFrame();
        string[] notTrueAnswers = GetUnTrueAnswers( Frame.TrueAnswer);
        bool IsTrue = UnityEngine.Random.Range(0, 15) > 5;

        Debug.Log(Frame.TrueAnswer);

        CountPassFrame++;
        return new PrefabWithAnswers()
        {
            imgPrefabs = Frame.imgPrefabs,
            TrueAnswer = Frame.TrueAnswer,
            OtherAnswers = notTrueAnswers,
            AnswerIsTrue = IsTrue,
            supAnswer = IsTrue ? Frame.TrueAnswer : notTrueAnswers[0]
        };
    }
}
