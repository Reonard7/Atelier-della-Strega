using UnityEngine;

public class TrialDialogueController : MonoBehaviour
{
    [Header("Start Messages (Index = Trial Index)")]
    [TextArea(3,6)]
    [SerializeField] private string[] startMessages;

    [Header("Retreat Message")]
    [TextArea(3,6)]
    [SerializeField] private string retreatMessage;

    private void OnEnable()
    {
        SpellEvents.OnTrialStarted += OnTrialStarted;
        SpellEvents.OnTrialSuspended += OnTrialSuspended;
    }

    private void OnDisable()
    {
        SpellEvents.OnTrialStarted -= OnTrialStarted;
        SpellEvents.OnTrialSuspended -= OnTrialSuspended;
    }

    private void OnTrialStarted(int trialIndex)
    {
        if (trialIndex < startMessages.Length)
        {
            DialogueManager.Instance.ShowMessage(startMessages[trialIndex]);
        }
    }

    private void OnTrialSuspended()
    {
        DialogueManager.Instance.ShowMessage(retreatMessage);
    }
}
