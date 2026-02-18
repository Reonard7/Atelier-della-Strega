using GameData.Scripts.Items;
using UnityEngine;
using UnityEngine.UI;

/*
 * Rappresenta lo slot nella hotbar.
 * Expected behaviour:
 * - Settare correttamente lo sprite corrispondente trovato dentro lo SO Ingredient
 */
namespace Inventory
{
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

            if (ingredient)
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
}
