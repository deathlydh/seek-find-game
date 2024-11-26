using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController1 : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Ссылка на VideoPlayer
    public GameObject uiPanel; // Ссылка на интерфейс (остальные UI-элементы)

    private bool videoPlaying = true;

    void Start()
    {
        // Убедитесь, что интерфейс выключен
        if (uiPanel != null)
            uiPanel.SetActive(false);
    }

    void Update()
    {
        // Проверяем нажатия мышкой, сенсорно или клавиатурой
        if (videoPlaying && (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            HideVideo();
        }
    }

    void HideVideo()
    {
        // Скрываем видео
        if (videoPlayer != null)
        {
            videoPlayer.Stop(); // Останавливаем видео
            videoPlayer.gameObject.SetActive(false); // Скрываем объект с VideoPlayer
        }

        // Показываем интерфейс
        if (uiPanel != null)
            uiPanel.SetActive(true);

        videoPlaying = false; // Обновляем статус
    }
}
