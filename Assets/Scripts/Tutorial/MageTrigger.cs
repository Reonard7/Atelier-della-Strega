using UnityEngine;

public class MageTrigger : MonoBehaviour
{
    [SerializeField] private TutorialManager tutorialManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialManager.OnPlayerReachedMage();
            // opzionale: disabilitare il trigger dopo l'attivazione
            gameObject.SetActive(false);
        }
    }
}

