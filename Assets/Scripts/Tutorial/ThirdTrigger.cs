using UnityEngine;

public class ThirdAreaTrigger : MonoBehaviour
{
    [SerializeField] private TutorialManager tutorialManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialManager.OnPlayerReachedThirdArea();
            gameObject.SetActive(false);
        }
    }
}
