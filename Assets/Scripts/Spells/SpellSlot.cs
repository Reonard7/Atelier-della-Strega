using UnityEngine;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour
{
    public int index;               // slot index
    public IGrimoireData ability;   // ability in this slot
    public Image icon;

    private void EnsureIcon()
    {
        if (icon == null)
            icon = GetComponent<Image>();
    }

    public void SetAbility(IGrimoireData newAbility)
    {
        EnsureIcon();

        if (newAbility != null)
        {
            icon.sprite = newAbility.Icon;
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
        SetAbility(null);
    }
}
