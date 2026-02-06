using UnityEngine;

public class SpellZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpellEvents.OnSpellZoneTrigger?.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpellEvents.OnSpellZoneTrigger?.Invoke(false);
        }    
    }
}
