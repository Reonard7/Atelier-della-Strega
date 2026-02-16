using UnityEngine;

public class TrialDialogueController : MonoBehaviour
{
    [Header("Trial 1 Lines")]
    [TextArea(2, 5)] [SerializeField] private string[] trial1Lines;

    [Header("Trial 2 Lines")]
    [TextArea(2, 5)] [SerializeField] private string[] trial2Lines;

    [Header("Trial 3 Lines")]
    [TextArea(2, 5)] [SerializeField] private string[] trial3Lines;

    [Header("Retreat Message")]
    [TextArea(2, 5)] [SerializeField] private string retreatMessage;

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
        string[] linesToShow = trialIndex switch
        {
            0 => trial1Lines,
            1 => trial2Lines,
            2 => trial3Lines,
            _ => null
        };

        if (linesToShow != null && linesToShow.Length > 0)
        {
            DialogueManager.Instance.StartDialogue(linesToShow);
        }
    }

    private void OnTrialSuspended()
    {
        DialogueManager.Instance.StartDialogue(new string[] { retreatMessage });
    }
}
