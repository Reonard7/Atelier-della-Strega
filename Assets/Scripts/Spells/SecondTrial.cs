using StarterAssets;
using System.Collections;
using UnityEngine;

public class SecondTrial : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject swiftRetreatVFX;
    [SerializeField] private GameObject[] braziers;
    [SerializeField] private GameObject objectToRespawn;
    [SerializeField] private GameObject respawDice;
    private bool inArea = false;

    private void OnEnable()
    {
        SpellEvents.OnTrialStarted += OnTrialStarted;
        SpellEvents.OnSwiftretreatEnded += Suspend;
        SpellEvents.OnFirebreathEnded += CheckIfEnded;
        SpellEvents.OnFireballEnded += TeleportAndSuspend;
    }
    private void OnDisable()
    {
        SpellEvents.OnTrialStarted -= OnTrialStarted;
        SpellEvents.OnSwiftretreatEnded -= Suspend;
        SpellEvents.OnFirebreathEnded -= CheckIfEnded;
        SpellEvents.OnFireballEnded -= TeleportAndSuspend;
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
        var state = TrialManager.Instance.trialStates[1];

        if (state == TrialManager.TrialState.InProgress)
        {
            SpellEvents.OnTrialSuspended?.Invoke();
            Teleport(new Vector3(10.01f, 0.02f, 23.38f));
        }
        
    }

    private void OnTrialStarted(int trialIndex)
    {
        if (trialIndex == 1)
        {
            Teleport(new Vector3(-12.221f, 0.268f, 55.708f));
        }
    }

    private void EndTrial()
    {
        foreach (GameObject brazier in braziers)
        {
            brazier.GetComponent<BrazerCollider>().ResetBrazier();
        }

        respawDice.SetActive(true);
        objectToRespawn.SetActive(true);

        StartCoroutine(EndRoutine());
    }

    private IEnumerator EndRoutine()
    {
        yield return new WaitForSeconds(3f);
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

    private void CheckIfEnded()
    {
        if (TrialManager.Instance.trialStates[1] == TrialManager.TrialState.Completed) return;

        bool ended = true;

        for (int i = 0; i < braziers.Length; i++)
        {
            if (!braziers[i].transform.GetChild(0).gameObject.activeSelf)
            {
                ended = false;
                break;
            }
        }

        Debug.Log(ended);

        if (ended)
        {
            EndTrial();
        }
    }
}
