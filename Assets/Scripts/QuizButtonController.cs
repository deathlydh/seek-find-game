using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizButtonController : MonoBehaviour
{
    public Sprite inactiveSprite;  // ������ ��� ����������� ���������
    public Sprite activeSprite;    // ������ ��� ��������� ���������
    public Sprite correctSprite;   // ������ ��� ����������� ������
    public Sprite incorrectSprite; // ������ ��� ������������� ������

    private Image buttonImage;     // ������ �� ��������� Image ������

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        SetInactive(); // ������������� ��������� ���������
    }

    public void SetInactive()
    {
        buttonImage.sprite = inactiveSprite;
        GetComponent<Button>().interactable = false;
    }

    public void SetActive()
    {
        buttonImage.sprite = activeSprite;
        GetComponent<Button>().interactable = true;
    }

    public void SetCorrect()
    {
        buttonImage.sprite = correctSprite;
    }

    public void SetIncorrect()
    {
        buttonImage.sprite = incorrectSprite;
    }
}
