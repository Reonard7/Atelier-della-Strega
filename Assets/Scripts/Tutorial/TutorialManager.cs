using TMPro;
using UnityEngine;

public enum TutorialState
{
    IntroDialogue,
    ReachMageDownstairs,
    MageDialogue
}

public class TutorialManager : MonoBehaviour
{
    [Header("UI Tutorial")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI tutorialText;

    [Header("Player")]
    [SerializeField] private MonoBehaviour[] scriptsToDisable; // metti qui tutti gli script da bloccare
    [SerializeField] private Transform PlayerTransform;

    [Header("Mage")]
    [SerializeField] private MageController mage;

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

    private int _currentLineIndex;
    private TutorialState _currentState;

    void Start()
    {
        Debug.Log("[TutorialManager] Start della scena");
        StartIntro();
    }

    void Update()
    {
        // Click per scorrere le linee dei dialoghi
        if (_currentState == TutorialState.IntroDialogue || _currentState == TutorialState.MageDialogue)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("[TutorialManager] Click mouse rilevato durante dialogo");
                NextLine();
            }
        }
    }

    // =========================
    // INTRO
    // =========================
    private void StartIntro()
    {
        Debug.Log("[TutorialManager] StartIntro: inizializzo dialogo introduttivo");

        _currentState = TutorialState.IntroDialogue;
        _currentLineIndex = 0;

        tutorialPanel.SetActive(true);
        tutorialText.text = introLines[_currentLineIndex];

        LockPlayer(true);
        Debug.Log("[TutorialManager] Player bloccato durante intro");
        Debug.Log("[TutorialManager] Mostrata linea: " + introLines[_currentLineIndex]);
    }

    private void NextLine()
    {
        _currentLineIndex++;

        if (_currentState == TutorialState.IntroDialogue)
        {
            if (_currentLineIndex >= introLines.Length)
            {
                Debug.Log("[TutorialManager] Fine dialogo intro");
                EndIntro();
            }
            else
            {
                tutorialText.text = introLines[_currentLineIndex];
                Debug.Log("[TutorialManager] Mostrata linea intro: " + introLines[_currentLineIndex]);
            }
        }
        else if (_currentState == TutorialState.MageDialogue)
        {
            if (_currentLineIndex >= mageLines.Length)
            {
                Debug.Log("[TutorialManager] Fine dialogo maga");
                EndMageDialogue();
            }
            else
            {
                tutorialText.text = mageLines[_currentLineIndex];
                Debug.Log("[TutorialManager] Mostrata linea maga: " + mageLines[_currentLineIndex]);
            }
        }
    }

    private void EndIntro()
    {
        tutorialPanel.SetActive(false);

        if (mage != null)
        {
            Debug.Log("[TutorialManager] Teletrasporto della maga al piano di sotto");
            mage.TeleportDownstairs();
        }
        else
        {
            Debug.LogWarning("[TutorialManager] MageController non assegnato!");
        }

        // Blocca solo le interazioni, ma lascia il player libero di muoversi
        LockInteractions(true);

        _currentState = TutorialState.ReachMageDownstairs;
        Debug.Log("[TutorialManager] Stato aggiornato a ReachMageDownstairs (interazioni bloccate, movimento attivo)");
    }

    // =========================
    // STEP 2 – Trigger zona maga
    // =========================
    private void FaceMageToPlayer()
    {
        if (mage == null || PlayerTransform == null) return;

        Vector3 direction = (PlayerTransform.position - mage.transform.position).normalized;
        direction.y = 0; // ruota solo sull'asse Y

        if (direction != Vector3.zero)
        {
            mage.transform.forward = direction;
            Debug.Log("[TutorialManager] Maga ruotata verso il player");
        }
    }

    public void OnPlayerReachedMage()
    {
        Debug.Log("[TutorialManager] OnPlayerReachedMage chiamato");

        if (_currentState != TutorialState.ReachMageDownstairs)
        {
            Debug.Log("[TutorialManager] Ignoro OnPlayerReachedMage, stato corrente: " + _currentState);
            return;
        }

        FaceMageToPlayer();

        _currentState = TutorialState.MageDialogue;
        _currentLineIndex = 0;

        tutorialPanel.SetActive(true);
        tutorialText.text = mageLines[_currentLineIndex];

        // Blocca tutte le interazioni durante il dialogo (compreso movimento se vuoi)
        LockPlayer(true);

        Debug.Log("[TutorialManager] Player bloccato durante dialogo maga");
        Debug.Log("[TutorialManager] Mostrata linea: " + mageLines[_currentLineIndex]);
    }

    private void EndMageDialogue()
    {
        tutorialPanel.SetActive(false);

        // Riattiva tutti gli script
        foreach (var script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = true;
        }

        Debug.Log("[TutorialManager] Player sbloccato dopo dialogo maga, tutte le interazioni ripristinate");

        _currentState = TutorialState.ReachMageDownstairs; // o nuovo stato
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

        Debug.Log("[TutorialManager] LockPlayer(" + lockPlayer + ")");
    }

    private void LockInteractions(bool lockInteractions)
    {
        foreach (var script in scriptsToDisable)
        {
            if (script != null && !(script is CharacterController))
                script.enabled = !lockInteractions;
        }

        Debug.Log("[TutorialManager] Interazioni bloccate: " + lockInteractions);
    }
}
