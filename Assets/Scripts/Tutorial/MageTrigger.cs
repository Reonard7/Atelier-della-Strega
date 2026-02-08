using UnityEngine;

public class MageTrigger : MonoBehaviour
{
    [Header("Tutorial Manager")]
    [SerializeField] private TutorialManager tutorialManager;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return; 

        if (other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("[MageTrigger] Player entrato nella zona della maga");
            tutorialManager.OnPlayerReachedMage();
        }
    }
}
