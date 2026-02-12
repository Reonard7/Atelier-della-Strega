using Events;
using GameData.Scripts.Items;
using UnityEngine;

public class AudioPotions : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip failedSound;
    [SerializeField] private AudioClip mysteriousSound;
    [SerializeField] private AudioClip normalSound;
    [SerializeField] private AudioClip mirableSound;

    private Potion lastCraftedPotion;
    private bool lastCraftedWasMirable;

    private void OnEnable()
    {
        AlchemyEvents.OnPotionCrafted += StorePotion;
        AlchemyEvents.OnAnimationEnded += PlayCraftAudioAfterAnimation;
    }

    private void OnDisable()
    {
        AlchemyEvents.OnPotionCrafted -= StorePotion;
        AlchemyEvents.OnAnimationEnded -= PlayCraftAudioAfterAnimation;
    }

    private void StorePotion(Potion potion, bool isMirable)
    {
        lastCraftedPotion = potion;
        lastCraftedWasMirable = isMirable;
    }

    private void PlayCraftAudioAfterAnimation()
    {
        if (audioSource == null || lastCraftedPotion == null) return;

        switch (lastCraftedPotion.id)
        {
            case "thrash_potion":
                audioSource.PlayOneShot(failedSound);
                break;

            case "mysterious_potion":
                audioSource.PlayOneShot(mysteriousSound);
                break;

            default:
                if (lastCraftedWasMirable)
                {
                    audioSource.PlayOneShot(mirableSound);
                }
                else
                {
                    audioSource.PlayOneShot(normalSound);
                }
                break;
        }

        // Puliamo i dati dopo aver riprodotto il suono
        lastCraftedPotion = null;
    }
}
