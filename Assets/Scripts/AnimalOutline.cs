using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalOutline : MonoBehaviour
{
    public GameObject outlineObject; // ������ �� ������ � ��������

    public void SetOutlineActive(bool isActive)
    {
        outlineObject.SetActive(isActive);
    }
}
