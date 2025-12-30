using System;
using GameData.Scripts.Items;
using Interaction;
using UnityEngine;

namespace Events
{
    public static class InteractionEvents
    {
        public static Action<Ingredient> OnIngredientPickup;
        public static Action OnIngredientDiscard;
        public static Action OnCauldronInteracted;
        public static Action OnCauldronExit;
    }
}
