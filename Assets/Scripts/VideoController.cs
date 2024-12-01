using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController1 : MonoBehaviour
{
    public VideoPlayer videoPlayer; // ������ �� VideoPlayer
    public GameObject uiPanel;      // ������ �� ��������� (��������� UI-��������)
    public string videoFileName;    // ��� ���������� � ����� StreamingAssets

    private bool videoPlaying = true;

    void Start()
    {
        // ���������, ��� ��������� ��������
        if (uiPanel != null)
            uiPanel.SetActive(false);

        // ������������� ���� � �����
        SetVideoPath();

        // ��������� �����
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    void Update()
    {
        // ��������� ������� ������, �������� ��� �����������
        if (videoPlaying && (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            Debug.Log("������� ����������. �������� �����.");
            HideVideo();
        }
    }

    void HideVideo()
    {
        // �������� �����
        if (videoPlayer != null)
        {
            videoPlayer.Stop(); // ������������� �����
            videoPlayer.gameObject.SetActive(false); // �������� ������ � VideoPlayer
        }

        // ���������� ���������
        if (uiPanel != null)
            uiPanel.SetActive(true);

        videoPlaying = false; // ��������� ������

        // ��������� ������ �� ��������� ��������� �����
        StartCoroutine(ShowVideoAfterDelay(60f));
    }

    IEnumerator ShowVideoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("����� ������ ���� ���������������");

        // �������� �����
        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(true); // �������� ������ � VideoPlayer
            videoPlayer.Play(); // ��������� �����
        }

        // �������� ���������
        if (uiPanel != null)
            uiPanel.SetActive(false);

        videoPlaying = true; // ��������� ������
    }

    void SetVideoPath()
    {
        if (videoPlayer == null || string.IsNullOrEmpty(videoFileName))
        {
            Debug.LogError("VideoPlayer ��� videoFileName �� ���������!");
            return;
        }

        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);

#if UNITY_WEBGL && !UNITY_EDITOR
        // ��� WebGL ���������� ������ �������� ����� URL
        videoPlayer.url = videoPath;
#else
        // ��� ������ �������� ���������� ��������� ����
        videoPlayer.url = "file://" + videoPath;
#endif

        Debug.Log($"����� ��������� �� ����: {videoPlayer.url}");
    }
}
