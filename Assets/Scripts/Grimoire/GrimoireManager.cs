using Events;
using GameData.Scripts.Items;
using System.Collections.Generic;
using UnityEngine;

public class GrimoireManager : MonoBehaviour
{
    [Header("All Scriptable Objects")]
    [SerializeField] private List<Ingredient> allIngredients;
    [SerializeField] private List<Potion> allPotions;
    [SerializeField] private List<Spell> allSpells;

    private List<GrimoireEntry<Ingredient>> ingredientEntries;
    private List<GrimoireEntry<Potion>> potionEntries;
    private List<GrimoireEntry<Spell>> spellEntries;

    private void OnEnable()
    {
        InteractionEvents.OnIngredientPickup += OnIngredientPickup;
        AlchemyEvents.OnPotionCrafted += OnPotionCrafted;
    }

    private void OnDisable()
    {
        InteractionEvents.OnIngredientPickup -= OnIngredientPickup;
        AlchemyEvents.OnPotionCrafted -= OnPotionCrafted;
    }
    private void Awake()
    {
        ingredientEntries = BuildEntries(allIngredients);
        potionEntries = BuildEntries(allPotions);
        spellEntries = BuildEntries(allSpells);
    }

    private List<GrimoireEntry<T>> BuildEntries<T>(List<T> list) where T : ScriptableObject
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
            entry.discovered = true;
    }
}