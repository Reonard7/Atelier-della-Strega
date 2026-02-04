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

    private void OnEnable()
    {
        GrimoireEvents.OnEntryDiscovered += OnEntryDiscovered;
    }

    private void OnDisable()
    {
        GrimoireEvents.OnEntryDiscovered -= OnEntryDiscovered;
    }
    void Start()
    {
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

    private void OnEntryDiscovered(IGrimoireData data)
    {
        if (!usableIDs.Contains(data.Id))
            return;

        abilityList.Add(data);
        UpdateHotbar();
    }
}
