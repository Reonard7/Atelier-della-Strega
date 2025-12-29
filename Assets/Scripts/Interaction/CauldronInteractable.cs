using StarterAssets;
using UnityEngine;
using Cinemachine;
using System.Collections;

namespace Interaction
{
    public class CauldronInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject _cauldronCanvas;
        [SerializeField] private CinemachineVirtualCamera _mainCam;
        [SerializeField] private CinemachineVirtualCamera _cauldronCam;
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

            _mainCam.Priority = 20;
            _cauldronCam.Priority = 10;
        }

        private void Update()
        {
            if (_cauldronCanvas.activeSelf && Input.GetKeyDown(KeyCode.H))
                StartCoroutine(CloseCauldronRoutine());
        }
        
        public void Interact(GameObject caller)
        {
            StartCoroutine(OpenCauldronRoutine());
        }

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
