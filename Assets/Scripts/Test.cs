using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
public class Test : MonoBehaviour
{
    private Collider2D collider2D;

    void Start()
    {
        collider2D = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        if (collider2D != null)
        {
            Debug.Log("Область была кликнута!");
            DoSomething();
        }
    }

    void DoSomething()
    {
        Debug.Log("Действие выполнено!");
    }

}