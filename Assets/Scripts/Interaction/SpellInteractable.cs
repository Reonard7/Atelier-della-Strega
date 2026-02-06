using UnityEngine;
using GameData.Scripts.Items;
using Events;
using Interaction;

public class SpellInteractable : MonoBehaviour, IInteractable
{
    public Spell spell;

    public void Interact(GameObject caller)
    {
        InteractionEvents.OnSpellPickup?.Invoke(spell);
    }
}
