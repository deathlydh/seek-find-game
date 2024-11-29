using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        int lastSavedScore = SaveSystem.GetSave(SaveSystem.GetCount() - 1); // �������� ��������� ���������� ����
        Debug.Log("�������� ��������� ����: " + lastSavedScore); // ������� � �������
        finalScoreText.text = lastSavedScore.ToString(); // ���������� �� UI
    }
}
