using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimalQuizManagerRound2 : MonoBehaviour
{
    [Header("UI Elements")]
    public Image animalImageDisplay;
    public TextMeshProUGUI animalNameText;
    public GameObject yesNoPanel;
    public GameObject answerOptionsPanel;
    public GameObject gameOverPanel;
    public Button[] answerButtons; // Four buttons for answer options
    public Button yesButton;       // Button for 'Yes' answer
    public Button noButton;        // Button for 'No' answer
    public TextMeshProUGUI correctAnswerText;

    [Header("Quiz Data")]
    public AnimalConfirmationData[] quizData; // Array of quiz data for each photo

    private int currentPhotoIndex = 0;

    private void Start()
    {
        if (quizData.Length > 0)
        {
            // Set up button listeners for Yes and No buttons
            yesButton.onClick.AddListener(OnYesButtonPressed);
            noButton.onClick.AddListener(OnNoButtonPressed);

            LoadCurrentPhoto();
        }
        else
        {
            Debug.LogWarning("No quiz data provided!");
            gameOverPanel.SetActive(true);
        }
    }

    private void LoadCurrentPhoto()
    {
        if (currentPhotoIndex >= quizData.Length)
        {
            gameOverPanel.SetActive(true);
            yesNoPanel.SetActive(false);
            answerOptionsPanel.SetActive(false);
            return;
        }

        var currentData = quizData[currentPhotoIndex];
        animalImageDisplay.sprite = currentData.animalImage;
        animalNameText.text = $"На фото {currentData.animalOnPhotoName}";
        yesNoPanel.SetActive(true);
        answerOptionsPanel.SetActive(false);
    }

    public void OnYesButtonPressed()
    {
        if (quizData[currentPhotoIndex].isCorrectAnimal)
        {
            NextPhoto();
        }
        else
        {
            ShowAnswerOptions();
        }
    }

    public void OnNoButtonPressed()
    {
        if (!quizData[currentPhotoIndex].isCorrectAnimal)
        {
            ShowAnswerOptions();
        }
        else
        {
            NextPhoto();
        }
    }

    private void ShowAnswerOptions()
    {
        yesNoPanel.SetActive(false);
        answerOptionsPanel.SetActive(true);

        var currentData = quizData[currentPhotoIndex];
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentData.answerOptions[i];
            int index = i; // Local copy for lambda
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(currentData.answerOptions[index]));
        }
    }

    private void OnAnswerSelected(string selectedAnswer)
    {
        var currentData = quizData[currentPhotoIndex];
        if (selectedAnswer == currentData.correctAnswer)
        {
            correctAnswerText.text = "Правильно!";
        }
        else
        {
            correctAnswerText.text = $"Неправильно! Правильный ответ: {currentData.correctAnswer}";
        }
        NextPhoto();
    }

    private void NextPhoto()
    {
        currentPhotoIndex++;
        if (currentPhotoIndex < quizData.Length)
        {
            LoadCurrentPhoto();
        }
        else
        {
            gameOverPanel.SetActive(true);
        }
    }
}
