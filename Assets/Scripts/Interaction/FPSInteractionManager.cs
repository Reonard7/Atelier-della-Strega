using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class FPSInteractionManager : MonoBehaviour
    {
        [SerializeField] private Transform _fpsCameraT;
        [SerializeField] private bool _debugRay;
        [SerializeField] private float _interactionDistance;

        [SerializeField] private Image _target;
        [SerializeField] private TextMeshProUGUI _text;

        [SerializeField] private Color normalColor = new Color(1f, 0.6f, 0.2f);
        [SerializeField] private Color interactableColor = new Color(0.8f, 0.3f, 0.1f);

        public string _name;

        private IInteractable _pointingInteractable;

        private CharacterController _fpsController;
        private Vector3 _rayOrigin;

        private bool _inCrafting;

        private void OnEnable()
        {
            InteractionEvents.OnCauldronInteracted += OnCauldronInteracted;
            InteractionEvents.OnCauldronExit += OnCauldronExit;
        }

        private void OnDisable()
        {
            InteractionEvents.OnCauldronInteracted -= OnCauldronInteracted;
            InteractionEvents.OnCauldronExit -= OnCauldronExit;
        }
        void Start()
        {
            _fpsController = GetComponent<CharacterController>();
            _inCrafting = false;
        }

        void Update()
        {
            _rayOrigin = _fpsCameraT.position + _fpsController.radius * _fpsCameraT.forward;

            if (!_inCrafting)
                CheckInteraction();

            UpdateUITarget();

            if (_debugRay)
                DebugRaycast();
        }

        private void CheckInteraction()
        {
            var ray = new Ray(_rayOrigin, _fpsCameraT.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _interactionDistance))
            {
                //Check if is interactable
                _pointingInteractable = hit.collider?.GetComponent<IInteractable>();

                switch (_pointingInteractable)
                {
                    // Interactable is an Ingredient
                    case IngredientInteractable interactableIngredient:
                    {
                        _name = interactableIngredient.ingredient.displayName; 
                        if (Input.GetMouseButtonDown(0))
                            interactableIngredient.Interact(gameObject);
                        break;
                    }
                    // Interactable is the Cauldron
                    case CauldronInteractable cauldronInteractable:
                    {
                        _name = "Calderone";
                        if (Input.GetMouseButtonDown(0))
                            cauldronInteractable.Interact(gameObject);
                        break;
                    }
                    // Interactable is a Spell
                    case SpellInteractable spellInteractable:
                    {
                        _name = spellInteractable.spell.displayName;
                        if (Input.GetMouseButtonDown(0))
                            spellInteractable.Interact(gameObject);
                        break;
                    }
                    // Interactable is a Portal
                    case PortalInteractable portalInteractable:
                    {
                        _name = portalInteractable.name;
                        if (Input.GetMouseButtonDown(0))
                            portalInteractable.Interact(gameObject);
                        break;
                    }
                    //Interactable is a Treasure
                    case TreasureInteractable treasureInteractable:
                    {
                        _name = "Inspect";
                        if (Input.GetMouseButtonDown(0))
                            treasureInteractable.Interact(gameObject);
                        break;
                    }
                }
            }
            else
            {
                _pointingInteractable = null;
            }
        }

        private void UpdateUITarget()
        {
            if (_inCrafting)
            {
                _target.enabled = false;
                _text.enabled = false;
            }
            else
            {
                _target.enabled = true;
                _text.enabled = true;
            }

            if (_pointingInteractable != null)
            {
                _target.color = interactableColor;
                _text.text = _name;
            }
            else
            {
                _target.color = normalColor;
                _text.text = "";
            }
        }

        private void DebugRaycast()
        {
            Debug.DrawRay(_rayOrigin, _fpsCameraT.forward * _interactionDistance, Color.red);
        }

        private void OnCauldronInteracted()
        {
            _inCrafting = true;
        }

        private void OnCauldronExit()
        {
            _inCrafting = false;
        }
    }
}
