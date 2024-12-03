using DA_Assets.Shared.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefDrower : MonoBehaviour
{
    [SerializeField]
    GameObject border;
    GameObject content;

    public void SetImg(GameObject go)
    {
        content?.Destroy();
        content = Instantiate(go, border.transform);
        content.transform.parent = border.transform;
    }

    void Awake(){
        Round2StateMahine.SetImg += SetImg;
    }
    void OnDestroy(){
        Round2StateMahine.SetImg -= SetImg;
    }
}
