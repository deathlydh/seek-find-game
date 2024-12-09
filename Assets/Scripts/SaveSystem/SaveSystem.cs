using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public static class SaveSystem
{
    public  static int  count = 0;

    private static string _count        = "CountSave";
    private static string _firstStage   = "FirstStage";
    private static string _save         = "ScoreSave_";
    private static string PassPool      = "PassPool";

    public static void init()
    {
        /*
        if (PlayerPrefs.GetInt(_count) != 0)
        {
            count = PlayerPrefs.GetInt(_count);
        }*/
        count = 0;
        PlayerPrefs.SetString(PassPool, "");
    }

    public static int GetFirstStage()
    {
        return PlayerPrefs.GetInt(_firstStage);
    }

    public static void SaveFirstStage(int score)
    {
        PlayerPrefs.SetInt(_firstStage, score);
    }

    public static void Save(int score)
    {
        count++;
        PlayerPrefs.SetInt(_count, count);
        PlayerPrefs.SetInt(_save + (count - 1).ToString(), score);
        Debug.Log("Сохранён счёт: " + score);  // Выводим сохранённый счёт в консоль
    }

    public static int GetCount()
    {
        return PlayerPrefs.GetInt(_count);
    }

    public static int GetSave(int index)
    {
        return PlayerPrefs.GetInt(_save + index);
    }

    public static int[] GetAllScore()
    {
        int[] allScore = new int[count];
        count = GetCount();

        for(int i = 0; i < count; i++)
        {
            allScore[i] = GetSave(i);
        }
        return allScore;
    }

    public static int AnaliticSoreBatterThen(int score)
    {
        int[] allScore = GetAllScore();
        int LessCount = 0;

        foreach(int _score in allScore)
        {
            if (_score < score) LessCount++;
        }

        return Mathf.RoundToInt((float)LessCount / count * 100);
    }

    public static void setPassPool(string name)
    {
        string _passPool = PlayerPrefs.GetString(PassPool);
        _passPool += (_passPool.Length > 1 ? "|" : "") + name.Replace("(Clone)","");
        Debug.Log(_passPool);

        PlayerPrefs.SetString(PassPool, _passPool);
    }

    public static bool isContainOnPassPool(string PrefabsName)
    {
        string[] _pool = getPassPool();
        return _pool.Contains(PrefabsName);
    }

    public static string[] getPassPool()
    {
        return PlayerPrefs.GetString(PassPool).Split('|');
    }
}
