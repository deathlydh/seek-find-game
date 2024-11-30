using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSetter : MonoBehaviour
{
    void setText(string animal){
        this.GetComponent<TMPro.TMP_Text>().SetText(animal);
    }
    void Start()
    {
        Round2StateMahine.OnSetAnimal += setText;
    }

    void OnDestroy()
    {
        Round2StateMahine.OnSetAnimal -= setText;
    }
}
