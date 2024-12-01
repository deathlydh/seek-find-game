using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController1 : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Ссылка на VideoPlayer
    public GameObject uiPanel;      // Ссылка на интерфейс (остальные UI-элементы)
    public string videoFileName;    // Имя видеофайла в папке StreamingAssets

    private bool videoPlaying = true;

    void Start()
    {
        // Убедитесь, что интерфейс выключен
        if (uiPanel != null)
            uiPanel.SetActive(false);

        // Устанавливаем путь к видео
        SetVideoPath();

        // Запускаем видео
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    void Update()
    {
        // Проверяем нажатия мышкой, сенсорно или клавиатурой
        if (videoPlaying && (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            Debug.Log("Нажатие обнаружено. Скрываем видео.");
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

        Debug.Log("Видео должно было перезапуститься");

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

    void SetVideoPath()
    {
        if (videoPlayer == null || string.IsNullOrEmpty(videoFileName))
        {
            Debug.LogError("VideoPlayer или videoFileName не настроены!");
            return;
        }

        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);

#if UNITY_WEBGL && !UNITY_EDITOR
        // Для WebGL используем прямую загрузку через URL
        videoPlayer.url = videoPath;
#else
        // Для других платформ используем локальный путь
        videoPlayer.url = "file://" + videoPath;
#endif

        Debug.Log($"Видео загружено из пути: {videoPlayer.url}");
    }
}
