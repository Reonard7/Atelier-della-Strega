using UnityEngine;

public class HubZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpellEvents.OnSpellZoneTrigger?.Invoke(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpellEvents.OnSpellZoneTrigger?.Invoke(true);
        }
    }
}
