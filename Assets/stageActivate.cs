using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class stageActivate : MonoBehaviour
{
    [SerializeField]
    Round2State stateActive;

    private void Activate()
    {
        gameObject.SetActive(true);
    }

    private void DeActivate()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        if (stateActive == Round2State.First)
        { 
            Round2MachinState.ActivStage1 += Activate;
            Round2MachinState.ActivStage2 += DeActivate;
            gameObject.SetActive(true);
        }
        else
        {
            Round2MachinState.ActivStage2 += Activate;
            Round2MachinState.ActivStage1 += DeActivate;
            gameObject.SetActive(false);
        }
    }
}
