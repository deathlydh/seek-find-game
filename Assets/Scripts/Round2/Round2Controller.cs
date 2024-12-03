using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round2Controller : MonoBehaviour
{
    private PrefabWithAnswers? actualAnimal;
    private int trueAnswerIndex;
    private string[] answers = new string[4];
    private bool poolIsNotEmpty = true;
    public static int CountAnswers = 0;

    [SerializeField]
    private ParticleSystem particleSystem;

    private void DetermineAnswers()
    {
        if (actualAnimal == null) return;

        trueAnswerIndex = Random.Range(0, answers.Length);

        for (int i = 0; i < answers.Length; i++)
        {
            answers[i] = i == trueAnswerIndex
                ? actualAnimal.Value.TrueAnswer
                : actualAnimal.Value.OtherAnswers[i < trueAnswerIndex ? i : i - 1];
        }
    }

    private void ChangeFrame()
    {
        if (particleSystem.isPlaying)
            particleSystem.Stop();

        if (!poolIsNotEmpty) return;

        var imgPool = GetComponent<Round2ImgPool>();
        if (imgPool == null)
        {
            Debug.LogError("Round2ImgPool компонент не найден!");
            return;
        }

        var newAnimal = imgPool.GetNewFrameInfo();
        if (newAnimal == null)
        {
            Debug.LogWarning("Больше нет доступных вопросов.");
            poolIsNotEmpty = false;
            Round2StateMahine.EndGame?.Invoke();
            return;
        }

        actualAnimal = newAnimal.Value;
        DetermineAnswers();

        // Отправляем события
        Round2StateMahine.SetAnswers?.Invoke(answers);
        Round2StateMahine.SetImg?.Invoke(actualAnimal.Value.imgPrefabs);
        Round2StateMahine.OnGoodStage?.Invoke(true);
        Round2StateMahine.OnSetAnimal?.Invoke(actualAnimal.Value.supAnswer);
        Round2StateMahine.setStage1?.Invoke();
    }

    public void OnStage1Click(bool answer)
    {
        if (actualAnimal == null) return;

        if (answer == actualAnimal.Value.AnswerIsTrue)
        {
            if (actualAnimal.Value.AnswerIsTrue)
            {
                particleSystem.Play();
                GoodAnswer(true);
                CountAnswers++;
                Invoke(nameof(ChangeFrame), 0.6f);
            }
            else
            {
                GoodAnswer(false);
                Invoke(nameof(SetStage2), 0.3f);
            }
        }
        else
        {
            WrongAnswer();
            Invoke(nameof(ChangeFrame), 0.6f);
        }
    }

    public void OnStage2Click(int selectedAnswer)
    {
        if (selectedAnswer == trueAnswerIndex)
        {
            particleSystem.Play();
            GoodAnswer(true);
            CountAnswers++;
            Invoke(nameof(ChangeFrame), 0.8f);
        }
        else
        {
            WrongAnswer(selectedAnswer);
            Invoke(nameof(ChangeFrame), 0.8f);
        }
    }

    private void GoodAnswer(bool isCorrect)
    {
        Round2StateMahine.OnGoodStage?.Invoke(isCorrect);
        Round2StateMahine.OnGoodStage2?.Invoke(trueAnswerIndex);
    }

    private void WrongAnswer()
    {
        Round2StateMahine.OnWrongStage?.Invoke(actualAnimal?.AnswerIsTrue ?? false);
    }

    private void WrongAnswer(int selectedAnswer)
    {
        Round2StateMahine.OnWrongStage2?.Invoke(selectedAnswer, trueAnswerIndex);
    }

    private void SetStage2()
    {
        Round2StateMahine.setStage2?.Invoke();
    }

    private void Final()
    {
        poolIsNotEmpty = false;
    }

    private void Start()
    {
        particleSystem.Stop();
        Round2StateMahine.EndGame += Final;
    }

    public void StartGame()
    {
        SaveSystem.SaveFirstStage(SaveSystem.GetSave(SaveSystem.count - 1));
        Round2StateMahine.setStage1?.Invoke();
        Round2StateMahine.StartGame?.Invoke();

        ChangeFrame();
    }

    private void OnDestroy()
    {
        Round2StateMahine.EndGame -= Final;
    }
}
