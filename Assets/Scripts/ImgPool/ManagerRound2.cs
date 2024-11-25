using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerRound2 : MonoBehaviour
{
    private QuestionRound2 actualQuestion;
    [SerializeField]
    private ImgPoolRound2 imgPoolRound2;

    private int TrueAnswerIndex;

    public void setImg(GameObject obj)
    {
        //воткни отображение вопроса, я не разобрался
    }

    public void WrongAnswer()
    {
        //воткни отображение косяка
    }

    public void setAnswers()
    {
        string[] answers = new string[4];
        int index = UnityEngine.Random.Range(0, answers.Length);

        for(int i = 0; i < 4; i++)
        {
            answers[i] = i != index ? actualQuestion.Answers[i < index ? i : i-1] : actualQuestion.TrueAnswer;
        }
        TrueAnswerIndex = index;
        Round2MachinState.SetAnswers.Invoke(answers);
    }

    public void SetNextQuestion()
    {
        Debug.Log("______________________Manager________________________");
        actualQuestion = imgPoolRound2.GetQuestion();
        //setImg(actualQuestion.imgPref);

        setAnswers();

        Round2MachinState.SetQuestion.Invoke(actualQuestion);
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
