using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController1 : MonoBehaviour
{
    public VideoPlayer videoPlayer; // ������ �� VideoPlayer
    public GameObject uiPanel; // ������ �� ��������� (��������� UI-��������)

    private bool videoPlaying = true;

    void Start()
    {
        // ���������, ��� ��������� ��������
        if (uiPanel != null)
            uiPanel.SetActive(false);
    }

    void Update()
    {
        // ��������� ������� ������, �������� ��� �����������
        if (videoPlaying && (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
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
}
