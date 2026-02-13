using StarterAssets;
using UnityEngine;
using Cinemachine;
using System.Collections;
using Events;
using Interaction;
using System.Runtime.InteropServices;

public class PortalInteractable : MonoBehaviour, IInteractable
{
    private int trailIndex;
    private void Awake()
    {
        trailIndex = GetComponent<TrialPortal>().trialIndex;
    }
    public void Interact(GameObject caller)
    {
        if (TrialManager.Instance.GetTrialState(trailIndex) == TrialManager.TrialState.InProgress) return;

        Debug.Log($"Trail started: {trailIndex + 1}");
        SpellEvents.OnTrialStarted?.Invoke(trailIndex);
    }
}
