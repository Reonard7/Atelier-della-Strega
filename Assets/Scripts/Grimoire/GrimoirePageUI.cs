using GameData.Scripts.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GrimoirePageUI : MonoBehaviour
{
    [SerializeField] public Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    public void SetEntry<T>(GrimoireEntry<T> entry) where T : IGrimoireData
    {
        if (entry == null)
        {
            icon.enabled = false;
            nameText.text = "";
            descriptionText.text = "";
            return;
        }

        if (!entry.discovered)
        {
            icon.enabled = false;
            nameText.text = "???";

            descriptionText.text = BuildUndiscoveredText(entry);
            return;
        }

        icon.enabled = true;
        icon.sprite = entry.data.Icon;
        nameText.text = entry.data.DisplayName;
        descriptionText.text = entry.data.Description;
    }

    private string BuildUndiscoveredText<T>(GrimoireEntry<T> entry) where T : IGrimoireData
    {
        string text = "Not yet discovered\n\n";

        // Only potions have ingredients
        if (entry.data is Potion potion)
        {
            text += "Ingredients:\n";

            foreach (var ingredient in potion.potionRecipe)
            {
                text += "- " + ingredient.displayName + "\n";
            }
        }

        return text;
    }
}