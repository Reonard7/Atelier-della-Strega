using GameData.Scripts.Items;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{
    private List<IGrimoireData> abilityList;
    [SerializeField] private GameObject hotbar;
    private SpellSlot[] spellSlots;
    private int _activeSlotIndex;
    [SerializeField] private List<string> usableIDs;
    private bool _isActive;

    [SerializeField] private float holdTimeToCast = 1f;
    private float _holdTimer;
    private bool _isHolding;
    [SerializeField] private RectTransform chargeBar;


    private void OnEnable()
    {
        GrimoireEvents.OnEntryDiscovered += OnEntryDiscovered;
        SpellEvents.OnSpellZoneTrigger += OnSpellZoneTrigger;
    }

    private void OnDisable()
    {
        GrimoireEvents.OnEntryDiscovered -= OnEntryDiscovered;
        SpellEvents.OnSpellZoneTrigger -= OnSpellZoneTrigger;
    }
    void Start()
    {
        _isActive = false;
        abilityList = new List<IGrimoireData>();
        spellSlots = new SpellSlot[10];

        for (int i = 0; i < 10; i++)
        {
            spellSlots[i] = hotbar.transform.GetChild(i).GetComponent<SpellSlot>();
            spellSlots[i].index = i;
        }

        // Initial update
        UpdateHotbar();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive) { return; }

        if (abilityList.Count == 0) return;

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

        // gestire l'interazione col tastro sinistro
        if (Input.GetMouseButtonDown(0))
        {
            _isHolding = true;
            _holdTimer = 0f;
            chargeBar.localScale = new Vector3(0f,1f,1f);
            chargeBar.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(0) && _isHolding)
        {
            _holdTimer += Time.deltaTime;
            chargeBar.localScale = new Vector3(Mathf.Clamp01(_holdTimer / holdTimeToCast), 1f, 1f);

            if (_holdTimer >= holdTimeToCast)
            {
                UseAbility(abilityList[_activeSlotIndex]);
                _isHolding = false;
                chargeBar.gameObject.SetActive(false);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isHolding = false;
            _holdTimer = 0f;
            chargeBar.gameObject.SetActive(false);
        }
    }

    // Ability functions
    private void UseAbility(IGrimoireData data)
    {
        switch (data.Id)
        {
            case "firebreath_potion":
                {
                    UseFirebreathPotion();
                    break;
                }
            case "clarovegency_potion":
                {
                    UseClarovencyPotion();
                    break;
                }
            case "invulnerability_potion":
                {
                    UseInvulnerabilityPotion();
                    break;
                }
            case "speed_potion":
                {
                    UseSpeedPotion();
                    break;
                }
            case "vitality_potion":
                {
                    UseVitalityPotion();
                    break;
                }
            case "light":
                {
                    UseLight();
                    break;
                }
            case "fireball":
                {
                    UseFireball();
                    break;
                }
            case "swift_retreat":
                {
                    UseSwiftRetreat();
                    break;
                }
            case "jumping":
                {
                    UseJumping();
                    break;
                }
            case "shield":
                {
                    UseShield();
                    break;
                }
        }
    }

    private void UseFirebreathPotion()
    {
        Debug.Log("Firebreath Potion used");
    }
    private void UseClarovencyPotion()
    {
        Debug.Log("Calrovency Potion used");
    }
    private void UseInvulnerabilityPotion()
    {
        Debug.Log("Invulnerability Potion used");
    }
    private void UseSpeedPotion()
    {
        Debug.Log("Speed Potion used");
    }
    private void UseVitalityPotion()
    {
        Debug.Log("Vitality Potion used");
    }
    private void UseLight()
    {
        Debug.Log("Light Spell used");
    }
    private void UseFireball()
    {
        Debug.Log("Fireball Spell used");
    }
    private void UseSwiftRetreat()
    {
        Debug.Log("Swift Retreat Spell used");
    }
    private void UseJumping()
    {
        Debug.Log("Jumping Spell used");
    }
    private void UseShield()
    {
        Debug.Log("Shield Spell used");
    }

    // Helper methods
    private void UpdateHotbar()
    {
        for (int i = 0; i < spellSlots.Length; i++)
        {
            if (i < abilityList.Count)
            {
                spellSlots[i].SetAbility(abilityList[i]);
            }
            else
            {
                spellSlots[i].Clear();
            }
        }

        UpdateSlotColors();
    }

    private void UpdateSlotColors()
    {
        for (var i = 0; i < spellSlots.Length; i++)
        {
            if (i == _activeSlotIndex)
            {
                // Active slot → red
                spellSlots[i].GetComponent<Image>().color = Color.red;
            }
            else
            {
                // Inactive slot → white (or default alpha)
                Color c = spellSlots[i].GetComponent<Image>().color;
                c.r = 1f;
                c.g = 1f;
                c.b = 1f;
                spellSlots[i].GetComponent<Image>().color = c;
            }
        }
    }
    private void SelectNextSlot()
    {
        _activeSlotIndex++;
        if (_activeSlotIndex >= abilityList.Count)
            _activeSlotIndex = 0; // wrap around
        UpdateSlotColors();
    }

    private void SelectPreviousSlot()
    {
        _activeSlotIndex--;
        if (_activeSlotIndex < 0)
            _activeSlotIndex = abilityList.Count - 1; // wrap around
        UpdateSlotColors();
    }

    // Events handler
    private void OnEntryDiscovered(IGrimoireData data)
    {
        if (!usableIDs.Contains(data.Id))
            return;

        abilityList.Add(data);
        UpdateHotbar();
    }

    private void OnSpellZoneTrigger(bool active)
    {
        _isActive = active;
        hotbar.SetActive(active);   // show/hide UI
    }
}
