using TMPro;
using UnityEngine;
using StarterAssets;
using System.Collections;

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
    private float rotationSpeed = 5f;

    // =========================
    // MAGE
    // =========================
    [Header("Mage")]
    [SerializeField] private MageController mage;
    [SerializeField] private float rotationSpeedMage = 120f;
    private Coroutine rotateCoroutine;

    [Header("Final Teleport")]
    [SerializeField] private Transform finalTeleportPoint;

    // =========================
    // DIALOGHI
    // =========================
    [Header("Intro Dialogue Lines")]
    [TextArea(2, 4)]
    [SerializeField] private string[] introLines;

    [Header("Mage Dialogue Lines")]
    [TextArea(2, 5)]
    [SerializeField] private string[] mageLines;

    // =========================
    private int _currentLineIndex;
    private TutorialState _currentState;
    private bool _isLookingAtMage = false;

    // =========================
    private void Start()
    {
        StartIntro();
    }

    private void Update()
    {
        if (_currentState == TutorialState.IntroDialogue ||
            _currentState == TutorialState.MageDialogue)
        {
            if (Input.GetMouseButtonDown(0))
                NextLine();
        }

        if (_isLookingAtMage && fpsController != null && mage != null)
            SmoothLookAtMage();
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
    }

    private void EndIntro()
    {
        tutorialPanel.SetActive(false);

        if (mage != null)
            mage.TeleportDownstairs();

        LockPlayer(false);
        _currentState = TutorialState.ReachMageDownstairs;
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
                EndIntro();
            else
                tutorialText.text = introLines[_currentLineIndex];
        }
        else if (_currentState == TutorialState.MageDialogue)
        {
            if (_currentLineIndex >= mageLines.Length)
                EndMageDialogue();
            else
                tutorialText.text = mageLines[_currentLineIndex];
        }
    }

    // =========================
    // PLAYER RAGGIUNGE LA MAGA
    // =========================
    public void OnPlayerReachedMage()
    {
        if (_currentState != TutorialState.ReachMageDownstairs)
            return;

        LockPlayer(true);
        _isLookingAtMage = true;

        FaceMageToPlayer();

        _currentState = TutorialState.MageDialogue;
        _currentLineIndex = 0;

        tutorialPanel.SetActive(true);
        tutorialText.text = mageLines[_currentLineIndex];
    }

    private void EndMageDialogue()
    {
        tutorialPanel.SetActive(false);
        LockPlayer(false);
        _isLookingAtMage = false;

        if (mage != null && finalTeleportPoint != null)
            mage.TeleportTo(finalTeleportPoint);

        _currentState = TutorialState.ReachMageDownstairs;
    }

    // =========================
    // ROTAZIONE PLAYER
    // =========================
    private void SmoothLookAtMage()
    {
        Vector3 targetPos = mage.transform.position + Vector3.up * 1.5f;

        Vector3 flatDir = targetPos - fpsController.transform.position;
        flatDir.y = 0f;

        if (flatDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatDir);
            fpsController.transform.rotation =
                Quaternion.Slerp(fpsController.transform.rotation,
                                 targetRotation,
                                 rotationSpeed * Time.deltaTime);
        }
    }

    // =========================
    // ROTAZIONE MAGA
    // =========================
    private void FaceMageToPlayer()
    {
        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(RotateTowardsPlayer());
    }

    private IEnumerator RotateTowardsPlayer()
    {
        Vector3 dir = fpsController.transform.position - mage.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f)
            yield break;

        Quaternion targetRotation = Quaternion.LookRotation(dir);

        while (Quaternion.Angle(mage.transform.rotation, targetRotation) > 0.5f)
        {
            mage.transform.rotation = Quaternion.RotateTowards(
                mage.transform.rotation,
                targetRotation,
                rotationSpeedMage * Time.deltaTime
            );

            yield return null;
        }
    }

    // =========================
    private void LockPlayer(bool lockPlayer)
    {
        foreach (var script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = !lockPlayer;
        }
    }
}
