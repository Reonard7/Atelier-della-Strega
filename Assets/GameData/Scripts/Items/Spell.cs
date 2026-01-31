using UnityEngine;

namespace GameData.Scripts.Items
{
    [CreateAssetMenu(fileName = "Spell", menuName = "GameData/Spell")]
    public class Spell : ScriptableObject
    {
        [Header("General")]
        public string id;
        public string displayName;

        [TextArea(5, 5)]
        public string description;

        public Sprite icon;
        public GameObject prefab;
    }
}

