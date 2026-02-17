using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixerSnapshot defaultSnapshot;
    public AudioMixerSnapshot trialSnapshot;
    public float transitionTime = 1f;

    private void OnEnable()
    {
        SpellEvents.OnTrialStarted += HandleTrialStarted;
        SpellEvents.OnTrialCompleted += HandleTrialEnded;
        SpellEvents.OnTrialSuspended += HandleTrialEnded;
    }

    private void OnDisable()
    {
        SpellEvents.OnTrialStarted -= HandleTrialStarted;
        SpellEvents.OnTrialCompleted -= HandleTrialEnded;
        SpellEvents.OnTrialSuspended -= HandleTrialEnded;
    }

    void Start()
    {
        defaultSnapshot.TransitionTo(0f);
    }

    private void HandleTrialStarted(int trialIndex)
    {
        trialSnapshot.TransitionTo(transitionTime);
    }

    private void HandleTrialEnded()
    {
        defaultSnapshot.TransitionTo(transitionTime);
    }
}
