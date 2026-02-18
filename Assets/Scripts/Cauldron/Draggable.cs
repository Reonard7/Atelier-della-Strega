using Events;
using GameData.Scripts.Items;
using Inventory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * Classe che definisce i comportamenti drag&drop degli ingredienti della hotbar.
 * Expected behaviours:
 * - Dentro la hierarchy, gli slot si spostano di parent, dalla UI generica a quella del crafting
 * - Gli ingredienti fanno uno snap in place quando vengono trascinati in uno slot del crafting
 * - Gli ingredienti fanno uno snap back quando spostati in una posizione che non sia uno slot del crafting
 */
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler  
{
    // tutte le reference ai canvas (CanvasGroup serve per il raycasting)
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private HotbarSlot hotbarSlot;      // reference alla classe HotbarSlot, dove si trovano tutte le informazioni relative a cosa rappresenta l'ingrediente

    // variabili relative alla posizione originale, servono per lo snap back
    private Transform homeParent;
    private Vector3 homeLocalPosition;

    // reference allo slot di crafting
    private IngredientSlot ingredientSlot;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        hotbarSlot = GetComponentInParent<HotbarSlot>();

        homeParent = transform.parent;
        homeLocalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        AlchemyEvents.OnBrewingEnded += OnBrewingEnded;
    }

    private void OnDisable()
    {
        AlchemyEvents.OnBrewingEnded -= OnBrewingEnded;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // questa parte serve a gestire la rimozione dell'ingrediente dalla lista interna a AlchemyManager
        // Se IngredientSlot � presente, vuol dire che l'ingrediente si trova attualmente in uno slot di crafting
        IngredientSlot ingredientSlot = GetComponentInParent<IngredientSlot>();

        if (ingredientSlot != null)
        {
            Ingredient ingredient = ingredientSlot.GetIngredient();
            if (ingredient != null)
            {
                AlchemyEvents.OnIngredientRemovedFromSlot?.Invoke(ingredient);
            }
            ingredientSlot.Clear();
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

        // se non viene droppato su uno slot di crafting, snap back nella hotbar
        if (transform.parent == canvas.transform)
        {
            transform.SetParent(homeParent);
            transform.localPosition = homeLocalPosition;
        }
        var originalSlot = homeParent.GetComponent<IngredientSlot>();
        if (originalSlot != null)
        {
            originalSlot.OnDrop(eventData); 
        }
    }

    private void OnBrewingEnded()
    {
        transform.SetParent(homeParent);
        transform.localPosition = homeLocalPosition;
        canvas = GetComponentInParent<Canvas>();
        canvasGroup.blocksRaycasts = true;
    }

    public Ingredient GetIngredient()
    {
        // Helper method chiamato per ottenere la reference all'ingrediente nello slot di crafting
        IngredientSlot ingredientSlot = GetComponentInParent<IngredientSlot>();
        if (ingredientSlot != null)
            return ingredientSlot.GetIngredient();

        return hotbarSlot.ingredient;
    }
}
