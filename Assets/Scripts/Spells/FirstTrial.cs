using StarterAssets;
using UnityEngine;

public class FirstTrial : MonoBehaviour
{
    [SerializeField] private GameObject teleportPoint;
    [SerializeField] private GameObject player;

    private void OnEnable()
    {
        SpellEvents.OnTrialStarted += OnTrialStarted;
    }
    private void OnDisable()
    {
        SpellEvents.OnTrialStarted -= OnTrialStarted;
    }

    private void OnTrialStarted(int trialIndex)
    {
        if (trialIndex == 0)
        {
            var fps = player.GetComponentInChildren<FirstPersonController>();
            fps.enabled = false;
            player.transform.position = new Vector3(-3.6f, 0.2f, 18.9f);
            fps.enabled = true;
        }
    }
}
