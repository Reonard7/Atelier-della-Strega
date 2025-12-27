using StarterAssets;
using UnityEngine;

namespace Interaction
{
    public class CauldronInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject _cauldronCanvas;
        private FirstPersonController _playerFPS;
        private bool _cursorLocked;

        private void Start()
        {
            // Hide cursor
            LockCursor();
            // Initialize the FPSController
            _playerFPS = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();
            // Hide Cauldron Canvas
            _cauldronCanvas.SetActive(false);
        }

        private void Update()
        {
            if (_cursorLocked && _cauldronCanvas.activeSelf && Input.GetKeyDown(KeyCode.H))
                CloseCauldronCanvas();
        }
        
        public void Interact(GameObject caller)
        {
            OpenCauldronCanvas();
        }
    
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

        private void CloseCauldronCanvas()
        {
            _playerFPS.enabled = true;
            _cauldronCanvas.SetActive(false);
            LockCursor();
        }
    }
}
