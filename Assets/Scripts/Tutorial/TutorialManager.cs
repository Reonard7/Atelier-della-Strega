using TMPro;
using UnityEngine;

public enum TutorialState
{
    IntroDialogue,
    ReachMageDownstairs
}

public class TutorialManager : MonoBehaviour
{
    [Header("UI Tutorial")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI tutorialText;

    [Header("Player")]
    [Tooltip("Script del player da disabilitare durante il tutorial")]
    [SerializeField] private MonoBehaviour[] scriptsToDisable;

    [Header("Mage")]
    [SerializeField] private MageController mage;

    [Header("Intro Dialogue Lines")]
    [TextArea(2, 4)]
    [SerializeField] private string[] introLines =
    {
        "Ciao.",
        "Ciao."
    };

    private int _currentLineIndex;
    private TutorialState _currentState;

    void Start()
    {
        StartIntro();
    }

    void Update()
    {
        if (_currentState == TutorialState.IntroDialogue)
        {
            if (Input.GetMouseButtonDown(0))
            {
                NextIntroLine();
            }
        }
    }

    // =========================
    // INTRO
    // =========================

    private void StartIntro()
    {
        Debug.Log("Tutorial: Start Intro");

        _currentState = TutorialState.IntroDialogue;
        _currentLineIndex = 0;

        tutorialPanel.SetActive(true);
        tutorialText.text = introLines[_currentLineIndex];

        LockPlayer(true);
    }

    private void NextIntroLine()
    {
        _currentLineIndex++;

        if (_currentLineIndex >= introLines.Length)
        {
            EndIntro();
        }
        else
        {
            tutorialText.text = introLines[_currentLineIndex];
        }
    }

    private void EndIntro()
    {
        Debug.Log("Tutorial: End Intro");

        tutorialPanel.SetActive(false);

        if (mage != null)
        {
            mage.TeleportDownstairs();
        }
        else
        {
            Debug.LogWarning("MageController non assegnato nel TutorialManager");
        }

        LockPlayer(false);
        _currentState = TutorialState.ReachMageDownstairs;
    }

    // =========================
    // STEP 2 (trigger esterno)
    // =========================

    public void OnPlayerReachedMage()
    {
        if (_currentState != TutorialState.ReachMageDownstairs)
            return;

        Debug.Log("Tutorial: Player ha raggiunto la maga al piano di sotto");

        // Qui in futuro partirà il tutorial crafting
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
    }
}
