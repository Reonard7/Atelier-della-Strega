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
        [SerializeField] private Canvas canvas;
        private List<Ingredient> _inventory;
        private HotbarSlot[] _hotbarSlots;
        private HashSet<Ingredient> _lockedIngredients = new();
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
            InteractionEvents.OnIngredientLocked += OnIngredientLocked;
            InteractionEvents.OnIngredientUnlocked += OnIngredientUnlocked;
        }

        private void OnDisable()
        {
            InteractionEvents.OnCauldronInteracted -= OnCauldronInteracted;
            InteractionEvents.OnCauldronExit -= OnCauldronExit;
            InteractionEvents.OnIngredientPickup -= OnIngredientPickup;
            InteractionEvents.OnIngredientDiscard -= OnIngredientDiscard;
            SpellEvents.OnSpellZoneTrigger -= OnSpellZoneTrigger;
            InteractionEvents.OnIngredientLocked -= OnIngredientLocked;
            InteractionEvents.OnIngredientUnlocked -= OnIngredientUnlocked;
        }

        private void Start()
        {
            _inCrafting = false;
            _isActive = true;

            // initialize the inventory list and hotbar slots array
            _inventory = new List<Ingredient>();
            _hotbarSlots = new HotbarSlot[3];

            for (int i = 0; i < 3; i++)
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
            if (_inventory.Count < 3)
            {
                _inventory.Add(ingredient);
            }

            UpdateHotbar();
        }

        private void OnIngredientDiscard()
        {
            if (_inventory.Count <= 0 || _activeSlotIndex >= _inventory.Count) return;

            var ingredient = _inventory[_activeSlotIndex];

            if (_lockedIngredients.Contains(ingredient))
                return; // cannot discard ingredient in cauldron

            _inventory.RemoveAt(_activeSlotIndex);
            if (_activeSlotIndex == _inventory.Count) SelectPreviousSlot();
            UpdateHotbar();
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
            canvas.enabled = _isActive;
        }

        private void OnIngredientLocked(Ingredient ingredient)
        {
            _lockedIngredients.Add(ingredient);
        }

        private void OnIngredientUnlocked(Ingredient ingredient)
        {
            _lockedIngredients.Remove(ingredient);
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
