using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider))]
public class SpellWarningTriggerFade : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private TextMeshProUGUI warningText;

    [Header("Settings")]
    [SerializeField] private string message = "You haven't unlocked this spell yet.";

    private void Awake()
    {
        if (warningPanel == null || warningText == null)
        {
            Debug.LogError("Assign Panel and Text!");
            enabled = false;
            return;
        }

        // Inizia nascosto
        warningPanel.SetActive(false);

        // Assicurati che il collider sia Trigger
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            col.isTrigger = true;
            Debug.Log("Collider impostato come Trigger automaticamente");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player entrato nel trigger: {other.name}");
            warningText.text = message;
            warningPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player uscito dal trigger: {other.name}");
            warningPanel.SetActive(false);
        }
    }
    private bool retreatDiscovered = false;

    private void OnEnable()
    {
        GrimoireEvents.OnRetreatDiscovered += OnRetreatDiscovered;
    }

    private void OnRetreatDiscovered()
    {
        this.gameObject.SetActive(false);
    }
}
