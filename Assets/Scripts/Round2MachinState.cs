using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.WSA;


public enum Round2State
{
    First, // верю/не верю
    Second // угадай зверя
}

public static class Round2MachinState
{
    [SerializeField]
    private static ImgPoolRound2 pool;
    static QuestionRound2 animal;

    public static Action ActivStage1;
    public static Action ActivStage2;

    public static Action<QuestionRound2> SetQuestion;
    public static Action<string[]> SetAnswers;

    static public Round2State State;

    public static void ActivateStage2()
    {
        State = Round2State.Second;
        Debug.Log(State);
        ActivStage2.Invoke();
    }

    public static void ActivateStage1()
    {
        State = Round2State.First;
        Debug.Log(State);
        ActivStage1.Invoke();
    }

    public static bool CheckFirst(string choose)
    {
        if (animal.isTrueAnswer)
        {
            return choose == animal.TrueAnswer;
        }
        return choose != animal.TrueAnswer;
    }

    public static void GetAnimal()
    {
        animal = pool.GetQuestion();
    }
}
