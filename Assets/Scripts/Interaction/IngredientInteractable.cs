using Events;
using GameData.Scripts.Items;
using UnityEngine;

namespace Interaction
{
    public class IngredientInteractable : MonoBehaviour, IInteractable
    {
        public Ingredient ingredient;

        public void Interact(GameObject caller)
        {
            InteractionEvents.OnIngredientPickup?.Invoke(ingredient);
        }
    }
}
