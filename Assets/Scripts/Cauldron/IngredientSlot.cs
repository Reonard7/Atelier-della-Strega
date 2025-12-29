using Events;
using GameData.Scripts.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientSlot : MonoBehaviour, IDropHandler
{
    private Ingredient currentIngredient;
    public void OnDrop(PointerEventData eventData)
    {
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
