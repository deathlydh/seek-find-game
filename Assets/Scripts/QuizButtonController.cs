using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizButtonController : MonoBehaviour
{
    public Sprite inactiveSprite;  // Спрайт для неактивного состояния
    public Sprite activeSprite;    // Спрайт для активного состояния
    public Sprite correctSprite;   // Спрайт для правильного ответа
    public Sprite incorrectSprite; // Спрайт для неправильного ответа

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
