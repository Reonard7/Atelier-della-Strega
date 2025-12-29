using Events;
using GameData.Scripts.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * Classe che definisce lo slot di crafting.
 * Expected behaviours:
 * - Gestisce tutta la logica del drop
 * - L'icona della hotbar viene messa al centro dello slot
 * - Invoca l'evento per aggiungere l'ingrediente alla lista nel manager
 */
public class IngredientSlot : MonoBehaviour, IDropHandler
{
    private Ingredient currentIngredient;
    public void OnDrop(PointerEventData eventData)
    {
        // check necessario per evitare che l'ingrediente venga inserito nella lista provando a metetre un ingrediente sopra l'altro
        if (currentIngredient != null) return;

        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable == null) return;

        Ingredient ingredient = draggable.GetIngredient();
        if (ingredient == null) return;

        currentIngredient = ingredient;
        AlchemyEvents.OnIngredientDropped?.Invoke(ingredient);

        if (transform.childCount > 0) return; // slot occupied
        draggable.transform.SetParent(transform);
        draggable.transform.position = transform.position;
    }

    public Ingredient GetIngredient()
    {
        return currentIngredient;
    }

    public void Clear()
    {
        currentIngredient = null;
    }
}
