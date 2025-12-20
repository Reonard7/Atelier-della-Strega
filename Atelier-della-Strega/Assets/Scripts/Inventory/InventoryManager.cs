using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject _hotbar;
    private List<Ingredient> _inventory;
    private Image[] _hotbarSlots;
    private int activeSlotIndex = 0;

    private void OnEnable()
    {
        InteractionEvents.OnIngredientPickup += OnIngredientPickup;
        InteractionEvents.OnIngredientDiscard += OnIngredientDiscard;
    }

    private void OnDisable()
    {
        InteractionEvents.OnIngredientPickup -= OnIngredientPickup;
        InteractionEvents.OnIngredientDiscard -= OnIngredientDiscard;
    }

    void Start()
    {
        // initialize the inventory list and hotbarslots array
        _inventory = new List<Ingredient>();
        _hotbarSlots = new Image[6];

        // we create the _hotbarSlots array
        for (int i = 0; i < 6; i++)
        {
            _hotbarSlots[i] = _hotbar.transform.GetChild(i).GetComponent<Image>();
        }

        // Initial update
        UpdateHotbar();
    }


    void Update()
    {
        // Detect mouse wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) // wheel up
        {
            SelectNextSlot();
        }
        else if (scroll < 0f) // wheel down
        {
            SelectPreviousSlot();
        }
    }

    private void OnIngredientPickup(Ingredient ingredient)
    {
        // we need to populate the _inventory of the Ingredient picked up
        // fristly, we check if the _inventory is full
        if (ingredient != null)
        {
            if (_inventory.Count < 6)
            {
                _inventory.Add(ingredient);
            }

            UpdateHotbar();
        }
    }

    private void OnIngredientDiscard()
    {
        if (_inventory.Count > 0 && activeSlotIndex < _inventory.Count)
        {
            _inventory.RemoveAt(activeSlotIndex);
            UpdateHotbar();
        }
    }

    private void UpdateHotbar()
    {
        // Clear hotbar first
        for (int i = 0; i < _hotbarSlots.Length; i++)
        {
            _hotbarSlots[i].sprite = null;
            _hotbarSlots[i].enabled = false;
        }

        // Fill hotbar with current inventory
        for (int i = 0; i < _inventory.Count && i < _hotbarSlots.Length; i++)
        {
            _hotbarSlots[i].sprite = _inventory[i].icon;
            _hotbarSlots[i].enabled = true;
        }

        UpdateSlotColors();
    }

    private void UpdateSlotColors()
    {
        for (int i = 0; i < _hotbarSlots.Length; i++)
        {
            if (i == activeSlotIndex)
            {
                // Active slot → red
                _hotbarSlots[i].color = Color.red;
            }
            else
            {
                // Inactive slot → white (or default alpha)
                Color c = _hotbarSlots[i].color;
                c.r = 1f;
                c.g = 1f;
                c.b = 1f;
                _hotbarSlots[i].color = c;
            }
        }
    }

    private void SelectNextSlot()
    {
        activeSlotIndex++;
        if (activeSlotIndex >= _hotbarSlots.Length)
            activeSlotIndex = 0; // wrap around
        UpdateSlotColors();
    }

    private void SelectPreviousSlot()
    {
        activeSlotIndex--;
        if (activeSlotIndex < 0)
            activeSlotIndex = _hotbarSlots.Length - 1; // wrap around
        UpdateSlotColors();
    }
}
