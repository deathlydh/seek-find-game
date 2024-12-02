using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreStartSceneManager : MonoBehaviour
{
    private void Start()
    {
        UsedQuestionsManager.Instance.ResetUsedQuestions();
    }
}
