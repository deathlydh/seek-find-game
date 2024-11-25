using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefDrower : MonoBehaviour
{
    [SerializeField]
    GameObject border;

    public void SetImg(GameObject go)
    {
        Instantiate(go, border.transform);
    }
}
