using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round2Controller : MonoBehaviour
{
    private PrefabWithAnswers actualAnimal;
    private int TrueAnswerIndex;
    private string[] answers = new string[4];
    
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
        actualAnimal = GetComponent<Round2ImgPool>().GetNewFrameInfo();
        DetermineAnswers();



        Round2StateMahine.SetAnswers?.Invoke(answers);
        Round2StateMahine.SetImg?.Invoke(actualAnimal.imgPrefabs);

        Round2StateMahine.setStage1();
    }

    private void WrongAnswer(){

    }

    private void GoodAnswer(){

    }

    public void OnStage1Click(bool _answer){
        if(_answer == actualAnimal.AnswerIsTrue){
            if(actualAnimal.AnswerIsTrue){
                GoodAnswer();
                ChangeFrame();
            }else{
                Round2StateMahine.setStage2();
            }
            
        }else{
            WrongAnswer();
        }
    }

    public void OnStage2Click(int _answer){
        if(_answer == TrueAnswerIndex){
            ChangeFrame();
        }else{
            WrongAnswer();
        }
    }

    void Start(){
        ChangeFrame();
        Round2StateMahine.setStage1();
    }
}
