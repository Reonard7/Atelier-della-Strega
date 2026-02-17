using UnityEngine;
using StarterAssets;
using System.Collections;

public class ThirdTrial : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject swiftRetreatVFX;
    [SerializeField] private GameObject objectToRespawn;

    private void OnEnable()
    {
        SpellEvents.OnTrialStarted += OnTrialStarted;
        SpellEvents.OnSwiftretreatEnded += Suspend;
        SpellEvents.OnPlatformCollision += EndTrial;
    }
    private void OnDisable()
    {
        SpellEvents.OnTrialStarted -= OnTrialStarted;
        SpellEvents.OnSwiftretreatEnded -= Suspend;
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

    private void Suspend()
    {
        SpellEvents.OnTrialSuspended?.Invoke();
    }

    private void TeleportAndSuspend()
    {
        SpellEvents.OnTrialSuspended?.Invoke();
        Teleport(new Vector3(10.01f, 0.02f, 23.38f));
    }

    private void OnTrialStarted(int trialIndex)
    {
        if (trialIndex == 2)
        {
            Teleport(new Vector3(10f, -7.7f, 74.4f));
        }
    }

    private void EndTrial()
    {
        objectToRespawn.SetActive(true);
        StartCoroutine(EndRoutine());
    }

    private IEnumerator EndRoutine()
    {
        swiftRetreatVFX.SetActive(true);
        var ps = swiftRetreatVFX.GetComponent<ParticleSystem>();

        ps.Play();

        yield return new WaitForSeconds(2f);

        SpellEvents.OnTrialCompleted?.Invoke();
        Teleport(new Vector3(10.01f, 0.02f, 23.38f));

        yield return new WaitForSeconds(2f);

        ps.Stop();
        swiftRetreatVFX.SetActive(false);
    }
}
