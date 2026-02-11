using StarterAssets;
using UnityEngine;

public class FirstTrial : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public bool invulnerability = false;

    private void OnEnable()
    {
        SpellEvents.OnTrialStarted += OnTrialStarted;
        SpellEvents.OnShieldUsed += ProtectionStarted;
        SpellEvents.OnShieldEnded += ProtectionEnded;
        SpellEvents.OnInvulnerabilityUsed += ProtectionStarted;
        SpellEvents.OnInvulnerabilityEnded += ProtectionEnded;
        SpellEvents.OnSwiftretreatUsed += TeleportAndSuspend;
        SpellEvents.OnArrowCollision += CollisionCheck;
    }
    private void OnDisable()
    {
        SpellEvents.OnTrialStarted -= OnTrialStarted;
        SpellEvents.OnShieldUsed -= ProtectionStarted;
        SpellEvents.OnShieldEnded -= ProtectionEnded;
        SpellEvents.OnInvulnerabilityUsed -= ProtectionStarted;
        SpellEvents.OnInvulnerabilityEnded -= ProtectionEnded;
        SpellEvents.OnSwiftretreatUsed -= TeleportAndSuspend;
        SpellEvents.OnArrowCollision -= CollisionCheck;
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
        if (trialIndex == 0)
        {
            Teleport(new Vector3(-3.6f, 0.2f, 18.9f));
        }
    }

    private void ProtectionStarted()
    {
        invulnerability = true;
    }

    private void ProtectionEnded()
    {
        invulnerability = false;
    }

    private void CollisionCheck()
    {
        if (invulnerability) return;

        TeleportAndSuspend();
    }
}
