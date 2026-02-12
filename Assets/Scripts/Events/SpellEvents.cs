using System;
using Unity.VisualScripting;
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

    public static Action<int> OnTrialStarted;
    public static Action OnTrialSuspended;
    public static Action OnTrialCompleted;

    public static Action OnArrowCollision;
    public static Action OnMimicInteracted;
    public static Action OnTreasureInteracted;
    public static Action OnPlatformCollision;
}
