using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BorderSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
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
    }

    void setMat(Color border, Color border1)
    {
        Border.material.SetColor("_Color", border);
        Border1.material.SetColor("_Color", border1);
    }

    public void SetWrong(bool IsWrong)
    {
        if (!IsWrong) 
        {
            setMat(baseBorder, baseBorder1);
        }
        else
        {
            setMat(wrongBorder, wrongBorder1);
        }
    }
}
