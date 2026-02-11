using UnityEngine;
using GameData.Scripts.Items;
using Events;
using Interaction;

public class SpellInteractable : MonoBehaviour, IInteractable
{
    public Spell spell;
    public AudioClip pickupSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact(GameObject caller)
    {
        audioSource.PlayOneShot(pickupSound);
        InteractionEvents.OnSpellPickup?.Invoke(spell);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
