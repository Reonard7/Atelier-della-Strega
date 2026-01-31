using UnityEngine;

[System.Serializable]
public class GrimoireEntry<T> where T : ScriptableObject
{
    public T data;
    public bool discovered;
}
