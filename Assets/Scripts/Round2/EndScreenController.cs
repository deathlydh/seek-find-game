using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenController : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text _Player;
    [SerializeField]
    TMPro.TMP_Text _AI;
    [SerializeField]
    GameObject obj;

    void SetText(){
        obj?.SetActive(true);
        //_Player.SetText(SaveSystem.GetFirstStage().ToString());
        _AI.SetText(SaveSystem.GetSave(SaveSystem.GetCount()-1).ToString());
    }

    void Awake(){
        Round2StateMahine.EndGame += SetText;
    }

    void OnDestroy(){
        Round2StateMahine.EndGame -= SetText;
    }
}
