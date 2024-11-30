using System;
using System.Collections;
using System.Collections.Generic;
using DA_Assets.FCU.Model;
using UnityEngine;
using UnityEngine.UIElements;

public class StagePanelController : MonoBehaviour
{
    [SerializeField]
    private StateRound2 stage;
    public static Action Sctiv1;
    public static Action Sctiv2;

    [SerializeField]
    private string thisStage;

    private void Show(){
        transform.Find(thisStage).gameObject.SetActive(true);
         if(stage == StateRound2.Stage1){
            Sctiv1.Invoke();
        }else{
            Sctiv2.Invoke();
        }
    }
    private void Hide(){
        transform.Find(thisStage).gameObject.SetActive(false);
    }

    void Start()
    {
        if(stage == StateRound2.Stage1){
            Round2StateMahine.setStage1 += Show;
            Round2StateMahine.setStage2 += Hide;
        }else{
            Round2StateMahine.setStage2 += Show;
            Round2StateMahine.setStage1 += Hide;
        }
    }

    void OnDestroy(){
        if(stage == StateRound2.Stage1){
            Round2StateMahine.setStage1 -= Show;
            Round2StateMahine.setStage2 -= Hide;
        }else{
            Round2StateMahine.setStage2 -= Show;
            Round2StateMahine.setStage1 -= Hide;
        }
    }
}
