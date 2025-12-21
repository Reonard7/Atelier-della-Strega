using UnityEngine;

namespace GameData.Scripts.Items
{
    [CreateAssetMenu(fileName = "Ingredient", menuName = "GameData/Ingredient")]
    public class Ingredient : ScriptableObject
    {
        [Header("General")]
        public string id;
        public string displayName;

        [TextArea(5, 5)]
        public string description;

        public Sprite icon;
        public GameObject prefab;

        [Header("Ingredient")]
        public Potion[] craftablePotions;
    }
}
