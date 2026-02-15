using StarterAssets;
using System.Collections;
using UnityEngine;

public class FirstTrial : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject swiftRetreatVFX;
    [SerializeField] private GameObject mimicGroup;
    public bool invulnerability = false;

    private void OnEnable()
    {
        SpellEvents.OnTrialStarted += OnTrialStarted;
        SpellEvents.OnShieldUsed += ProtectionStarted;
        SpellEvents.OnShieldEnded += ProtectionEnded;
        SpellEvents.OnInvulnerabilityUsed += ProtectionStarted;
        SpellEvents.OnInvulnerabilityEnded += ProtectionEnded;
        SpellEvents.OnSwiftretreatEnded += Suspend;
        SpellEvents.OnClarovencyUsed += EnableHighlight;
        SpellEvents.OnClarovencyEnded += DisableHighlight;
        SpellEvents.OnArrowCollision += CollisionCheck;
        SpellEvents.OnTreasureInteracted += EndTrial;
        SpellEvents.OnMimicInteracted += TeleportAndSuspend;
    }
    private void OnDisable()
    {
        SpellEvents.OnTrialStarted -= OnTrialStarted;
        SpellEvents.OnShieldUsed -= ProtectionStarted;
        SpellEvents.OnShieldEnded -= ProtectionEnded;
        SpellEvents.OnInvulnerabilityUsed -= ProtectionStarted;
        SpellEvents.OnInvulnerabilityEnded -= ProtectionEnded;
        SpellEvents.OnSwiftretreatEnded -= Suspend;
        SpellEvents.OnClarovencyUsed -= EnableHighlight;
        SpellEvents.OnClarovencyEnded -= DisableHighlight;
        SpellEvents.OnArrowCollision -= CollisionCheck;
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

    private void Suspend()
    {
        SpellEvents.OnTrialSuspended?.Invoke();
    }

    private void TeleportAndSuspend()
    {
        SpellEvents.OnTrialSuspended?.Invoke();
        Teleport(new Vector3(8f, 0.2f, 20f));
    }

    private void EnableHighlight()
    {
        mimicGroup.SetActive(false);
    }

    private void DisableHighlight()
    {
        mimicGroup.SetActive(true);
    }

    private void OnTrialStarted(int trialIndex)
    {
        if (trialIndex == 0)
        {
            Teleport(new Vector3(-25.7f, 0.2f, 34.9f));
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

    private void EndTrial()
    {
        StartCoroutine(EndRoutine());
    }

    private IEnumerator EndRoutine()
    {
        swiftRetreatVFX.SetActive(true);
        var ps = swiftRetreatVFX.GetComponent<ParticleSystem>();

        ps.Play();

        yield return new WaitForSeconds(2f);

        SpellEvents.OnTrialCompleted?.Invoke();
        Teleport(new Vector3(8f, 0.2f, 20f));

        yield return new WaitForSeconds(2f);

        ps.Stop();
        swiftRetreatVFX.SetActive(false);
    }
}
