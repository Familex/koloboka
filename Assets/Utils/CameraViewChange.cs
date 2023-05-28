using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
    /// <summary>
    /// MonoBehaviour that allows switching between cameras with a button.
    /// </summary>
    public class CameraViewChange : MonoBehaviour
    {
        /* ---- Inspector things ---- */
        [SerializeField] private Camera[] cameras;
        [SerializeField] private InputActionReference camSwitch;
        
        /* ---- Private mutables ---- */
        private int _currentCamera;

        /* ---- Unity overrides ---- */
        /// <summary>
        /// Set up actions callbacks.
        /// </summary>
        private void Awake()
        {
            if (cameras.Length > 0)
            {
                camSwitch.action.performed += ctx =>
                {
                    _currentCamera += Mathf.RoundToInt(ctx.ReadValue<float>());
                    if (_currentCamera < 0) _currentCamera = cameras.Length - 1;
                    if (_currentCamera >= cameras.Length) _currentCamera = 0;
                    TurnOnCurrent();
                };
            }
        }
        
        /// <summary>
        /// Turn on the first camera.
        /// </summary>
        private void Start()
        {
            if (cameras.Length > 0)
            {
                _currentCamera = 0;
                TurnOnCurrent();
            }
        }

        /// <summary>
        /// Enable the action.
        /// </summary>
        private void OnEnable()
        {
            camSwitch.action.Enable();
        }

        /// <summary>
        /// Disable the action.
        /// </summary>
        private void OnDisable()
        {
            camSwitch.action.Disable();
        }

        /* ---- Private methods ---- */
        /// <summary>
        /// Util method to turn on the current camera and turn off the others.
        /// </summary>
        private void TurnOnCurrent()
        {
            foreach (var cam in cameras)
            {
                cam.enabled = false;
            }

            cameras[_currentCamera].enabled = true;
        }
    }
}
