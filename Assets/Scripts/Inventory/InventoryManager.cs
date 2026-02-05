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
        private HotbarSlot[] _hotbarSlots;
        private int _activeSlotIndex;
        private bool _inCrafting;
        private bool _isActive;

        private void OnEnable()
        {
            InteractionEvents.OnCauldronInteracted += OnCauldronInteracted;
            InteractionEvents.OnCauldronExit += OnCauldronExit;
            InteractionEvents.OnIngredientPickup += OnIngredientPickup;
            InteractionEvents.OnIngredientDiscard += OnIngredientDiscard;
            SpellEvents.OnSpellZoneTrigger += OnSpellZoneTrigger;
        }

        private void OnDisable()
        {
            InteractionEvents.OnCauldronInteracted -= OnCauldronInteracted;
            InteractionEvents.OnCauldronExit -= OnCauldronExit;
            InteractionEvents.OnIngredientPickup -= OnIngredientPickup;
            InteractionEvents.OnIngredientDiscard -= OnIngredientDiscard;
            SpellEvents.OnSpellZoneTrigger -= OnSpellZoneTrigger;
        }

        private void Start()
        {
            _inCrafting = false;
            _isActive = true;

            // initialize the inventory list and hotbar slots array
            _inventory = new List<Ingredient>();
            _hotbarSlots = new HotbarSlot[6];

            for (int i = 0; i < 6; i++)
            {
                _hotbarSlots[i] = hotbar.transform.GetChild(i).GetComponent<HotbarSlot>();
                _hotbarSlots[i].index = i;
            }

            // Initial update
            UpdateHotbar();
        }


        private void Update()
        {
            if (!_isActive) { return; }

            if (!_inCrafting)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    InteractionEvents.OnIngredientDiscard?.Invoke();
                }

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
            if (!_inCrafting)
            {
                if (_inventory.Count <= 0 || _activeSlotIndex >= _inventory.Count) return;
                _inventory.RemoveAt(_activeSlotIndex);
                if (_activeSlotIndex == _inventory.Count) SelectPreviousSlot();
                UpdateHotbar();
            }
        }

        private void OnCauldronInteracted()
        {
            _inCrafting = true;
        }

        private void OnCauldronExit()
        {
            _inCrafting = false;
        }

        private void OnSpellZoneTrigger(bool active)
        {
            _isActive = !active;
            hotbar.SetActive(_isActive);
        }

        private void UpdateHotbar()
        {
            for (int i = 0; i < _hotbarSlots.Length; i++)
            {
                if (i < _inventory.Count)
                {
                    _hotbarSlots[i].SetIngredient(_inventory[i]);
                }
                else
                {
                    _hotbarSlots[i].Clear();
                }
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
                    _hotbarSlots[i].GetComponent<Image>().color = Color.red;
                }
                else
                {
                    // Inactive slot → white (or default alpha)
                    Color c = _hotbarSlots[i].GetComponent<Image>().color;
                    c.r = 1f;
                    c.g = 1f;
                    c.b = 1f;
                    _hotbarSlots[i].GetComponent<Image>().color = c;
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
