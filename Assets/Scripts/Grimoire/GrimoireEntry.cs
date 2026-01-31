using UnityEngine;

[System.Serializable]
public class GrimoireEntry<T> where T : IGrimoireData
{
    public T data;
    public bool discovered;
}
