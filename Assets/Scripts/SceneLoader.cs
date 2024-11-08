using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName; // ��� ����� ��� ��������
    [SerializeField] private Button loadButton; // ������ ��� ������� �����

    private void Start()
    {
        // ������������� �� ������� ������� ������
        if (loadButton != null)
        {
            loadButton.onClick.AddListener(LoadScene);
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneName); // �������� ��������� �����
    }
}
