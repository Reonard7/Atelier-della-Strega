using Events;
using GameData.Scripts.Items;
using UnityEngine;

public class PotionCraftVFX : MonoBehaviour
{
    [Header("Calderone VFX (fumo)")]
    [SerializeField] private ParticleSystem cauldronSmoke;  // Particle System del fumo
    [SerializeField] private ParticleSystem sparklePrefab;  // Scintille standard

    private Potion lastCraftedPotion;
    private string lastPotionState; // "trash", "mirable", "mysterious", "normal"

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

        // Determina lo stato della pozione
        if (potion.id == "thrash_potion")
            lastPotionState = "trash";
        else if (potion.id == "mysterious_potion")
            lastPotionState = "mysterious";
        else if (isMirable)
            lastPotionState = "mirable";
        else
            lastPotionState = "normal";
    }

    private void PlayVFXAfterAnimation()
    {
        if (lastCraftedPotion == null) return;

        // Cambia colore del fumo
        if (cauldronSmoke != null)
        {
            Color smokeColor = lastPotionState switch
            {
                "trash" => Color.black,
                "mirable" => Color.yellow,  // giallo/oro
                "mysterious" => Color.magenta,
                _ => Color.white             // default per le pozioni normali
            };

            var main = cauldronSmoke.main;
            main.startColor = smokeColor;
            cauldronSmoke.Play();
        }

        // Scintille standard
        if (sparklePrefab != null)
        {
            ParticleSystem sparks = Instantiate(
                sparklePrefab,
                cauldronSmoke.transform.position,
                Quaternion.identity
            );
            sparks.Play();
            Destroy(sparks.gameObject, sparks.main.duration + sparks.main.startLifetime.constantMax);
        }

        lastCraftedPotion = null;
    }
}
