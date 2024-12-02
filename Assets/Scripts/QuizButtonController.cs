using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizButtonController : MonoBehaviour
{
    public Sprite inactiveSprite;  // ������ ��� ����������� ���������
    public Sprite activeSprite;    // ������ ��� ��������� ���������
    public Sprite correctSprite;   // ������ ��� ����������� ������
    public Sprite incorrectSprite; // ������ ��� ������������� ������

    public TMP_Text buttonText;    // ������ �� TextMeshPro ��� ������ ������
    public Color activeTextColor = Color.white; // ���� ������ � �������� ���������
    public Color inactiveTextColor = Color.gray; // ���� ������ � ���������� ���������
    public Color incorrectTextColor = new Color32(145, 82, 174, 255); // ���� ������ ��� ��������� "Incorrect"

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

        if (buttonText != null)
        {
            buttonText.color = inactiveTextColor;
        }
    }

    public void SetActive()
    {
        buttonImage.sprite = activeSprite;
        GetComponent<Button>().interactable = true;

        if (buttonText != null)
        {
            buttonText.color = activeTextColor;
        }
    }

    public void SetCorrect()
    {
        buttonImage.sprite = correctSprite;

        if (buttonText != null)
        {
            buttonText.color = activeTextColor; // ���� ������ ��� ����������� ������
        }
    }

    public void SetIncorrect()
    {
        buttonImage.sprite = incorrectSprite;

        if (buttonText != null)
        {
            buttonText.color = incorrectTextColor; // ���� ������ ��� ������������� ������
        }
    }
}
