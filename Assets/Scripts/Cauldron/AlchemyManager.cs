using Events;
using GameData.Scripts.Items;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using System.Collections;

public class AlchemyManager : MonoBehaviour
{
    /*
     * Cosa deve fare il manager:
     * 
     * - Tenere una reference ai tre ingredienti negli slot         FATTO!
     * - Un metodo chiamabile per trovare la pozione corretta in relazione ai tre ingredienti       FATTO!
     * - Metodo che dia in output la pozione craftata (da passare al grimorio -> evento!)
     * - Tutto relativo al dado (far startare l'animazione e restituire il numero generato)
     * - Metodo che printi la pozione creata a schermo
     * 
     * 
    */

    [Header("Craftable")]
    [SerializeField] private List<Potion> craftablePotions;
    [SerializeField] private List<Ingredient> ingredients;
    private Potion currentResult;
    private int diceResult;
    private bool _isBrewing = false;

    [Header("Canvas and Cinemachines")]
    [SerializeField] private GameObject _cauldronCanvas;
    [SerializeField] private CinemachineVirtualCamera _mainCam;
    [SerializeField] private CinemachineVirtualCamera _cauldronCam;
    [SerializeField] private FirstPersonController _playerFPS;
    private bool _cursorLocked;
    [SerializeField] private Animator _animator;


    private void OnEnable()
    {
        InteractionEvents.OnCauldronInteracted += OnCauldronInteracted;
        AlchemyEvents.OnIngredientDropped += OnIngredientDropped;
        AlchemyEvents.OnIngredientRemovedFromSlot += OnIngredientRemovedFromSlot;
        AlchemyEvents.OnDiceCast += OnDiceCast;
        AlchemyEvents.OnAnimationEnded += OnAnimationEnded;
    }

    private void OnDisable()
    {
        InteractionEvents.OnCauldronInteracted -= OnCauldronInteracted;
        AlchemyEvents.OnIngredientDropped -= OnIngredientDropped;
        AlchemyEvents.OnIngredientRemovedFromSlot -= OnIngredientRemovedFromSlot;
        AlchemyEvents.OnDiceCast -= OnDiceCast;
        AlchemyEvents.OnAnimationEnded -= OnAnimationEnded;
    }

    private void Start()
    {
        // Hide cursor
        LockCursor();
        // Hide Cauldron Canvas
        _cauldronCanvas.SetActive(false);

        _mainCam.Priority = 20;
        _cauldronCam.Priority = 10;
    }

    private void Update()
    {
        if (_cauldronCanvas.activeSelf && Input.GetKeyDown(KeyCode.H))
        {
            InteractionEvents.OnCauldronExit?.Invoke();
            StartCoroutine(CloseCauldronRoutine());
        }
    }

    // COROUTINE
    private IEnumerator OpenCauldronRoutine()
    {
        // Switch camera priority
        _cauldronCam.Priority = 20;
        _mainCam.Priority = 10;

        // Wait one frame for Cinemachine to blend
        yield return new WaitForSeconds(1f);

        // Disable player movement
        _playerFPS.enabled = false;

        // Show UI
        _cauldronCanvas.SetActive(true);

        // Unlock cursor
        UnlockCursor();
    }

    private IEnumerator CloseCauldronRoutine()
    {
        // Switch camera back
        _mainCam.Priority = 20;
        _cauldronCam.Priority = 10;

        // Wait one frame for blend
        yield return null;

        // Hide UI
        _cauldronCanvas.SetActive(false);

        // Enable player movement
        _playerFPS.enabled = true;

        // Lock cursor
        LockCursor();
    }

