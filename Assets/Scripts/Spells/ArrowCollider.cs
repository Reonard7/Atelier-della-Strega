using UnityEngine;

public class ArrowCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpellEvents.OnArrowCollision?.Invoke();
        }
    }
}
