using UnityEngine;

public class TrialDialogueController : MonoBehaviour
{
    [Header("Trial 1 Lines")]
    [TextArea(2, 5)] [SerializeField] private string[] trial1Lines;
    [SerializeField] private AudioClip[] trial1Audio;

    [Header("Trial 2 Lines")]
    [TextArea(2, 5)] [SerializeField] private string[] trial2Lines;
    [SerializeField] private AudioClip[] trial2Audio;

    [Header("Trial 3 Lines")]
    [TextArea(2, 5)] [SerializeField] private string[] trial3Lines;
    [SerializeField] private AudioClip[] trial3Audio;

    [Header("Retreat Message")]
    [TextArea(2, 5)] [SerializeField] private string retreatMessage;
    [SerializeField] private AudioClip retreatAudio;

    [Header("Completed Lines")]
    [TextArea(2, 5)] [SerializeField] private string completeMessage;
    [SerializeField] private AudioClip completeAudio;

    [Header("Fireball Lines")]
    [TextArea(2, 5)] [SerializeField] private string fireballMessage;
    [SerializeField] private AudioClip fireballAudio;

    [Header("Mimic Lines")]
    [TextArea(2, 5)] [SerializeField] private string mimicMessage;
    [SerializeField] private AudioClip mimicAudio;

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    private void OnEnable()
    {
        SpellEvents.OnTrialStarted += OnTrialStarted;
        SpellEvents.OnTrialCompleted += OnTrialCompleted; 
        SpellEvents.OnSwiftretreatEnded += OnSwiftretreatEnded;
        SpellEvents.OnFireballEnded += OnFireballEnded;
        SpellEvents.OnMimicInteracted += OnMimicInteracted;

        DialogueManager.OnDialogueEnded += StopAudio;
    }

    private void OnDisable()
    {
        SpellEvents.OnTrialStarted -= OnTrialStarted;
        SpellEvents.OnTrialCompleted -= OnTrialCompleted;
        SpellEvents.OnSwiftretreatEnded -= OnSwiftretreatEnded;
        SpellEvents.OnFireballEnded -= OnFireballEnded;
        SpellEvents.OnMimicInteracted -= OnMimicInteracted;

        DialogueManager.OnDialogueEnded -= StopAudio;
    }

    private void PlayAudio(AudioClip clip)
    {
        if (clip == null || audioSource == null)
            return;

        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = clip;
        audioSource.Play();
    }

    private void StopAudio()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void OnTrialStarted(int trialIndex)
    {
        string[] linesToShow = null;
        AudioClip[] audioToPlay = null;

        switch(trialIndex)
        {
            case 0:
                linesToShow = trial1Lines;
                audioToPlay = trial1Audio;
                break;
            case 1:
                linesToShow = trial2Lines;
                audioToPlay = trial2Audio;
                break;
            case 2:
                linesToShow = trial3Lines;
                audioToPlay = trial3Audio;
                break;
        }

        if (linesToShow != null && linesToShow.Length > 0)
        {
            DialogueManager.Instance.StartDialogue(linesToShow);

            if (audioToPlay != null && audioToPlay.Length > 0)
            {
                PlayAudio(audioToPlay[0]);
            }
        }
    }

    private void OnSwiftretreatEnded()
    {
        DialogueManager.Instance.StartDialogue(new string[] { retreatMessage });
        PlayAudio(retreatAudio);
    }

    private void OnTrialCompleted()
    {
        DialogueManager.Instance.StartDialogue(new string[] { completeMessage });
        PlayAudio(completeAudio);
    }

    private void OnFireballEnded()
    {
        DialogueManager.Instance.StartDialogue(new string[] { fireballMessage });
        PlayAudio(fireballAudio);
    }

    private void OnMimicInteracted()
    {
        DialogueManager.Instance.StartDialogue(new string[] { mimicMessage });
        PlayAudio(mimicAudio);
    }
}
