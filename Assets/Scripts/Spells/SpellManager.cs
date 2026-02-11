using GameData.Scripts.Items;
using NUnit.Framework;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{
    private List<IGrimoireData> abilityList;
    [SerializeField] private GameObject hotbar;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI name;
    private SpellSlot[] spellSlots;
    private int _activeSlotIndex;
    [SerializeField] private List<string> usableIDs;
    private bool _isActive;

    private int _zoneCounter = 0;

    [SerializeField] private float holdTimeToCast = 1f;
    private float _holdTimer;
    private bool _isHolding;
    [SerializeField] private RectTransform chargeBar;

    private Coroutine _speedCoroutine;
    private GameObject _speedInstance;
    private Coroutine _jumpingCoroutine;
    private GameObject _jumpingInstance;
    private Coroutine _fireBreathCoroutine;
    private GameObject _fireBreathInstance;
    private Coroutine _clarovencyCoroutine;
    private GameObject _clarovencyInstance;
    private Coroutine _invulnerabilityCoroutine;
    private GameObject _invulnerabilityInstance;
    private Coroutine _vitalityCoroutine;
    private GameObject _vitalityInstance;
    private Coroutine _lightCoroutine;
    private GameObject _lightInstance;
    private Coroutine _fireballCoroutine;
    private GameObject _fireballInstance;
    private Coroutine _swiftRetreatCoroutine;
    private GameObject _swiftRetreatInstance;
    private Coroutine _shieldCoroutine;
    private GameObject _shieldInstance;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject fireBreathPrefab;
    [SerializeField] private float fireBreathDuration = 3f;
    [SerializeField] private GameObject clarovencyPrefab;
    [SerializeField] private float clarovencyDuration = 3f;
    [SerializeField] private GameObject invulnerabilityPrefab;
    [SerializeField] private float invulnerabilityDuration = 3f;
    [SerializeField] private GameObject speedPrefab;
    [SerializeField] private float speedDuration = 10f;
    [SerializeField] private GameObject vitalityPrefab;
    [SerializeField] private float vitalityDuration = 3f;
    [SerializeField] private GameObject lightPrefab;
    [SerializeField] private float lightDuration = 3f;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float fireballDuration = 3f;
    [SerializeField] private GameObject swiftRetreatPrefab;
    [SerializeField] private float swiftRetreatDuration = 3f;
    [SerializeField] private GameObject jumpingPrefab;
    [SerializeField] private float jumpingDuration = 10f;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private float shieldDuration = 3f;


    private void OnEnable()
    {
        GrimoireEvents.OnEntryDiscovered += OnEntryDiscovered;
        SpellEvents.OnSpellZoneEnter += OnZoneEnter;
        SpellEvents.OnSpellZoneExit += OnZoneExit;
    }

    private void OnDisable()
    {
        GrimoireEvents.OnEntryDiscovered -= OnEntryDiscovered;
        SpellEvents.OnSpellZoneEnter -= OnZoneEnter;
        SpellEvents.OnSpellZoneExit -= OnZoneExit;
    }
    void Start()
    {
        _isActive = false;
        abilityList = new List<IGrimoireData>();
        spellSlots = new SpellSlot[10];

        for (int i = 0; i < 10; i++)
        {
            spellSlots[i] = hotbar.transform.GetChild(i).GetComponent<SpellSlot>();
            spellSlots[i].index = i;
        }

        // Initial update
        UpdateHotbar();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive) { return; }

        if (abilityList.Count == 0) return;

        if (_activeSlotIndex < 0) _activeSlotIndex = 0;
        // Detect mouse wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) // wheel up
        {
            SelectNextSlot();
        }
        else if (scroll < 0f) // wheel down
        {
            SelectPreviousSlot();
        }

        // gestire l'interazione col tastro destro
        if (Input.GetMouseButtonDown(1))
        {
            _isHolding = true;
            _holdTimer = 0f;
            chargeBar.localScale = new Vector3(0f,1f,1f);
            chargeBar.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(1) && _isHolding)
        {
            _holdTimer += Time.deltaTime;
            chargeBar.localScale = new Vector3(Mathf.Clamp01(_holdTimer / holdTimeToCast), 1f, 1f);

            if (_holdTimer >= holdTimeToCast)
            {
                UseAbility(abilityList[_activeSlotIndex]);
                _isHolding = false;
                chargeBar.gameObject.SetActive(false);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            _isHolding = false;
            _holdTimer = 0f;
            chargeBar.gameObject.SetActive(false);
        }

        name.text = (abilityList.Count == 0 ? "" : abilityList[_activeSlotIndex].DisplayName);
    }

    // Ability functions
    private void UseAbility(IGrimoireData data)
    {
        switch (data.Id)
        {
            case "firebreath_potion":
                {
                    UseFirebreathPotion();
                    break;
                }
            case "clarovegency_potion":
                {
                    UseClarovencyPotion();
                    break;
                }
            case "invulnerability_potion":
                {
                    UseInvulnerabilityPotion();
                    break;
                }
            case "speed_potion":
                {
                    UseSpeedPotion();
                    break;
                }
            case "vitality_potion":
                {
                    UseVitalityPotion();
                    break;
                }
            case "light":
                {
                    UseLight();
                    break;
                }
            case "fireball":
                {
                    UseFireball();
                    break;
                }
            case "swift_retreat":
                {
                    UseSwiftRetreat();
                    break;
                }
            case "jumping":
                {
                    UseJumping();
                    break;
                }
            case "shield":
                {
                    UseShield();
                    break;
                }
        }
    }

    // FIREBREATH POTION
    private void UseFirebreathPotion()
    {
        if (_fireBreathCoroutine != null)
            return; // already active, do not restart

        _fireBreathCoroutine = StartCoroutine(FireBreathRoutine());
    }
    private IEnumerator FireBreathRoutine()
    {
        // Instantiate if not existing
        if (_fireBreathInstance == null)
        {
            _fireBreathInstance = Instantiate(
                fireBreathPrefab,
                player.transform.position + new Vector3(0, 1, 0),
                player.transform.rotation * Quaternion.Euler(new Vector3(90, 0, 0)),
                player.transform //follows player
            );
        }

        var ps = _fireBreathInstance.GetComponent<ParticleSystem>();

        ps.Play();
        SpellEvents.OnFirebreathUsed?.Invoke();

        yield return new WaitForSeconds(fireBreathDuration);

        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        _fireBreathCoroutine = null;
        SpellEvents.OnFirebreathEnded?.Invoke();
    }
    // CLAROVENCY POTION
    private void UseClarovencyPotion()
    {
        if (_clarovencyCoroutine != null)
            return; // already active, do not restart

        _clarovencyCoroutine = StartCoroutine(ClarovencyRoutine());
    }
    private IEnumerator ClarovencyRoutine()
    {
        // Instantiate if not existing
        if (_clarovencyInstance == null)
        {
            _clarovencyInstance = Instantiate(
                clarovencyPrefab,
                player.transform.position + new Vector3(0, 1, 0),
                player.transform.rotation * Quaternion.Euler(new Vector3(90, 0, 0)),
                player.transform //follows player
            );
        }

        var ps = _clarovencyInstance.GetComponent<ParticleSystem>();

        ps.Play();
        SpellEvents.OnClarovencyUsed?.Invoke();

        yield return new WaitForSeconds(clarovencyDuration);

        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        _clarovencyCoroutine = null;
        SpellEvents.OnClarovencyEnded?.Invoke();
    }
    // INVULNERABILITY POTION
    private void UseInvulnerabilityPotion()
    {
        if (_invulnerabilityCoroutine != null)
            return; // already active, do not restart

        _invulnerabilityCoroutine = StartCoroutine(InvulnerabilityRoutine());
    }
    private IEnumerator InvulnerabilityRoutine()
    {
        // Instantiate if not existing
        if (_invulnerabilityInstance == null)
        {
            _invulnerabilityInstance = Instantiate(
                invulnerabilityPrefab,
                player.transform.position + new Vector3(0, 1, 0),
                player.transform.rotation * Quaternion.Euler(new Vector3(90, 0, 0)),
                player.transform //follows player
            );
        }

        var ps = _invulnerabilityInstance.GetComponent<ParticleSystem>();

        ps.Play();
        SpellEvents.OnInvulnerabilityUsed?.Invoke();

        yield return new WaitForSeconds(invulnerabilityDuration);

        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        _invulnerabilityCoroutine = null;
        SpellEvents.OnInvulnerabilityEnded?.Invoke();
    }
    // SPEED POTION
    private void UseSpeedPotion()
    {
        if (_speedCoroutine != null)
            return; // already active, do not restart

        _speedCoroutine = StartCoroutine(SpeedRoutine());
    }
    private IEnumerator SpeedRoutine()
    {
        // Instantiate if not existing
        if (_speedInstance == null)
        {
            _speedInstance = Instantiate(
                speedPrefab,
                player.transform.position + new Vector3(0, 1, 0),
                player.transform.rotation * Quaternion.Euler(new Vector3(90, 0, 0)),
                player.transform //follows player
            );
        }

        var ps = _speedInstance.GetComponent<ParticleSystem>();
        var playerdata = player.GetComponent<FirstPersonController>();

        ps.Play();
        float originalSpeed = playerdata.MoveSpeed;
        playerdata.MoveSpeed = 10f;
        SpellEvents.OnSpeedUsed?.Invoke();

        yield return new WaitForSeconds(speedDuration);

        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        playerdata.MoveSpeed = originalSpeed;

        _speedCoroutine = null;
        SpellEvents.OnSpeedEnded?.Invoke();
    }

    // VITALITY POTION
    private void UseVitalityPotion()
    {
        if (_vitalityCoroutine != null)
            return; // already active, do not restart

        _vitalityCoroutine = StartCoroutine(VitalityRoutine());
    }
    private IEnumerator VitalityRoutine()
    {
        // Instantiate if not existing
        if (_vitalityInstance == null)
        {
            _vitalityInstance = Instantiate(
                vitalityPrefab,
                player.transform.position + new Vector3(0, 1, 0),
                player.transform.rotation * Quaternion.Euler(new Vector3(90, 0, 0)),
                player.transform //follows player
            );
        }

        var ps = _vitalityInstance.GetComponent<ParticleSystem>();

        ps.Play();
        SpellEvents.OnVitalityUsed?.Invoke();

        yield return new WaitForSeconds(vitalityDuration);

        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        _vitalityCoroutine = null;
        SpellEvents.OnVitalityEnded?.Invoke();
    }
    // LIGHT SPELL
    private void UseLight()
    {
        Debug.Log("Light Spell used");
    }
    // FIREBALL SPELL
    private void UseFireball()
    {
        Debug.Log("Fireball Spell used");
    }
    // SWIFT RETREAT SPELL
    private void UseSwiftRetreat()
    {
        Debug.Log("Swift Retreat Spell used");
    }
    // JUMPING SPELL
    private void UseJumping()
    {
        if (_jumpingCoroutine != null)
            return; // already active, do not restart

        _jumpingCoroutine = StartCoroutine(JumpingRoutine());
    }
    private IEnumerator JumpingRoutine()
    {
        // Instantiate if not existing
        if (_jumpingInstance == null)
        {
            _jumpingInstance = Instantiate(
                jumpingPrefab,
                player.transform.position + new Vector3(0, 1, 0),
                player.transform.rotation * Quaternion.Euler(new Vector3(90, 0, 0)),
                player.transform //follows player
            );
        }

        var ps = _jumpingInstance.GetComponent<ParticleSystem>();
        var playerdata = player.GetComponent<FirstPersonController>();

        ps.Play();
        float originalJump = playerdata.JumpHeight;
        playerdata.JumpHeight = 3f;
        SpellEvents.OnJumpingUsed?.Invoke();

        yield return new WaitForSeconds(jumpingDuration);

        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        playerdata.JumpHeight = originalJump;

        _jumpingCoroutine = null;
        SpellEvents.OnJumpingEnded?.Invoke();
    }
    // SHIELD SPELL
    private void UseShield()
    {
        if (_shieldCoroutine != null)
            return; // already active, do not restart

        _shieldCoroutine = StartCoroutine(ShieldRoutine());
    }
    private IEnumerator ShieldRoutine()
    {
        // Instantiate if not existing
        if (_shieldInstance == null)
        {
            _shieldInstance = Instantiate(
                shieldPrefab,
                player.transform.position + new Vector3(0, 1, 0),
                player.transform.rotation * Quaternion.Euler(new Vector3(90, 0, 0)),
                player.transform //follows player
            );
        }

        var ps = _shieldInstance.GetComponent<ParticleSystem>();

        ps.Play();
        SpellEvents.OnShieldUsed?.Invoke();

        yield return new WaitForSeconds(shieldDuration);

        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        _shieldCoroutine = null;
        SpellEvents.OnShieldEnded?.Invoke();
    }


    // Helper methods
    private void UpdateHotbar()
    {
        for (int i = 0; i < spellSlots.Length; i++)
        {
            if (i < abilityList.Count)
            {
                spellSlots[i].SetAbility(abilityList[i]);
            }
            else
            {
                spellSlots[i].Clear();
            }
        }

        UpdateSlotColors();
    }

    private void UpdateSlotColors()
    {
        for (var i = 0; i < spellSlots.Length; i++)
        {
            if (i == _activeSlotIndex)
            {
                // Active slot → red
                spellSlots[i].GetComponent<Image>().color = Color.red;
            }
            else
            {
                // Inactive slot → white (or default alpha)
                Color c = spellSlots[i].GetComponent<Image>().color;
                c.r = 1f;
                c.g = 1f;
                c.b = 1f;
                spellSlots[i].GetComponent<Image>().color = c;
            }
        }
    }
    private void SelectNextSlot()
    {
        _activeSlotIndex++;
        if (_activeSlotIndex >= abilityList.Count)
            _activeSlotIndex = 0; // wrap around
        UpdateSlotColors();
    }

    private void SelectPreviousSlot()
    {
        _activeSlotIndex--;
        if (_activeSlotIndex < 0)
            _activeSlotIndex = abilityList.Count - 1; // wrap around
        UpdateSlotColors();
    }

    // Events handler
    private void OnEntryDiscovered(IGrimoireData data)
    {
        if (!usableIDs.Contains(data.Id))
            return;

        abilityList.Add(data);
        UpdateHotbar();
    }

    private void OnZoneEnter()
    {
        _zoneCounter++;

        if (_zoneCounter == 1)
            SetSpellMode(true);
    }

    private void OnZoneExit()
    {
        _zoneCounter--;

        if (_zoneCounter <= 0)
        {
            _zoneCounter = 0;
            SetSpellMode(false);
        }
    }

    private void SetSpellMode(bool active)
    {
        _isActive = active;
        canvas.enabled = active;
    }
}
