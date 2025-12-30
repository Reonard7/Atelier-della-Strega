using System;
using GameData.Scripts.Items;
using Interaction;
using UnityEngine;

namespace Events
{
    public static class AlchemyEvents
    {
        public static Action<Ingredient> OnIngredientDropped;
        public static Action<Ingredient> OnIngredientRemovedFromSlot;
        public static Action OnBrewingStarted;
        public static Action<int> OnDiceCast;
        public static Action<Potion, bool> OnPotionCrafted;
    }
}
