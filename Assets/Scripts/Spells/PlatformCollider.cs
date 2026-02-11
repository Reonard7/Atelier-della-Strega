using UnityEngine;

public class PlatformCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpellEvents.OnPlatformCollision?.Invoke();
        }
    }
}