    // HELPER METHODS
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        _cursorLocked = false;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _cursorLocked = true;
    }

    private void OpenCauldronCanvas()
    {
        _playerFPS.enabled = false;
        _cauldronCanvas.SetActive(true);
        UnlockCursor();
    }

    public void CloseFunction()
    {
        if (_cauldronCanvas.activeSelf)
        {
            InteractionEvents.OnCauldronExit?.Invoke();
            StartCoroutine(CloseCauldronRoutine());
        }
    }

    private void CloseCauldronCanvas()
    {
        _playerFPS.enabled = true;
        _cauldronCanvas.SetActive(false);
        LockCursor();
    }

    private Potion FindCraftablePotion()
    {
        if (ingredients.Count != 3)
            return null;

        // usiamo un approccio sequenziale: prendiamo la lista di pozioni craftabili dal primo ingrediente e troviamo l'intersezione con quelle del secondo
        Ingredient first = ingredients[0];

        foreach (Potion candidate in first.craftablePotions)
        {
            bool canCraft = true;

            for (int i = 1; i < ingredients.Count; i++)
            {
                if (!System.Array.Exists(
                        ingredients[i].craftablePotions,
                        p => p == candidate))
                {
                    canCraft = false;
                    break;
                }
            }

            if (canCraft) return candidate;
        }

        return null;
    }

    private bool HasDuplicateIngredients()
    {
        for (int i = 0; i < ingredients.Count; i++)
        {
            for (int j = i + 1; j < ingredients.Count; j++)
            {
                if (ingredients[i] == ingredients[j])
                    return true;
            }
        }

        return false;
    }

    public void Brew()
    {
        // chiama l'evento per far partire il dado, che sta sul suo script dedicato (Dice.cs)
        if (ingredients.Count != 3) return;
        if (_isBrewing) return;

        Potion thrashPotion = craftablePotions.Find(x => x.id == "thrash_potion");
        Potion mysteriousPotion = craftablePotions.Find(x => x.id == "mysterious_potion");
        Potion craftedPotion = null;
        bool isMirable = false;

        _animator.SetTrigger("OnBrewingStarted");
        AlchemyEvents.OnBrewingStarted?.Invoke();
        _isBrewing = true;

        /* qua dobbiamo mettere degli switch case per le diverse pozioni da craftare.
         * Le casistiche sono:
         * - currentResult = Potion, diceResult = 1 -> Thrash Potion
         * - currentResult = Potion, diceResult = 2-19 -> Potion of XXX
         * - currentResult = Potion, diceResult = 20 -> Mirable Potion of XXX
         * - currentResult = null, diceResult = 1-15 -> Thrash Potion
         * - currentResult = null, diceResult = 16-19 -> Mysterious Potion
         * - currentResult = null, diceResult = 20 -> Mirable Mysterious Potion
         */

        switch (currentResult, diceResult)
        {
            case (not null, 1):
                (craftedPotion, isMirable) = (thrashPotion, false);
                break;
            case (not null, int n) when (n > 1 && n < 20):
                (craftedPotion, isMirable) = (currentResult, false);
                break;
            case (not null, 20):
                (craftedPotion, isMirable) = (currentResult, true);
                break;
            case (null, int n) when (n < 16):
                (craftedPotion, isMirable) = (thrashPotion, false);
                break;
            case (null, int n) when (n > 15 && n < 20):
                (craftedPotion, isMirable) = (mysteriousPotion, false);
                break;
            case (null, 20):
                (craftedPotion, isMirable) = (mysteriousPotion, true);
                break;
        }

        AlchemyEvents.OnPotionCrafted?.Invoke(craftedPotion, isMirable);
    }

    // EVENTS
    private void OnIngredientDropped(Ingredient ingredient)
    {
        InteractionEvents.OnIngredientLocked?.Invoke(ingredient);
        ingredients.Add(ingredient);

        if (ingredients.Count != 3) return;

        // check se alcuni ingredienti sono doppioni (nessuna ricetta ha ingredienti doppioni)
        if (HasDuplicateIngredients()) return;

        currentResult = FindCraftablePotion();
    }

    private void OnIngredientRemovedFromSlot(Ingredient ingredient)
    {
        InteractionEvents.OnIngredientUnlocked?.Invoke(ingredient);
        ingredients.Remove(ingredient);
        currentResult = null;
    }

    private void OnCauldronInteracted()
    {
        StartCoroutine(OpenCauldronRoutine());
    }

    private void OnDiceCast(int number)
    {
        diceResult = number;
    }

    private void OnAnimationEnded()
    {
        _isBrewing = false;
    }
}
