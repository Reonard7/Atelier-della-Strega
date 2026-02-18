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
    StartCoroutine(TeleportRoutine(downstairsPoint));
}


    // =========================
    // TELEPORT GENERICO
    // =========================
   public void TeleportTo(Transform targetPoint)
{
    if (targetPoint == null) return;
    StartCoroutine(TeleportRoutine(targetPoint));
}


   private IEnumerator TeleportRoutine(Transform targetPoint)
{
    // Sparizione
    SpawnVFX(transform.position);
    PlaySound();

    yield return new WaitForSeconds(teleportDelay);

    // 🔹 Solo rotazione Y del target
    float targetY = targetPoint.rotation.eulerAngles.y;
    Quaternion yOnlyRotation = Quaternion.Euler(0f, targetY, 0f);

    transform.SetPositionAndRotation(targetPoint.position, yOnlyRotation);

    // Apparizione
    SpawnVFX(transform.position);
    PlaySound();

    Debug.Log("Maga teletrasportata con Y rotation del target");
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
