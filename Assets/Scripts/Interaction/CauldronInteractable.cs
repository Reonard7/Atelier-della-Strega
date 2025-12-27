using UnityEngine;

namespace Interaction
{
    public class CauldronInteractable : Interactable
    {
        [SerializeField] private GameObject _cauldronCanvas;
        private bool _cursorLocked;

        private void Start()
        {
            // Hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            _cursorLocked = true;
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
    
        public override void Interact(GameObject caller)
        {
            UnlockCursor();
            _cauldronCanvas.SetActive(true);
        }
    }
}
