using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    [SerializeField]
    public TextMeshProUGUI _AI;

    void Start()
    {
        int lastSavedScore = SaveSystem.GetFirstStage(); // �������� ��������� ����������� ����
        Debug.Log("�������� ��������� ����: " + lastSavedScore); // ������� � �������
        finalScoreText.text = lastSavedScore.ToString(); // ���������� �� UI
        _AI.SetText(SaveSystem.GetSave(SaveSystem.GetCount() - 1).ToString());
    }

   
}
