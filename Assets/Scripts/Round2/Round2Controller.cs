using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round2Controller : MonoBehaviour
{
    private PrefabWithAnswers actualAnimal;
    private int TrueAnswerIndex;
    private string[] answers = new string[4];
    private bool PoolIsNotEmpty = true;
    private int CountAnswers = 0;
    
    private void DetermineAnswers(){
        TrueAnswerIndex = UnityEngine.Random.Range(0, answers.Length);

        for(int i = 0; i < answers.Length; i++)
        {
            if(TrueAnswerIndex == i){
                answers[i] = actualAnimal.TrueAnswer;
            }else{
                answers[i] = actualAnimal.OtherAnswers[i < TrueAnswerIndex ? i : i - 1];
            }
        }
    }

    private void ChangeFrame(){
        if(PoolIsNotEmpty){
            actualAnimal = GetComponent<Round2ImgPool>().GetNewFrameInfo();
            DetermineAnswers();

            Round2StateMahine.SetAnswers?.Invoke(answers);
            Round2StateMahine.SetImg?.Invoke(actualAnimal.imgPrefabs);
            Round2StateMahine.OnGoodStage.Invoke(true);

            Round2StateMahine.OnSetAnimal.Invoke(actualAnimal.supAnswer);

            Round2StateMahine.setStage1();
        }
    }

    private void WrongAnswer(){
        Round2StateMahine.OnWrongStage.Invoke(actualAnimal.AnswerIsTrue);
    }

    private void WrongAnswer(int _ind){
        Round2StateMahine.OnWrongStage2.Invoke(_ind, TrueAnswerIndex);
    }

    private void GoodAnswer(bool answer){
        Round2StateMahine.OnGoodStage.Invoke(answer);
        Round2StateMahine.OnGoodStage2.Invoke(TrueAnswerIndex);
    }

    private void SetStage2(){
        Round2StateMahine.setStage2();
    }

    public void OnStage1Click(bool _answer){
        if(_answer == actualAnimal.AnswerIsTrue){
            if(actualAnimal.AnswerIsTrue){
                GoodAnswer(actualAnimal.AnswerIsTrue);
                CountAnswers++;
                Invoke("ChangeFrame", 1.5f);
            }else{
                GoodAnswer(actualAnimal.AnswerIsTrue);
                Invoke("SetStage2", 1.5f);
            }
            
        }else{
            WrongAnswer();
            Invoke("ChangeFrame", 1.5f);
        }
    }

    public void OnStage2Click(int _answer){
        if(_answer == TrueAnswerIndex){
            GoodAnswer(actualAnimal.AnswerIsTrue);
            CountAnswers++;
            Invoke("ChangeFrame", 1.5f);
        }else{
            WrongAnswer(_answer);
            Invoke("ChangeFrame", 1.5f);
        }
    }

    private void Final(){
        PoolIsNotEmpty = false;
        SaveSystem.Save(CountAnswers);
    }

    public void StartGame(){

        SaveSystem.SaveFirstStage(SaveSystem.GetSave(SaveSystem.count-1));

        ChangeFrame();
        Round2StateMahine.setStage1();

        Round2StateMahine.OnPoolEmpty += Final;
    }
}
