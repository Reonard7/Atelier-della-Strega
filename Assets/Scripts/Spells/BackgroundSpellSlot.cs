using UnityEngine;
using UnityEngine.UI;

namespace Spells
{
    public class BackgroundSpellSlot : MonoBehaviour
    {
        [Header("Componente UI")]
        public Image targetImage;

        [Header("Icone")]
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
