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
        public string _name;

        private IInteractable _pointingInteractable;

        private CharacterController _fpsController;
        private Vector3 _rayOrigin;


        void Start()
        {
            _fpsController = GetComponent<CharacterController>();
        }

        void Update()
        {
            _rayOrigin = _fpsCameraT.position + _fpsController.radius * _fpsCameraT.forward;

            CheckInteraction();

            UpdateUITarget();

            if (_debugRay)
                DebugRaycast();
        }

        private void CheckInteraction()
        {
            if (Input.GetMouseButtonDown(1))
            {
                InteractionEvents.OnIngredientDiscard?.Invoke();
            }

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
                }
            }
            else
            {
                _pointingInteractable = null;
            }
        }

        private void UpdateUITarget()
        {
            if (_pointingInteractable != null)
            {
                _target.color = Color.green;
                _text.text = _name;
            }
            else
            {
                _target.color = Color.red;
                _text.text = "";
            }
        }

        private void DebugRaycast()
        {
            Debug.DrawRay(_rayOrigin, _fpsCameraT.forward * _interactionDistance, Color.red);
        }
    }
}
