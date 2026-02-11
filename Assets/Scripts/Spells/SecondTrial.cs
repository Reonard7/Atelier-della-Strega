using StarterAssets;
using UnityEngine;

public class SecondTrial : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] braziers;
    private bool inArea = false;

    private void OnEnable()
    {
        SpellEvents.OnTrialStarted += OnTrialStarted;
        SpellEvents.OnSwiftretreatUsed += TeleportAndSuspend;
        SpellEvents.OnFirebreathEnded += CheckIfEnded;
    }
    private void OnDisable()
    {
        SpellEvents.OnTrialStarted -= OnTrialStarted;
        SpellEvents.OnSwiftretreatUsed -= TeleportAndSuspend;
        SpellEvents.OnFirebreathEnded -= CheckIfEnded;
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
