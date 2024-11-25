using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.WSA;


public enum Round2State
{
    First, // верю/не верю
    Second // угадай зверя
}

public class Round2MachinState : MonoBehaviour
{
    [SerializeField]
    private ImgPoolRound2 pool;
    QuestionRound2 animal;

    public Round2State state;
    public Round2State State {
        get { return state; }
        private set 
        { 
            if (state == Round2State.First && value == Round2State.Second)
            {
                ActivateStage2();
                state = Round2State.Second;
            }
            else
            {
                ActivateStage1();
                state = Round2State.First;
            }
        }
    }

    public void ActivateStage2()
    {

    }

    public void ActivateStage1()
    {
        
    }

    public bool CheckFirst(string choose)
    {
        if (animal.isTrueAnswer)
        {
            return choose == animal.TrueAnswer;
        }
        return choose != animal.TrueAnswer;
    }

    public void GetAnimal()
    {
        animal = pool.GetQuestion();
    }
}
