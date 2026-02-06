using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData.Scripts.Items;
using Events;
using StarterAssets;
using TMPro;

/// <summary>
/// Tutorial iniziale: NPC guida, scritte e raccolta ingredienti.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [Header("References")]
    public TutorialNPC npc;                       // La maga
    public Transform npcTeleportPoint;            // Punto di teletrasporto
    public AlchemyManager alchemyManager;         // Manager per verificare ingredienti raccolti
    public FirstPersonController playerFPS;       // Player controller
    public StarterAssetsInputs playerInput;       // Input player
    public GameObject tutorialTextCanvas;         // Canvas scritte
    public TextMeshProUGUI tutorialTextTMP;       // TMP per testo

    [Header("Settings")]
    public List<Ingredient> requiredIngredients;  // Ingredienti da raccogliere
    public float playerCheckRadius = 2f;          // Vicinanza per trigger NPC
    public float textDisplayTime = 3f;

    private bool tutorialActive = false;

    private void Start()
    {
        StartCoroutine(TutorialRoutine());
    }

    private IEnumerator TutorialRoutine()
    {
        tutorialActive = true;

        // 1️ Blocca movimento
        playerFPS.enabled = false;
        playerInput.enabled = false;

        // 2️ NPC saluta
        yield return ShowText("Benvenuto! Io sono la tua guida, seguimi e imparerai l'arte dell'alchimia.");

        // 3️ NPC dice "Seguimi" e teletrasporta
        yield return ShowText("Seguimi...");
        npc.TeleportTo(npcTeleportPoint.position);
        yield return new WaitUntil(() => npc.HasReachedDestination);

        // 4️ Sblocca movimento player e aspetta che arrivi vicino all'npc
        playerFPS.enabled = true;
        playerInput.enabled = true;

        yield return ShowText("Avvicinati a me per continuare il tutorial.");

        yield return new WaitUntil(() =>
            Vector3.Distance(playerFPS.transform.position, npc.transform.position) <= playerCheckRadius);

        // 5️ Mostra altra scritta
        yield return ShowText("Ottimo! Adesso raccogli gli ingredienti giusti per preparare la tua prima pozione.");

        // 6️ Aspetta che il giocatore raccolga gli ingredienti necessari
        yield return new WaitUntil(() => PlayerHasAllIngredients());

        // 7️ Tutorial completato
        yield return ShowText("Perfetto! Ora puoi iniziare a sperimentare con le pozioni.");

        tutorialActive = false;
    }

    private IEnumerator ShowText(string message)
    {
        tutorialTextCanvas.SetActive(true);
        tutorialTextTMP.text = message;
        yield return new WaitForSeconds(textDisplayTime);
        tutorialTextCanvas.SetActive(false);
    }

    private bool PlayerHasAllIngredients()
    {
        // Controlla la lista ingredients di AlchemyManager
        foreach (var req in requiredIngredients)
        {
            if (!alchemyManager.ingredients.Contains(req))
                return false;
        }
        return true;
    }
}
