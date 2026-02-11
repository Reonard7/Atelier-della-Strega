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
        if (!entry.discovered)
        {
            icon.enabled = false;
            nameText.text = "???";
            descriptionText.text = "Not yet discovered";
            return;
        }

        if (entry == null)
        {
            icon.enabled = false;
            nameText.text = "";
            descriptionText.text = "";
            return;
        }

        icon.enabled = true;
        icon.sprite = entry.data.Icon;
        nameText.text = entry.data.DisplayName;
        descriptionText.text = entry.data.Description;
    }
}