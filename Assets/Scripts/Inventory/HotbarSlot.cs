using GameData.Scripts.Items;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public int index;               // slot index
    public Ingredient ingredient;   // ingredient in this slot
    public Image icon;

    private void Awake()
    {
        icon = GetComponent<Image>();
    }

    public void SetIngredient(Ingredient newIngredient)
    {
        ingredient = newIngredient;

        if (ingredient != null)
        {
            icon.sprite = ingredient.icon;
            icon.enabled = true;
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }

    public void Clear()
    {
        SetIngredient(null);
    }
}
