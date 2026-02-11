using UnityEngine;
using StarterAssets;

public class ThirdTrial : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void OnEnable()
    {
        SpellEvents.OnTrialStarted += OnTrialStarted;
        SpellEvents.OnSwiftretreatUsed += TeleportAndSuspend;
        SpellEvents.OnTreasureInteracted += EndTrial;
        SpellEvents.OnMimicInteracted += TeleportAndSuspend;
        SpellEvents.OnPlatformCollision += EndTrial;
    }
    private void OnDisable()
    {
        SpellEvents.OnTrialStarted -= OnTrialStarted;
        SpellEvents.OnSwiftretreatUsed -= TeleportAndSuspend;
        SpellEvents.OnTreasureInteracted -= EndTrial;
        SpellEvents.OnMimicInteracted -= TeleportAndSuspend;
        SpellEvents.OnPlatformCollision -= EndTrial;
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
        if (trialIndex == 2)
        {
            Teleport(new Vector3(0f, -6f, 36.2f));
        }
    }

    private void EndTrial()
    {
        SpellEvents.OnTrialCompleted?.Invoke();
        Teleport(new Vector3(8f, 0.2f, 20f));
    }
}
