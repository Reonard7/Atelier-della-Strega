using System;
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
    public RawImage slotImage;             // L'immagine di questo oggetto (lo slot stesso)
    [Header("Impostazioni Grafiche")]
    public Texture emptySprite;          // Sprite Rombo con "?"
    public Texture filledSprite;         // Sprite Rombo pulito
    
    public Ingredient _currentIngredient;
    
    public void Start()
    {
        UpdateSlotVisuals();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // check necessario per evitare che l'ingrediente venga inserito nella lista provando a mettere un ingrediente sopra l'altro
        if (_currentIngredient != null) return;

        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable == null) return;

        Ingredient ingredient = draggable.GetIngredient();
        if (ingredient == null) return;

        _currentIngredient = ingredient;
        AlchemyEvents.OnIngredientDropped?.Invoke(ingredient);

        if (transform.childCount > 0) return; // slot occupied
        draggable.transform.SetParent(transform);
        draggable.transform.position = transform.position;

        UpdateSlotVisuals();
    }

    public Ingredient GetIngredient()
    {
        return _currentIngredient;
    }

    public void Clear()
    {
        _currentIngredient = null;
        UpdateSlotVisuals();
    }

    private void UpdateSlotVisuals()
    {
        if (!slotImage) return;
        
        slotImage.texture = _currentIngredient ? filledSprite : emptySprite;
    }
}
