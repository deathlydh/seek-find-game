using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizButtonController : MonoBehaviour
{
    public Sprite inactiveSprite;  // Спрайт для неактивного состояния
    public Sprite activeSprite;    // Спрайт для активного состояния
    public Sprite correctSprite;   // Спрайт для правильного ответа
    public Sprite incorrectSprite; // Спрайт для неправильного ответа

    public TMP_Text buttonText;    // Ссылка на TextMeshPro для текста кнопки
    public Color activeTextColor = Color.white; // Цвет текста в активном состоянии
    public Color inactiveTextColor = Color.gray; // Цвет текста в неактивном состоянии
    public Color incorrectTextColor = new Color32(145, 82, 174, 255); // Цвет текста для состояния "Incorrect"

    private Image buttonImage;     // Ссылка на компонент Image кнопки

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        SetInactive(); // Устанавливаем начальное состояние
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
            buttonText.color = activeTextColor; // Цвет текста для правильного ответа
        }
    }

    public void SetIncorrect()
    {
        buttonImage.sprite = incorrectSprite;

        if (buttonText != null)
        {
            buttonText.color = incorrectTextColor; // Цвет текста для неправильного ответа
        }
    }
}
