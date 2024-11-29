using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] panels; // Все панели на канвасе
    [SerializeField] private GameObject settingsPanel; // Панель настроек
    private GameObject lastActivePanel; // Хранит ссылку на последнюю активную панель
    private bool isSettingsActive = false; // Флаг активности панели настроек

    // Метод для отображения панели настроек
    public void ShowSettingsPanel()
    {
        if (isSettingsActive) return;

        // Сохраняем текущую активную панель
        lastActivePanel = GetActivePanel();

        // Скрыть все панели
        HideAllPanels();

        // Показать панель настроек
        settingsPanel.SetActive(true);
        isSettingsActive = true;

        // Остановить игровые процессы (например, таймеры)
        Time.timeScale = 0;
    }

    // Метод для закрытия панели настроек
    public void CloseSettingsPanel()
    {
        if (!isSettingsActive) return;

        // Скрыть панель настроек
        settingsPanel.SetActive(false);
        isSettingsActive = false;

        // Восстановить последнюю активную панель
        if (lastActivePanel != null)
        {
            lastActivePanel.SetActive(true);
        }

        // Возобновить игровые процессы
        Time.timeScale = 1;
    }

    // Метод для скрытия всех панелей
    private void HideAllPanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    // Метод для получения текущей активной панели
    private GameObject GetActivePanel()
    {
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                return panel;
            }
        }
        return null; // Если активных панелей нет
    }

    // Метод для загрузки новой сцены
    public void LoadScene(string sceneName)
    {
        // Сбрасываем Time.timeScale перед загрузкой
        Time.timeScale = 1;

        // Загружаем новую сцену
        SceneManager.LoadScene(sceneName);
    }
}
