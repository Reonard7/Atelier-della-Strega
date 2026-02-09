using System;
using UnityEngine;

public class SpellEvents : MonoBehaviour
{
    public static Action<bool> OnSpellZoneTrigger;

    public static Action OnFirebreathUsed;
    public static Action OnClarovencyUsed;
    public static Action OnInvulnerabilityUsed;
    public static Action OnSpeedUsed;
    public static Action OnVitalityUsed;
    public static Action OnLightUsed;
    public static Action OnFireballUsed;
    public static Action OnSwiftretreatUsed;
    public static Action OnJumpingUsed;
    public static Action OnShieldUsed;

    public static Action OnFirebreathEnded;
    public static Action OnClarovencyEnded;
    public static Action OnInvulnerabilityEnded;
    public static Action OnSpeedEnded;
    public static Action OnVitalityEnded;
    public static Action OnLightEnded;
    public static Action OnFireballEnded;
    public static Action OnSwiftretreatEnded;
    public static Action OnJumpingEnded;
    public static Action OnShieldEnded;
}
