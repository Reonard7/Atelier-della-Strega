using UnityEngine;

public class BrazerCollider : MonoBehaviour
{
    private bool inPlace = false;
    private bool alreadyLit = false;

    private bool active = false;

    private void OnEnable()
    {
        SpellEvents.OnTrialStarted += OnTrialStarted;
        SpellEvents.OnTrialSuspended += OnTrialSuspended;
        SpellEvents.OnTrialCompleted += OnTrialCompleted;
    }

    private void OnDisable()
    {
        SpellEvents.OnTrialStarted -= OnTrialStarted;
        SpellEvents.OnTrialSuspended -= OnTrialSuspended;
        SpellEvents.OnTrialCompleted -= OnTrialCompleted;
    }

    private void OnTrialStarted(int index)
    {
        if (index == 1) // second trial
        {
            active = true;
            SpellEvents.OnFirebreathUsed += OnFirebreathUsed;
        }
    }

    private void OnTrialSuspended()
    {
        Deactivate();
    }

    private void OnTrialCompleted()
    {
        Deactivate();
    }

    private void Deactivate()
    {
        active = false;
        SpellEvents.OnFirebreathUsed -= OnFirebreathUsed;
        ResetBrazier();
    }

    private void OnFirebreathUsed()
    {
        if (!active) return;

        if (inPlace && !alreadyLit)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            alreadyLit = true;
        }
    }

    public void ResetBrazier()
    {
        alreadyLit = false;
        inPlace = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!active) return;

        if (other.CompareTag("Player"))
            inPlace = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            inPlace = false;
    }
}
