using StarterAssets;
using UnityEngine;
using Cinemachine;
using System.Collections;
using Events;

namespace Interaction
{
    public class CauldronInteractable : MonoBehaviour, IInteractable
    {
        public void Interact(GameObject caller)
        {
            InteractionEvents.OnCauldronInteracted?.Invoke();
        }
    }
}
