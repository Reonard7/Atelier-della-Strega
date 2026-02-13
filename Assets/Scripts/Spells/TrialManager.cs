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
    public TrialState PreviousState { get; private set; }
    public int CurrentTrialIndex = 0;

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

    private void OnEnable()
    {
        SpellEvents.OnTrialStarted += StartTrial;
        SpellEvents.OnTrialSuspended += SuspendCurrentTrial;
        SpellEvents.OnTrialCompleted += CompleteCurrentTrial;
    }
    private void OnDisable()
    {
        SpellEvents.OnTrialStarted -= StartTrial;
        SpellEvents.OnTrialCompleted -= CompleteCurrentTrial;
    }

    public TrialState GetTrialState(int index)
    {
        return trialStates[index];
    }

    public void StartTrial(int index)
    {
        if (trialStates[index] == TrialState.InProgress) return;

        CurrentTrialIndex = index;
        PreviousState = trialStates[index];
        trialStates[index] = TrialState.InProgress;
    }

    public void SuspendCurrentTrial()
    {
        if (PreviousState == TrialState.Completed)
        {
            trialStates[CurrentTrialIndex] = TrialState.Completed;
        }
        else
        {
            trialStates[CurrentTrialIndex] = TrialState.Idle;
        }
    }
    public void CompleteCurrentTrial()
    {
        trialStates[CurrentTrialIndex] = TrialState.Completed;
    }
}
