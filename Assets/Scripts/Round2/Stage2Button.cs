using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Stage2Button : MonoBehaviour
{
    [SerializeField]
    int indexBtn;

    void setTxt(string[] animals)
    {
        gameObject.transform.GetChild(0).GetComponent<TMP_Text>().SetText(animals[indexBtn]);
    }

    void Start()
    {
        Round2MachinState.SetAnswers += setTxt;
        GetComponent<QuizButtonController>().SetActive();
    }
}
