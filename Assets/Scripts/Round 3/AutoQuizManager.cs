using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoQuizManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public AnimalQuestionConfig[] autoQuestions;
    private List<AnimalQuestionConfig> availableQuestions = new List<AnimalQuestionConfig>();
    private AnimalQuestionConfig currentQuestion;
    private GameObject currentAnimalInstance;
    [SerializeField] private RectTransform animalContainer;
    public GameObject gameOverPanel;
    public Image selectionOutline;
    public ParticleSystem correctAnswerParticles;
    [SerializeField] private float fixOffsetX = 0f;
    [SerializeField] private float fixOffsetY = 0f;
    [SerializeField] private BorderSwitcher border;
    private BoxCollider2D containerCollider;
    public GameObject prestartPanel; // Назначьте в инспекторе
    public TMP_Text animalNameText;  // Текст для отображения названия животного
    private int currentScore = 0;

    

    private void Start()
    {
        availableQuestions = new List<AnimalQuestionConfig>(autoQuestions);
        containerCollider = animalContainer.GetComponent<BoxCollider2D>();
        gameOverPanel.SetActive(false);
        border.SetWrong(false); // Добавлено

        if (prestartPanel != null)
            prestartPanel.SetActive(true);

        SaveSystem.init();
    }

    // Метод, который вызывается кнопкой Start
    public void StartQuiz()
    {
        if (prestartPanel != null)
        {
            prestartPanel.SetActive(false); // Скрываем PrestartPanel
        }

        LoadNextQuestion(); // Начинаем игру
    }

    private void LoadNextQuestion()
    {
        if (availableQuestions.Count == 0)
        {
            EndAutoRound();
            return;
        }

        int randomIndex = Random.Range(0, availableQuestions.Count);
        currentQuestion = availableQuestions[randomIndex];
        availableQuestions.RemoveAt(randomIndex);

        Vector3 spawnPosition = containerCollider.bounds.center;
        currentAnimalInstance = Instantiate(currentQuestion.animalPrefab, spawnPosition, Quaternion.identity, animalContainer);
        FitPrefabToCollider(currentAnimalInstance, containerCollider);

        StartCoroutine(AutoAnswer());
    }

    private IEnumerator AutoAnswer()
    {
        // 1. Ждем 1 секунду перед выделением
        yield return new WaitForSeconds(0.7f);

        // 2. Активируем рамку выделения
        var animalCollider = currentAnimalInstance.GetComponent<BoxCollider>();
        if (animalCollider != null && selectionOutline != null)
        {
            UpdateOutline(selectionOutline.rectTransform, animalCollider);
            selectionOutline.gameObject.SetActive(true);
        }

        // 3. Активируем BorderSwitcher (если используется)
        border.SetWrong(false); // Аналогично AnimalQuizManager

        // 4. Отображаем название животного
        if (animalNameText != null)
        {
            animalNameText.text = currentQuestion.correctAnswer;
            animalNameText.gameObject.SetActive(true);
        }

        // 5. Эффект частиц
        if (correctAnswerParticles != null)
        {
            Vector3 topOfScreen = new Vector3(0, Camera.main.orthographicSize, 0);
            ParticleSystem particles = Instantiate(correctAnswerParticles, topOfScreen, Quaternion.identity);
            Destroy(particles.gameObject, 1.5f);
        }

        // 6. Увеличение счёта
        currentScore++;
        scoreManager.score++;
        SaveSystem.Save(scoreManager.score);

        // 7. Задержка перед следующим вопросом
        yield return new WaitForSeconds(2f);

        // 8. Очистка
        if (animalNameText != null)
            animalNameText.gameObject.SetActive(false);

        if (currentAnimalInstance != null)
            Destroy(currentAnimalInstance);

        selectionOutline.gameObject.SetActive(false);

        LoadNextQuestion();
    }

    private void UpdateOutline(RectTransform outline, BoxCollider collider)
    {
        if (outline == null || collider == null) return;


        Vector3 colliderSize = collider.size;
        Vector3 colliderCenter = collider.center;


        Vector3 offset = new Vector3(fixOffsetX, fixOffsetY, 0);


        Vector3 worldCenter = collider.transform.TransformPoint(colliderCenter + offset);
        Vector3 worldSize = Vector3.Scale(colliderSize, collider.transform.lossyScale);


        Vector2 canvasLocalPosition = animalContainer.transform.InverseTransformPoint(worldCenter);


        outline.sizeDelta = new Vector2(worldSize.x / animalContainer.transform.lossyScale.x, worldSize.y / animalContainer.transform.lossyScale.y);
        outline.anchoredPosition = canvasLocalPosition;
    }

    private void FitPrefabToCollider(GameObject prefab, BoxCollider2D containerCollider)
    {
        SpriteRenderer spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Vector2 spriteSize = spriteRenderer.bounds.size;
            Vector2 colliderSize = containerCollider.bounds.size;
            Vector3 scale = prefab.transform.localScale;
            scale.x *= colliderSize.x / spriteSize.x;
            scale.y *= colliderSize.y / spriteSize.y;
            prefab.transform.localScale = scale;
        }
    }

    public void EndAutoRound()
    {
        SaveSystem.Save(scoreManager.score);
        StopAllCoroutines();
        gameOverPanel.SetActive(true);
    }
}
