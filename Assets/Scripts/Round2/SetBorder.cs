using System.Collections;
using System.Collections.Generic;
using DA_Assets.Shared.Extensions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SetBorder : MonoBehaviour
{
    public Vector2 SetSizeMin(RectTransform trans, Vector2 newSize) {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        return trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
    }

    public Vector2 SetSizeMax(RectTransform trans, Vector2 newSize) {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        return trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }

    void SetlittleBorder(Trans rect){
        GetComponent<RectTransform>().localPosition = rect.position;
        GetComponent<RectTransform>().offsetMax = SetSizeMax(GetComponent<RectTransform>(), rect.scale);
        GetComponent<RectTransform>().offsetMin = SetSizeMin(GetComponent<RectTransform>(), rect.scale);
    }

    void Awake()
    {
        Round2StateMahine.SetLittleBorder += SetlittleBorder;
    }
    void OnDestroy(){
        Round2StateMahine.SetLittleBorder -= SetlittleBorder;
    }
}
