using UnityEngine;
using System.Collections.Generic;
using System;

public class GrimoireUIController : MonoBehaviour
{
    public enum GrimoireCategory
    {
        Ingredients,
        Potions,
        Spells
    }

    [Header("UI")]
    [SerializeField] private GrimoirePageUI leftPage;
    [SerializeField] private GrimoirePageUI rightPage;
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;

    [Header("References")]
    [SerializeField] private GrimoireManager grimoireManager;

    private GrimoireCategory currentCategory = GrimoireCategory.Ingredients;
    public int currentIndex = 0;
    public int maxIndex;

    private void OnEnable()
    {
        RefreshPages();
    }

    // Buttons

    public void ShowIngredients()
    {
        currentCategory = GrimoireCategory.Ingredients;
        currentIndex = 0;
        RefreshPages();
    }

    public void ShowPotions()
    {
        currentCategory = GrimoireCategory.Potions;
        currentIndex = 0;
        RefreshPages();
    }

    public void ShowSpells()
    {
        currentCategory = GrimoireCategory.Spells;
        currentIndex = 0;
        RefreshPages();
    }

    public void NextPage()
    {
        currentIndex += 2;
        if (currentIndex > maxIndex)
            currentIndex = maxIndex;

        RefreshPages();
    }

    public void PreviousPage()
    {
        currentIndex -= 2;
        if (currentIndex < 0)
            currentIndex = 0;

        RefreshPages();
    }

    // Methods

    private void RefreshPages()
    {
        switch (currentCategory)
        {
            case GrimoireCategory.Ingredients:
                Display(grimoireManager.IngredientEntries);
                    maxIndex = (int)Math.Round((double)grimoireManager.IngredientEntries.Count / 2) * 2 - 2;
                    break;

            case GrimoireCategory.Potions:
                Display(grimoireManager.PotionEntries);
                maxIndex = (int)Math.Round((decimal)(grimoireManager.PotionEntries.Count - 2) / 2) * 2;
                break;

            case GrimoireCategory.Spells:
                Display(grimoireManager.SpellEntries);
                maxIndex = (int)Math.Round((decimal)(grimoireManager.SpellEntries.Count - 2) / 2) * 2;
                break;
        }

        if (currentIndex == 0)
            leftArrow.SetActive(false);
        else
            leftArrow.SetActive(true);

        if (currentIndex == maxIndex)
            rightArrow.SetActive(false);
        else
            rightArrow.SetActive(true);
    }

    private void Display<T>(List<GrimoireEntry<T>> entries) where T : IGrimoireData
    {
        leftPage.SetEntry(entries[currentIndex]);
        if (currentIndex + 1 < entries.Count)
        {
            rightPage.SetEntry(entries[currentIndex + 1]);
            rightPage.gameObject.SetActive(true);
        }
        else
        {
            rightPage.gameObject.SetActive(false);
        }
    }
}

