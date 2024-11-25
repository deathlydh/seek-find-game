using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerRound2 : MonoBehaviour
{
    private QuestionRound2 actualQuestion;
    [SerializeField]
    private ImgPoolRound2 imgPoolRound2;

    [SerializeField]
    private PrefDrower prefDrower;

    private int TrueAnswerIndex;

    public void setImg(GameObject obj)
    {
        prefDrower.SetImg(obj);
        //воткни отображение вопроса, я не разобрался
    }

    public void WrongAnswer()
    {
        //воткни отображение косяка
    }

    public void setAnswers()
    {
        string[] answers = new string[4];
        int index = UnityEngine.Random.Range(0, 4);

        for(int i = 0; i < 4; i++)
        {
            if(i != index)
            {
                answers[i] = actualQuestion.Answers[i < index ? i : i - 1];
            }
            else
            {
                answers[i] = actualQuestion.TrueAnswer;
            }
        }
        TrueAnswerIndex = index;
        Round2MachinState.SetAnswers.Invoke(answers);
    }

    public void SetNextQuestion()
    {
        Debug.Log("______________________Manager________________________");
        actualQuestion = imgPoolRound2.GetQuestion();
        setImg(actualQuestion.imgPref);

        setAnswers();

        //Round2MachinState.SetQuestion.Invoke(actualQuestion);
        Round2MachinState.ActivateStage1();
    }

    public void OnClickBtnStage1(bool _isTrue)
    {
        Debug.Log($"U Answer is {actualQuestion.isTrueAnswer == _isTrue}");
        if (actualQuestion.isTrueAnswer == _isTrue)
        {
            if (actualQuestion.isTrueAnswer)
            {
                SetNextQuestion();
            }
            else
            {
                Round2MachinState.ActivateStage2();
            }
        }
        else
        {
            WrongAnswer();
        }
    }

    public void OnClickBtnStage2(int Btn)
    {
        Debug.Log($"U Answer is {TrueAnswerIndex == Btn}");
        if (TrueAnswerIndex == Btn)
        {
            SetNextQuestion();
        }
        else
        {
            WrongAnswer();
        }
    }

    void Start()
    {
        SetNextQuestion();
    }
}
