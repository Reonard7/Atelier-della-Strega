using UnityEngine;
using System.Collections;

public class MageController : MonoBehaviour
{
    [SerializeField] private Transform downstairsPoint;

     [Header("VFX")]
    [SerializeField] private GameObject teleportOutVFX;

    [Header("Audio")]
    [SerializeField] private AudioClip teleportSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Timing")]
    [SerializeField] private float teleportDelay = 1f; 

    void Update()
    {
        
    }

    public void TeleportDownstairs()
    {
        StartCoroutine(TeleportWithDelay());
    }

    private IEnumerator TeleportWithDelay()
    {
        // VFX + Audio di sparizione immediati
        SpawnVFX(teleportOutVFX, transform.position);
        PlaySound();

        // Aspetta un secondo (o il tempo che vuoi)
        yield return new WaitForSeconds(teleportDelay);

        // Teletrasporto
        transform.position = downstairsPoint.position;

        Debug.Log("Maga teletrasportata al piano di sotto");
    }

  private void SpawnVFX(GameObject vfxPrefab, Vector3 position)
    {
        if (vfxPrefab == null) return;

        Instantiate(vfxPrefab, position, Quaternion.identity);
    }

    private void PlaySound()
    {
        if (teleportSound == null) return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(teleportSound);
        }
        else
        {
            AudioSource.PlayClipAtPoint(teleportSound, transform.position);
        }
    }
}
