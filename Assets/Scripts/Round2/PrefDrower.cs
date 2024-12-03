using DA_Assets.Shared.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefDrower : MonoBehaviour
{
    [SerializeField]
    GameObject border;
    GameObject content;

    [SerializeField]
    GameObject little;

    public void SetImg(GameObject go)
    {
        content?.Destroy();
        content = Instantiate(go, border.transform);
        content.transform.parent = border.transform;
        content.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        Round2StateMahine.SetLittleBorder.Invoke(new Trans{ position = content.GetComponent<BoxCollider>().center, scale =  content.GetComponent<BoxCollider>().size});
    }

    void Awake(){
        Round2StateMahine.SetImg += SetImg;
    }
    void OnDestroy(){
        Round2StateMahine.SetImg -= SetImg;
    }
}
