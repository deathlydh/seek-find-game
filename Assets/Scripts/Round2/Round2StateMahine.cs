using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateRound2{
    Stage1,
    Stage2
}

public struct PrefabWithAnswers{
    public GameObject imgPrefabs;
    public string TrueAnswer;
    public string[] OtherAnswers;
    public bool AnswerIsTrue;
    public string supAnswer;
}

[Serializable]
    public struct Trans{
        public Vector3 position;
        public Vector2 scale;
    }

public static class Round2StateMahine
{
    public static Action setStage1;
    public static Action setStage2;

    public static Action<int, int> OnWrongStage2;
    public static Action<bool> OnWrongStage;

    public static Action<int> OnGoodStage2;
    public static Action<bool> OnGoodStage;
    public static Action<string> OnSetAnimal;

    public static Action<Trans> SetLittleBorder;

    public static Action EndGame;
    public static Action StartGame;

    private static StateRound2 state = StateRound2.Stage1;
    private static bool isInputBlocked = false; // ����� ����

    public static StateRound2 State
    {
        private set
        {
            state = value;
        }
        get
        {
            return state;
        }
    }

    public static bool IsButtonsLocked { get; private set; } = false;

    public static void LockButtons()
    {
        IsButtonsLocked = true;
    }

    public static void UnlockButtons()
    {
        IsButtonsLocked = false;
    }

    public static bool IsInputBlocked
    {
        get => isInputBlocked;
        set => isInputBlocked = value; // ���������� �����������
    }

    public static Action<string[]> SetAnswers;
    public static Action<GameObject> SetImg;

    public static void init()
    {
    }

    public static void SetStage1()
    {
        State = StateRound2.Stage1;
        setStage1?.Invoke();
    }

    public static void SetStage2()
    {
        State = StateRound2.Stage2;
        setStage2?.Invoke();
    }
}
