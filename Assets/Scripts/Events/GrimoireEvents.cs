using System;
using UnityEngine;

public class GrimoireEvents : MonoBehaviour
{
    public static Action<IGrimoireData> OnEntryDiscovered;
    public static Action OnRetreatDiscovered;
}
