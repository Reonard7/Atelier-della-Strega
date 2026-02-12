using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameData.Scripts.Items;
using Events;

public class Dice : MonoBehaviour
{
    [Header("Dice UI")]
    [SerializeField] private Image diceImage;              // Dice
    [SerializeField] private TextMeshProUGUI diceResult;   // Result

    [Header("Potion UI")]
    [SerializeField] private Image potionImage;            // Potion
    [SerializeField] private TextMeshProUGUI potionName;   // Name

    private bool isRolling;
    private Potion cachedPotion;
    private bool cachedIsMirable;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip rollSound;

    private void Awake()
    {
        // Initial visibility
        diceImage.enabled = false;
        diceResult.enabled = false;

        potionImage.enabled = false;
        potionName.enabled = false;
    }

    private void OnEnable()
    {
        AlchemyEvents.OnBrewingStarted += OnBrewingStarted;
        AlchemyEvents.OnPotionCrafted += CachePotionResult;
    }

    private void OnDisable()
    {
        AlchemyEvents.OnBrewingStarted -= OnBrewingStarted;
        AlchemyEvents.OnPotionCrafted -= CachePotionResult;
    }

    // Called when Brew() starts
    private void OnBrewingStarted()
    {
        // Hide potion result
        potionImage.enabled = false;
        potionName.enabled = false;

        // Show dice
        diceImage.enabled = true;
        diceResult.enabled = false;

        isRolling = true;

        int number = Random.Range(1, 21);
        diceResult.text = number.ToString();

        AlchemyEvents.OnDiceCast?.Invoke(number);
    }

    public void OnRollAnimationFinished()
    {
        if (!isRolling) return;

        diceResult.enabled = true;
        isRolling = false;
    }

    public void OnDiceRevealFinished()
    {
        if (cachedPotion == null) return;

        // Hide dice
        diceImage.enabled = false;
        diceResult.enabled = false;

        // Show potion
        potionImage.sprite = cachedPotion.icon;
        potionName.text = cachedIsMirable
            ? $"{cachedPotion.displayName} (Meravigliosa)"
            : cachedPotion.displayName;

        potionImage.enabled = true;
        potionName.enabled = true;

        // Reset cache
        cachedPotion = null;
        cachedIsMirable = false;

        AlchemyEvents.OnAnimationEnded?.Invoke();
    }

    public void PlayRollSound()
    {
        audioSource.PlayOneShot(rollSound);
    }

    private void CachePotionResult(Potion potion, bool isMirable)
    {
        cachedPotion = potion;
        cachedIsMirable = isMirable;
    }
}
