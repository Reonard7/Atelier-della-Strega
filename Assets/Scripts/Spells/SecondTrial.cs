using StarterAssets;
using UnityEngine;

public class SecondTrial : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void OnEnable()
    {
        SpellEvents.OnTrialStarted += OnTrialStarted;
        SpellEvents.OnSwiftretreatUsed += TeleportAndSuspend;
        SpellEvents.OnTreasureInteracted += EndTrial;
        SpellEvents.OnMimicInteracted += TeleportAndSuspend;
    }
    private void OnDisable()
    {
        SpellEvents.OnTrialStarted -= OnTrialStarted;
        SpellEvents.OnSwiftretreatUsed -= TeleportAndSuspend;
        SpellEvents.OnTreasureInteracted -= EndTrial;
        SpellEvents.OnMimicInteracted -= TeleportAndSuspend;
    }
    private void Teleport(Vector3 teleportPos)
    {
        var fps = player.GetComponent<FirstPersonController>();
        var cc = player.GetComponent<CharacterController>();

        cc.enabled = false;
        fps.enabled = false;
        player.transform.position = teleportPos;
        cc.enabled = true;
        fps.enabled = true;
    }

    private void TeleportAndSuspend()
    {
        SpellEvents.OnTrialSuspended?.Invoke();
        Teleport(new Vector3(8f, 0.2f, 20f));
    }

    private void OnTrialStarted(int trialIndex)
    {
        if (trialIndex == 1)
        {
            Teleport(new Vector3(-3.6f, 0.2f, 28.3f));
        }
    }

    private void EndTrial()
    {
        SpellEvents.OnTrialCompleted?.Invoke();
        Teleport(new Vector3(8f, 0.2f, 20f));
    }
}
