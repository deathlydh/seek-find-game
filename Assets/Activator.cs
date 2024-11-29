using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    [SerializeField]
    StateRound2 stage;
    [SerializeField]
    int ind;

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
    

    void Awake()
    {
        GetComponent<QuizButtonController>().SetActive();

        if(stage == StateRound2.Stage1){
            StagePanelController.Sctiv1+=Show;
            Round2StateMahine.setStage2+=Hide;
        }else{
            StagePanelController.Sctiv2+=Show;
            Round2StateMahine.setStage1+=Hide;

            Round2StateMahine.SetAnswers += SetText;
        }
    }
}
