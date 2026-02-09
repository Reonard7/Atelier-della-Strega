using TMPro;
using UnityEngine;
using StarterAssets;

public enum TutorialState
{
    IntroDialogue,
    ReachMageDownstairs,
    MageDialogue
}

public class TutorialManager : MonoBehaviour
{
    // =========================
    // UI
    // =========================
    [Header("UI Tutorial")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI tutorialText;

    // =========================
    // PLAYER
    // =========================
    [Header("Player")]
    [SerializeField] private MonoBehaviour[] scriptsToDisable;
    [SerializeField] private FirstPersonController fpsController;

    [Header("Rotation Settings")]
    [SerializeField, Range(1f, 10f)]
    private float rotationSpeed = 5f; // velocità rotazione verso la maga

    // =========================
    // MAGE
    // =========================
    [Header("Mage")]
    [SerializeField] private MageController mage;

    // =========================
    // DIALOGHI
    // =========================
    [Header("Intro Dialogue Lines")]
    [TextArea(2, 4)]
    [SerializeField] private string[] introLines =
    {
        "Ciao.",
        "Ciao."
    };

    [Header("Mage Dialogue Lines")]
    [TextArea(2, 5)]
    [SerializeField] private string[] mageLines =
    {
        "Benvenuto al piano di sotto!",
        "Ti spiegherò come creare le pozioni...",
        "Segui attentamente e vedrai come funziona il calderone!"
    };

    // =========================
    // STATE
    // =========================
    private int _currentLineIndex;
    private TutorialState _currentState;

    // Flag per attivare la rotazione lenta
    private bool _isLookingAtMage = false;

    // =========================
    // UNITY
    // =========================
    private void Start()
    {
        Debug.Log("[TutorialManager] Start scena");
        StartIntro();
    }

    private void Update()
    {
        // Gestione click dialogo
        if (_currentState == TutorialState.IntroDialogue || _currentState == TutorialState.MageDialogue)
        {
            if (Input.GetMouseButtonDown(0))
                NextLine();
        }

        // Rotazione lenta verso la maga
        if (_isLookingAtMage && fpsController != null && mage != null)
        {
            SmoothLookAtMage();
        }
    }

    // =========================
    // INTRO
    // =========================
    private void StartIntro()
    {
        _currentState = TutorialState.IntroDialogue;
        _currentLineIndex = 0;

        tutorialPanel.SetActive(true);
        tutorialText.text = introLines[_currentLineIndex];

        LockPlayer(true);

        Debug.Log("[TutorialManager] Intro avviato");
    }

    private void EndIntro()
    {
        tutorialPanel.SetActive(false);

        if (mage != null)
        {
            mage.TeleportDownstairs();
            Debug.Log("[TutorialManager] Maga teletrasportata");
        }

        LockPlayer(false);

        _currentState = TutorialState.ReachMageDownstairs;
        Debug.Log("[TutorialManager] Stato: ReachMageDownstairs");
    }

    // =========================
    // DIALOGHI
    // =========================
    private void NextLine()
    {
        _currentLineIndex++;

        if (_currentState == TutorialState.IntroDialogue)
        {
            if (_currentLineIndex >= introLines.Length)
            {
                EndIntro();
            }
            else
            {
                tutorialText.text = introLines[_currentLineIndex];
            }
        }
        else if (_currentState == TutorialState.MageDialogue)
        {
            if (_currentLineIndex >= mageLines.Length)
            {
                EndMageDialogue();
            }
            else
            {
                tutorialText.text = mageLines[_currentLineIndex];
            }
        }
    }

    // =========================
    // STEP 2 – PLAYER RAGGIUNGE LA MAGA
    // =========================
    public void OnPlayerReachedMage()
    {
        Debug.Log("[TutorialManager] OnPlayerReachedMage");

        if (_currentState != TutorialState.ReachMageDownstairs)
            return;

        LockPlayer(true);

        // attiva la rotazione lenta verso la maga
        _isLookingAtMage = true;

        // gira la maga verso il player (solo estetica)
        FaceMageToPlayer();

        _currentState = TutorialState.MageDialogue;
        _currentLineIndex = 0;

        tutorialPanel.SetActive(true);
        tutorialText.text = mageLines[_currentLineIndex];

        Debug.Log("[TutorialManager] Dialogo maga avviato");
    }

    private void EndMageDialogue()
    {
        tutorialPanel.SetActive(false);
        LockPlayer(false);

        _isLookingAtMage = false; // disattiva rotazione lenta

        Debug.Log("[TutorialManager] Dialogo maga terminato");

        _currentState = TutorialState.ReachMageDownstairs;
    }

    // =========================
    // ROTAZIONE SMOOTH
    // =========================
    private void SmoothLookAtMage()
    {
        Vector3 targetPos = mage.transform.position + Vector3.up * 1.5f;

        // --- CAPSULE (YAW) ---
        Vector3 flatDir = targetPos - fpsController.transform.position;
        flatDir.y = 0f;
        if (flatDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatDir);
            fpsController.transform.rotation =
                Quaternion.Slerp(fpsController.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // --- CAMERA (PITCH) ---
       Vector3 camDir = targetPos - fpsController.CinemachineCameraTarget.transform.position;
Quaternion camRotation = Quaternion.LookRotation(camDir);

float targetPitch = camRotation.eulerAngles.x;
if (targetPitch > 180f) targetPitch -= 360f;

fpsController.CinemachineTargetPitch = Mathf.Lerp(
    fpsController.CinemachineTargetPitch,
    Mathf.Clamp(targetPitch, fpsController.BottomClamp, fpsController.TopClamp),
    rotationSpeed * Time.deltaTime
);

fpsController.CinemachineCameraTarget.transform.localRotation =
    Quaternion.Euler(fpsController.CinemachineTargetPitch, 0f, 0f);
    }

    // =========================
    // UTILS
    // =========================
    private void LockPlayer(bool lockPlayer)
    {
        foreach (var script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = !lockPlayer;
        }

        Debug.Log("[TutorialManager] LockPlayer: " + lockPlayer);
    }

    private void FaceMageToPlayer()
    {
        if (mage == null || fpsController == null) return;

        Vector3 playerPos = fpsController.transform.position;
        Vector3 dir = playerPos - mage.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.01f)
        {
            mage.transform.forward = dir.normalized;
        }
    }
}

