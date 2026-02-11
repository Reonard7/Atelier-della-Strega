using UnityEngine;

public class openTreasure : MonoBehaviour
{
    public void Interact(GameObject caller)
    {
        Debug.Log(this.tag);
        if (this.CompareTag("Treasure"))
        {
            SpellEvents.OnTreasureInteracted?.Invoke();
        }
        else
        {
            SpellEvents.OnMimicInteracted?.Invoke();
        }
    }

    [SerializeField] private AudioSource audioSource;

    public void PlayOpenSound()
    {
        audioSource.Play();
    }
}
