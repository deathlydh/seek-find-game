using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] panels; // ��� ������ �� �������
    [SerializeField] private GameObject settingsPanel; // ������ ��������
    private GameObject lastActivePanel; // ������ ������ �� ��������� �������� ������
    private bool isSettingsActive = false; // ���� ���������� ������ ��������

    // ����� ��� ����������� ������ ��������
    public void ShowSettingsPanel()
    {
        if (isSettingsActive) return;

        // ��������� ������� �������� ������
        lastActivePanel = GetActivePanel();

        // ������ ��� ������
        HideAllPanels();

        // �������� ������ ��������
        settingsPanel.SetActive(true);
        isSettingsActive = true;

        // ���������� ������� �������� (��������, �������)
        Time.timeScale = 0;
    }

    // ����� ��� �������� ������ ��������
    public void CloseSettingsPanel()
    {
        if (!isSettingsActive) return;

        // ������ ������ ��������
        settingsPanel.SetActive(false);
        isSettingsActive = false;

        // ������������ ��������� �������� ������
        if (lastActivePanel != null)
        {
            lastActivePanel.SetActive(true);
        }

        // ����������� ������� ��������
        Time.timeScale = 1;
    }

    // ����� ��� ������� ���� �������
    private void HideAllPanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    // ����� ��� ��������� ������� �������� ������
    private GameObject GetActivePanel()
    {
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                return panel;
            }
        }
        return null; // ���� �������� ������� ���
    }

    // ����� ��� �������� ����� �����
    public void LoadScene(string sceneName)
    {
        // ���������� Time.timeScale ����� ���������
        Time.timeScale = 1;

        // ��������� ����� �����
        SceneManager.LoadScene(sceneName);
    }
}
