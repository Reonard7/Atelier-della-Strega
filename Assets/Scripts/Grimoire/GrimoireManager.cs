using Events;
using GameData.Scripts.Items;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class GrimoireManager : MonoBehaviour
{
    [SerializeField] private GameObject grimoireCanvas;
    private FirstPersonController playerFPS;
    private bool inCrafting;

    [Header("All Scriptable Objects")]
    [SerializeField] private List<Ingredient> allIngredients;
    [SerializeField] private List<Potion> allPotions;
    [SerializeField] private List<Spell> allSpells;

    private List<GrimoireEntry<Ingredient>> ingredientEntries;
    private List<GrimoireEntry<Potion>> potionEntries;
    private List<GrimoireEntry<Spell>> spellEntries;

    public List<GrimoireEntry<Ingredient>> IngredientEntries => ingredientEntries;
    public List<GrimoireEntry<Potion>> PotionEntries => potionEntries;
    public List<GrimoireEntry<Spell>> SpellEntries => spellEntries;

    private void OnEnable()
    {
        InteractionEvents.OnIngredientPickup += OnIngredientPickup;
        AlchemyEvents.OnPotionCrafted += OnPotionCrafted;
        InteractionEvents.OnCauldronInteracted += OnCauldronInteracted;
        InteractionEvents.OnCauldronExit += OnCauldronExit;
    }

    private void OnDisable()
    {
        InteractionEvents.OnIngredientPickup -= OnIngredientPickup;
        AlchemyEvents.OnPotionCrafted -= OnPotionCrafted;
        InteractionEvents.OnCauldronInteracted -= OnCauldronInteracted;
        InteractionEvents.OnCauldronExit -= OnCauldronExit;
    }
    private void Awake()
    {
        ingredientEntries = BuildEntries(allIngredients);
        potionEntries = BuildEntries(allPotions);
        spellEntries = BuildEntries(allSpells);
    }

    private void Start()
    {
        playerFPS = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();
        grimoireCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!inCrafting && Input.GetKeyDown(KeyCode.Tab))
        {
            if (!grimoireCanvas.activeSelf)
            {
                playerFPS.enabled = false;
                grimoireCanvas.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                playerFPS.enabled = true;
                grimoireCanvas.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    private List<GrimoireEntry<T>> BuildEntries<T>(List<T> list) where T : IGrimoireData
    {
        var result = new List<GrimoireEntry<T>>();
        foreach (var item in list)
        {
            result.Add(new GrimoireEntry<T>
            {
                data = item,
                discovered = false
            });
        }
        return result;
    }

    private void OnIngredientPickup(Ingredient ingredient)
    {
        var entry = ingredientEntries.Find(e => e.data == ingredient);
        if (entry != null)
            entry.discovered = true;
    }

    private void OnPotionCrafted(Potion potion, bool rarity)
    {
        var entry = potionEntries.Find(e => e.data == potion);
        if (entry != null)
        {
            entry.discovered = true;
            GrimoireEvents.OnEntryDiscovered?.Invoke(entry.data);
        }
    }
    private void OnCauldronInteracted()
    {
        inCrafting = true;
    }

    private void OnCauldronExit()
    {
        inCrafting = false;
    }
}