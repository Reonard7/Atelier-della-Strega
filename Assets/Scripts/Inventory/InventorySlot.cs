using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        [Header("Componente UI")]
        public Image targetImage;

        [Header("Nuova Icona")]
        public Sprite selectedIcon;
        public Sprite unselectedIcon;

        public void ChangeSelectedBackground()
        {
            if (!targetImage) return;
            if (!selectedIcon) return;

            targetImage.sprite = selectedIcon;
        }

        public void ChangeUnselectedBackground()
        {
            if (!targetImage) return;
            if (!selectedIcon) return;

            targetImage.sprite = unselectedIcon;
        }
    }
}
