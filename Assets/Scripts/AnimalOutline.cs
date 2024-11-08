using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalOutline : MonoBehaviour
{
    public GameObject outlineObject; // Ссылка на объект с обводкой

    public void SetOutlineActive(bool isActive)
    {
        outlineObject.SetActive(isActive);
    }
}
