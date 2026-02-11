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

    public static TrialManager Instance;

    public List<TrialState> trialStates;
    public int CurrentTrialIndex { get; private set; } = 0;

    private void Awake()
    {
        Instance = this;

        trialStates = new List<TrialState>
        {
            TrialState.Idle,
            TrialState.Idle,
            TrialState.Idle
        };
    }

    public TrialState GetTrialState(int index)
    {
        return trialStates[index];
    }

    public void StartTrial(int index)
    {
        if (trialStates[index] != TrialState.InProgress) return;

        CurrentTrialIndex = index;
        Debug.Log($"Trial {index + 1} started");
    }

    public void CompleteCurrentTrial()
    {
        trialStates[CurrentTrialIndex] = TrialState.Completed;
    }

    /*
     * Prima prova: frecce e mimic
     * Oggetti da passare: frecce, mimic, forziere vero
     */
}
