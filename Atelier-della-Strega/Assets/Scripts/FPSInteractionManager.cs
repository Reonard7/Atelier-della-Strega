using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FPSInteractionManager : MonoBehaviour
{
    [SerializeField] private Transform _fpsCameraT;
    [SerializeField] private bool _debugRay;
    [SerializeField] private float _interactionDistance;

    [SerializeField] private Image _target;
    [SerializeField] private TextMeshProUGUI _text;
    public string _name;

    private Interactable _pointingInteractable;

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
        Ray ray = new Ray(_rayOrigin, _fpsCameraT.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _interactionDistance))
        {
            //Check if is interactable
            _pointingInteractable = hit.transform.GetComponent<Interactable>();
            Debug.Log(hit.transform.name);
            if (_pointingInteractable)
            {
                _name = hit.transform.GetComponent<IngredientInteractable>().ingredient.displayName;
                if (Input.GetMouseButtonDown(0))
                    _pointingInteractable.Interact(gameObject);
            }
        }
        else
        {
            _pointingInteractable = null;
        }
    }

    private void UpdateUITarget()
    {
        if (_pointingInteractable)
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
