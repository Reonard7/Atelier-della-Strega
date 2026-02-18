using System.Collections.Generic;
using Events;
using GameData.Scripts.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [FormerlySerializedAs("_hotbar")] [SerializeField] private GameObject hotbar;
        [FormerlySerializedAs("_background")] [SerializeField] private GameObject background;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip pickupClip;
        [SerializeField] private Canvas canvas;
        [SerializeField] private new TextMeshProUGUI name;
        private List<Ingredient> _inventory;
        private HotbarSlot[] _hotbarSlots;
        private InventorySlot[] _inventorySlots;
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
            InteractionEvents.OnIngredientLocked += OnIngredientLocked;
            InteractionEvents.OnIngredientUnlocked += OnIngredientUnlocked;
            SpellEvents.OnSpellZoneTrigger += OnSpellZoneTrigger;
        }

        private void OnDisable()
        {
            InteractionEvents.OnCauldronInteracted -= OnCauldronInteracted;
            InteractionEvents.OnCauldronExit -= OnCauldronExit;
            InteractionEvents.OnIngredientPickup -= OnIngredientPickup;
            InteractionEvents.OnIngredientDiscard -= OnIngredientDiscard;
            InteractionEvents.OnIngredientLocked -= OnIngredientLocked;
            InteractionEvents.OnIngredientUnlocked -= OnIngredientUnlocked;
            SpellEvents.OnSpellZoneTrigger -= OnSpellZoneTrigger;
        }

        private void Start()
        {
            _inCrafting = false;
            _isActive = true;

            // initialize the inventory list and hotbar slots array
            _inventory = new List<Ingredient>();
            _hotbarSlots = new HotbarSlot[3];
            _inventorySlots = new InventorySlot[3];

            for (int i = 0; i < 3; i++)
            {
                _hotbarSlots[i] = hotbar.transform.GetChild(i).GetComponent<HotbarSlot>();
                _hotbarSlots[i].index = i;
            }
            
            for (int i = 0; i < 3; i++)
            {
                _inventorySlots[i] = background.transform.GetChild(i).GetComponent<InventorySlot>();
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

            name.text = (_inventory.Count == 0 ? "" : _inventory[_activeSlotIndex].displayName);
        }

        private void OnIngredientPickup(Ingredient ingredient)
        {
            // we need to populate the _inventory of the Ingredient picked up
            // firstly, we check if the _inventory is full
            if (ingredient is null) return;
            if (_inventory.Count < 3)
            {
                _inventory.Add(ingredient);
                audioSource.PlayOneShot(pickupClip);
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
            audioSource.PlayOneShot(pickupClip);
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
            // active = outside main room
            SetInventoryMode(!active);
        }

        private void SetInventoryMode(bool active)
        {
            _isActive = active;
            canvas.enabled = active;
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
                    _inventorySlots[i].ChangeSelectedBackground();
                }
                else
                {
                    _inventorySlots[i].ChangeUnselectedBackground();
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
