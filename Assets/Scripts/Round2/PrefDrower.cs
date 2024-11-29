using DA_Assets.Shared.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefDrower : MonoBehaviour
{
    [SerializeField]
    GameObject border;
    [SerializeField]
    GameObject content;

    public void SetImg(GameObject go)
    {
        content?.Destroy();
        content = Instantiate(go, border.transform);
    }

    void Awake(){
        Round2StateMahine.SetImg += SetImg;
    }
}
