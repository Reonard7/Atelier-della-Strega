using System.Collections.Generic;
using Events;
using GameData.Scripts.Items;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [FormerlySerializedAs("_hotbar")] [SerializeField] private GameObject hotbar;
        private List<Ingredient> _inventory;
        private Image[] _hotbarSlots;
        private int _activeSlotIndex;

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

        private void Start()
        {
            // initialize the inventory list and hotbar slots array
            _inventory = new List<Ingredient>();
            _hotbarSlots = new Image[6];

            // we create the _hotbarSlots array
            for (int i = 0; i < 6; i++)
            {
                _hotbarSlots[i] = hotbar.transform.GetChild(i).GetComponent<Image>();
            }

            // Initial update
            UpdateHotbar();
        }


        private void Update()
        {
            if (_activeSlotIndex < 0) _activeSlotIndex = 0;
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
            // firstly, we check if the _inventory is full
            if (ingredient is null) return;
            if (_inventory.Count < 6)
            {
                _inventory.Add(ingredient);
            }

            UpdateHotbar();
        }

        private void OnIngredientDiscard()
        {
            if (_inventory.Count <= 0 || _activeSlotIndex >= _inventory.Count) return;
            _inventory.RemoveAt(_activeSlotIndex);
            if (_activeSlotIndex == _inventory.Count) SelectPreviousSlot();
            UpdateHotbar();
        }

        private void UpdateHotbar()
        {
            // Clear hotbar first
            foreach (var t in _hotbarSlots)
            {
                t.sprite = null;
                t.enabled = false;
            }

            // Fill hotbar with current inventory
            for (var i = 0; i < _inventory.Count && i < _hotbarSlots.Length; i++)
            {
                _hotbarSlots[i].sprite = _inventory[i].icon;
                _hotbarSlots[i].enabled = true;
            }

            UpdateSlotColors();
        }

        private void UpdateSlotColors()
        {
            for (var i = 0; i < _hotbarSlots.Length; i++)
            {
                if (i == _activeSlotIndex)
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
            _activeSlotIndex++;
            if (_activeSlotIndex >= _inventory.Count)
                _activeSlotIndex = 0; // wrap around
            UpdateSlotColors();
        }

        private void SelectPreviousSlot()
        {
            _activeSlotIndex--;
            if (_activeSlotIndex < 0)
                _activeSlotIndex = _inventory.Count - 1; // wrap around
            UpdateSlotColors();
        }
    }
}
