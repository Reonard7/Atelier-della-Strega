using UnityEngine;
using System.Collections;

public class MageController : MonoBehaviour
{
    [Header("Teleport Points")]
    [SerializeField] private Transform downstairsPoint;

    [Header("VFX")]
    [SerializeField] private GameObject teleportVFX;

    [Header("Audio")]
    [SerializeField] private AudioClip teleportSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Timing")]
    [SerializeField] private float teleportDelay = 1f;

    // =========================
    // TELEPORT INIZIALE
    // =========================
    public void TeleportDownstairs()
    {
        if (downstairsPoint == null) return;
        StartCoroutine(TeleportRoutine(downstairsPoint.position));
    }

    // =========================
    // TELEPORT GENERICO
    // =========================
    public void TeleportTo(Transform targetPoint)
    {
        if (targetPoint == null) return;
        StartCoroutine(TeleportRoutine(targetPoint.position));
    }

    private IEnumerator TeleportRoutine(Vector3 targetPosition)
    {
        // Sparizione
        SpawnVFX(transform.position);
        PlaySound();

        yield return new WaitForSeconds(teleportDelay);

        transform.position = targetPosition;

        // Apparizione
        SpawnVFX(transform.position);
        PlaySound();

        Debug.Log("Maga teletrasportata");
    }

    private void SpawnVFX(Vector3 position)
    {
        if (teleportVFX == null) return;
        Instantiate(teleportVFX, position, Quaternion.identity);
    }

    private void PlaySound()
    {
        if (teleportSound == null) return;

        if (audioSource != null)
            audioSource.PlayOneShot(teleportSound);
        else
            AudioSource.PlayClipAtPoint(teleportSound, transform.position);
    }
}
