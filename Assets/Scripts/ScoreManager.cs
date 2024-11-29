using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public int score = 0;
    public void SaveScore()
    {
        SaveSystem.Save(score); // Сохранить очки в системе сейвов
    }
}
