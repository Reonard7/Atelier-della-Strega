using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TrialManager : MonoBehaviour
{
    public enum TrialState
    {
        Idle,
        InProgress,
        Completed
    }

    [Header("Trial States")]
    public TrialState[] trialStates = new TrialState[3];

    public int CurrentTrialIndex { get; private set; } = 1;

    private void Start()
    {
        trialStates[0] = TrialState.Idle;
        trialStates[1] = TrialState.Idle;
        trialStates[2] = TrialState.Idle;
    }

    /*
     * Prima prova: frecce e mimic
     * Oggetti da passare: frecce, mimic, forziere vero
     */
}
