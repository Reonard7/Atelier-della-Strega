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
    [SerializeField] private MonoBehaviour[] scriptsToDisable;
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
        // Gestione click solo per scorrere le linee dei dialoghi
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

        LockPlayer(false);
        Debug.Log("[TutorialManager] Player sbloccato dopo intro");

        _currentState = TutorialState.ReachMageDownstairs;
        Debug.Log("[TutorialManager] Stato aggiornato a ReachMageDownstairs");
    }

    // =========================
    // STEP 2 – Trigger zona maga
    // =========================
    public void OnPlayerReachedMage()
    {
        FaceMageToPlayer();

        Debug.Log("[TutorialManager] OnPlayerReachedMage chiamato");

        if (_currentState != TutorialState.ReachMageDownstairs)
        {
            Debug.Log("[TutorialManager] Ignoro OnPlayerReachedMage, stato corrente: " + _currentState);
            return;
        }

        Debug.Log("[TutorialManager] Player ha raggiunto la maga, avvio dialogo pozioni");

        _currentState = TutorialState.MageDialogue;
        _currentLineIndex = 0;

        tutorialPanel.SetActive(true);
        tutorialText.text = mageLines[_currentLineIndex];

        LockPlayer(true);
        Debug.Log("[TutorialManager] Player bloccato durante dialogo maga");
        Debug.Log("[TutorialManager] Mostrata linea: " + mageLines[_currentLineIndex]);
    }

    private void FaceMageToPlayer()
{
    if (mage == null) return;

    Vector3 direction = (PlayerTransform.position - mage.transform.position).normalized;

    direction.y = 0;

    if (direction != Vector3.zero)
    {
        mage.transform.forward = direction;
        Debug.Log("[TutorialManager] Maga ruotata verso il player");
    }
}

    private void EndMageDialogue()
    {
        tutorialPanel.SetActive(false);
        LockPlayer(false);

        Debug.Log("[TutorialManager] Player sbloccato dopo dialogo maga");
        Debug.Log("[TutorialManager] Dialogo maga completato, possibile passare allo step successivo");

        _currentState = TutorialState.ReachMageDownstairs; // oppure nuovo stato
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
}
