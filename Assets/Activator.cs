using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    [SerializeField]
    StateRound2 stage;
    [SerializeField]
    int ind;
    [SerializeField]
    QuizButtonController btn;


    void Show(){
        gameObject.SetActive(true);
        GetComponent<QuizButtonController>().SetActive();
    }

    void Hide(){
        gameObject.SetActive(false);
        this.GetComponent<QuizButtonController>().SetInactive();
    }

    void SetText(string[] answers){
        if( GetComponent<QuizButtonController>().isActiveAndEnabled){
            gameObject.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().SetText(answers[ind]);
        }
        else{
            gameObject.SetActive(true);
            GetComponent<QuizButtonController>().SetActive();
            gameObject.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().SetText(answers[ind]);
            gameObject.SetActive(false);
            this.GetComponent<QuizButtonController>().SetInactive();
        }
    }

    void SetWrong(int notTrueAnswerIndex, int TrueAnswerIndex){
        if(notTrueAnswerIndex == ind){
            GetComponent<QuizButtonController>().SetIncorrect();
        }else{
            if(TrueAnswerIndex == ind){
                GetComponent<QuizButtonController>().SetActive();
            }
            else
            {
                GetComponent<QuizButtonController>().SetInactive();
            }
        }
    }

    void SetWrong(bool TrueAnswer){
        if(TrueAnswer == (ind > 0)){
            GetComponent<QuizButtonController>().SetCorrect();
        }else{
            GetComponent<QuizButtonController>().SetIncorrect();
        }
            
    }
    
    private void SetGood(bool TrueAnswer)
    {
        if(TrueAnswer == (ind>0)){
            GetComponent<QuizButtonController>().SetCorrect();
        }else{
            GetComponent<QuizButtonController>().SetInactive();
        }
    }

    private void SetGood(int TrueAnswer)
    {
        if(TrueAnswer == ind){
            GetComponent<QuizButtonController>().SetCorrect();
        }else{
            GetComponent<QuizButtonController>().SetInactive();
        }
    }
    void Start(){
        if(stage == StateRound2.Stage1){
            StagePanelController.Sctiv1+=Show;
            Round2StateMahine.setStage2+=Hide;

            Round2StateMahine.OnGoodStage += SetGood;
            Round2StateMahine.OnWrongStage += SetWrong;
        }else{
            StagePanelController.Sctiv2+=Show;
            Round2StateMahine.setStage1+=Hide;

            Round2StateMahine.OnGoodStage2 += SetGood;
            Round2StateMahine.SetAnswers += SetText;
            Round2StateMahine.OnWrongStage2 += SetWrong;
        }  
    }

    void OnDestroy()
    {
        if(stage == StateRound2.Stage1){
            StagePanelController.Sctiv1-=Show;
            Round2StateMahine.setStage2-=Hide;

            Round2StateMahine.OnGoodStage -= SetGood;
            Round2StateMahine.OnWrongStage -= SetWrong;
        }else{
            StagePanelController.Sctiv2-=Show;
            Round2StateMahine.setStage1-=Hide;

            Round2StateMahine.OnGoodStage2 -= SetGood;
            Round2StateMahine.SetAnswers -= SetText;
            Round2StateMahine.OnWrongStage2 -= SetWrong;
        }
    }
}
/*
    void Awake()
    {
        if(stage == StateRound2.Stage1){
            StagePanelController.Sctiv1+=Show;
            Round2StateMahine.setStage2+=Hide;

            Round2StateMahine.OnGoodStage += SetGood;
            Round2StateMahine.OnWrongStage += SetWrong;
        }else{
            StagePanelController.Sctiv2+=Show;
            Round2StateMahine.setStage1+=Hide;

            Round2StateMahine.OnGoodStage2 += SetGood;
            Round2StateMahine.SetAnswers += SetText;
            Round2StateMahine.OnWrongStage2 += SetWrong;
        }  
    }*/

