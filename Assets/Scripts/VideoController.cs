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

        // Запускаем таймер на повторное включение видео
        StartCoroutine(ShowVideoAfterDelay(60f));
    }

    IEnumerator ShowVideoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Включаем видео
        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(true); // Включаем объект с VideoPlayer
            videoPlayer.Play(); // Запускаем видео
        }

        // Скрываем интерфейс
        if (uiPanel != null)
            uiPanel.SetActive(false);

        videoPlaying = true; // Обновляем статус
    }
}
