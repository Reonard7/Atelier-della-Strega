using Events;
using GameData.Scripts.Items;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyManager : MonoBehaviour
{
    /*
     * Cosa deve fare il manager:
     * 
     * - Tenere una reference ai tre ingredienti negli slot
     * - Un metodo chiamabile per trovare la pozione corretta in relazione ai tre ingredienti
     * - Metodo che dia in output la pozione craftata (da passare al grimorio -> evento!)
     * - Tutto relativo al dado (far startare l'animazione e restituire il numero generato)
     * - Metodo che printi la pozione creata a schermo
     * 
     * 
    */

    [Header("Craftable Potions")]
    [SerializeField] private List<Potion> craftablePotions;
    [SerializeField] private List<Ingredient> ingredients;

    private void OnEnable()
    {
        AlchemyEvents.OnIngredientDropped += OnIngredientDropped;
        AlchemyEvents.OnIngredientRemovedFromSlot += OnIngredientRemovedFromSlot;
    }

    private void OnDisable()
    {
        AlchemyEvents.OnIngredientDropped -= OnIngredientDropped;
        AlchemyEvents.OnIngredientRemovedFromSlot -= OnIngredientRemovedFromSlot;
    }

    private void OnIngredientDropped(Ingredient ingredient)
    {
        ingredients.Add(ingredient);
    }

    private void OnIngredientRemovedFromSlot(Ingredient ingredient)
    {
        Debug.Log("OnIngredientRemovedFromSlot received on manager");
        ingredients.Remove(ingredient);
    }
}
