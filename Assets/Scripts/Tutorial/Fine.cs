using System.Linq;
using UnityEngine;

public class Fine : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TutorialManager tutorialManager;

    private bool alreadyTriggered = false;

    private void Start()
    {
        // Controlla ogni 0.5 secondi se tutti i trial sono completati
        InvokeRepeating(nameof(CheckTrials), 1f, 0.5f);
    }

    private void CheckTrials()
    {
        if (alreadyTriggered) return;

        if (TrialManager.Instance == null) return;
        if (tutorialManager == null) return;

        // Controlla se tutti i trial sono Completed
        if (TrialManager.Instance.trialStates
            .All(state => state == TrialManager.TrialState.Completed))
        {
            alreadyTriggered = true;
            CancelInvoke();
            TriggerFinalDialogue();
        }
    }

    private void TriggerFinalDialogue()
    {
        Debug.Log("Tutti i trial completati! Avvio dialogo finale.");

        tutorialManager.StartFinalDialogue();
    }
}
