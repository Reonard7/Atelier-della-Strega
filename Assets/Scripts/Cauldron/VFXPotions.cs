using Events;
using GameData.Scripts.Items;
using UnityEngine;

public class PotionCraftVFX : MonoBehaviour
{
    private enum PotionState
    {
        Trash,
        Normal,
        Mirable,
        Mysterious
    }

    [Header("Calderone VFX (fumo)")]
    [SerializeField] private ParticleSystem cauldronSmoke;

    [Header("Scintille")]
    [SerializeField] private ParticleSystem normalSparklePrefab;
    [SerializeField] private ParticleSystem trashSparklePrefab;
    [SerializeField] private ParticleSystem mirableSparklePrefab;
    [SerializeField] private ParticleSystem mysteriousSparklePrefab;

    private Potion lastCraftedPotion;
    private PotionState lastPotionState;

    private void OnEnable()
    {
        AlchemyEvents.OnPotionCrafted += StorePotion;
        AlchemyEvents.OnAnimationEnded += PlayVFXAfterAnimation;
    }

    private void OnDisable()
    {
        AlchemyEvents.OnPotionCrafted -= StorePotion;
        AlchemyEvents.OnAnimationEnded -= PlayVFXAfterAnimation;
    }

    private void StorePotion(Potion potion, bool isMirable)
    {
        lastCraftedPotion = potion;

        if (potion.id == "thrash_potion")
            lastPotionState = PotionState.Trash;
        else if (potion.id == "mysterious_potion")
            lastPotionState = PotionState.Mysterious;
        else if (isMirable)
            lastPotionState = PotionState.Mirable;
        else
            lastPotionState = PotionState.Normal;
    }

    private void PlayVFXAfterAnimation()
    {
        if (lastCraftedPotion == null) return;

        // Fumo
        if (cauldronSmoke != null)
        {
            Color smokeColor = lastPotionState switch
            {
                PotionState.Trash => Color.black,
                PotionState.Mirable => Color.yellow,
                PotionState.Mysterious => Color.magenta,
                _ => Color.white
            };

            var main = cauldronSmoke.main;
            main.startColor = smokeColor;
            cauldronSmoke.Play();
        }

        // Scintille
        ParticleSystem selectedSparkle = lastPotionState switch
        {
            PotionState.Trash => trashSparklePrefab,
            PotionState.Mirable => mirableSparklePrefab,
            PotionState.Mysterious => mysteriousSparklePrefab,
            _ => normalSparklePrefab
        };

        if (selectedSparkle != null)
        {
            ParticleSystem sparks = Instantiate(
                selectedSparkle,
                cauldronSmoke.transform.position,
                Quaternion.identity
            );

            sparks.Play();
            Destroy(
                sparks.gameObject,
                sparks.main.duration + sparks.main.startLifetime.constantMax
            );
        }

        lastCraftedPotion = null;
    }
}
