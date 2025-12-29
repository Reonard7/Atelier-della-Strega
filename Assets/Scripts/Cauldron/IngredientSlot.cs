using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Draggable ingredient = eventData.pointerDrag.GetComponent<Draggable>();
        if (ingredient != null)
        {
            // Move the ingredient into this slot
            ingredient.transform.SetParent(transform);
            ingredient.transform.position = transform.position; // Snap to center
        }
    }
}
