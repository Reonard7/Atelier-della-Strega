using UnityEngine;
using System.Collections;

public class TutorialNPC : MonoBehaviour
{
    public bool HasReachedDestination { get; private set; }

    [Header("Teleport FX")]
    public GameObject teleportEffectPrefab; // optional: particelle da spawnare
    public float teleportDelay = 0.5f;

    public void TeleportTo(Vector3 target)
    {
        StartCoroutine(TeleportRoutine(target));
    }

    private IEnumerator TeleportRoutine(Vector3 target)
    {
        HasReachedDestination = false;

        // Optional: effetto sparizione
        if (teleportEffectPrefab != null)
            Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);

        // NPC sparisce
        gameObject.SetActive(false);

        // Attendi un breve delay
        yield return new WaitForSeconds(teleportDelay);

        // NPC ricompare nel punto target
        transform.position = target;

        // Optional: effetto ricomparsa
        if (teleportEffectPrefab != null)
            Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);

        gameObject.SetActive(true);

        HasReachedDestination = true;
    }
}
