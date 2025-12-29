using Events;
using GameData.Scripts.Items;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler  
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private HotbarSlot hotbarSlot;

    private Transform homeParent;
    private Vector3 homeLocalPosition;

    private IngredientSlot ingredientSlot;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        hotbarSlot = GetComponentInParent<HotbarSlot>();

        homeParent = transform.parent;
        homeLocalPosition = transform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IngredientSlot ingredientSlot = GetComponentInParent<IngredientSlot>();

        if (ingredientSlot != null)
        {
            Ingredient ingredient = ingredientSlot.GetIngredient();
            if (ingredient != null)
            {
                AlchemyEvents.OnIngredientRemovedFromSlot?.Invoke(ingredient);
                Debug.Log("OnIngredientRemovedFromSlot invoked");
                ingredientSlot.Clear();
            }
        }

        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // If not dropped into a slot
        if (transform.parent == canvas.transform)
        {
            transform.SetParent(homeParent);
            transform.localPosition = homeLocalPosition;
        }
    }

    public Ingredient GetIngredient()
    {
        IngredientSlot ingredientSlot = GetComponentInParent<IngredientSlot>();
        if (ingredientSlot != null)
            return ingredientSlot.GetIngredient();

        return hotbarSlot.ingredient;
    }
}
