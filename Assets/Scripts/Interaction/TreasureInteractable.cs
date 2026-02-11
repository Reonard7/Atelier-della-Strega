using Events;
using GameData.Scripts.Items;
using Interaction;
using UnityEngine;

public class TreasureInteractable : MonoBehaviour, IInteractable
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
}
