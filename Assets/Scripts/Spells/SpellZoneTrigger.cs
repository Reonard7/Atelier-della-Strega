using UnityEngine;

public class SpellZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SpellEvents.OnSpellZoneEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            SpellEvents.OnSpellZoneExit?.Invoke();
    }
}
