using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        int lastSavedScore = SaveSystem.GetSave(SaveSystem.GetCount() - 1); // Получаем последний сохранённый счёт
        Debug.Log("Загружен последний счёт: " + lastSavedScore); // Выводим в консоль
        finalScoreText.text = lastSavedScore.ToString(); // Отобразить на UI
    }
}
