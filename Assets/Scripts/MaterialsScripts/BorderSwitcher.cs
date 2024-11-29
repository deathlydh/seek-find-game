using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BorderSwitcher : MonoBehaviour
{
    [SerializeField]
    Color baseBorder;
    [SerializeField]
    Color wrongBorder;

    [SerializeField]
    Color baseBorder1;
    [SerializeField]
    Color wrongBorder1;

    Image Border;
    Image Border1;
    void Start()
    {
        Border = GetComponent<Image>();
        Border1 = this.gameObject.transform.GetChild(0).GetComponent<Image>();

        Round2StateMahine.OnWrongStage += SetWrong;
        Round2StateMahine.OnGoodStage += SetGood;

        Round2StateMahine.OnWrongStage2 += SetWrong;
        Round2StateMahine.OnGoodStage2 += SetGood;
    }

    void setMat(Color border, Color border1)
    {
        Border.material.SetColor("_Color", border);
        Border1.material.SetColor("_Color", border1);
    }

    public void SetWrong(bool IsWrong)
    {
        if (IsWrong) 
        {
            setMat(baseBorder, baseBorder1);
        }
        else
        {
            setMat(wrongBorder, wrongBorder1);
        }
    }

    private void SetWrong()
    {
        SetWrong(false);
    }
    private void SetGood(bool a)
    {
        SetWrong(true);
    }
    private void SetWrong(int a, int b)
    {
        SetWrong(false);
    }
    private void SetGood(int a)
    {
        SetWrong(true);
    }
}
