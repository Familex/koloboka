using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
    public class CameraViewChange : MonoBehaviour
    {
        /* ---- Inspector things ---- */
        [SerializeField] private Camera[] cameras;
        [SerializeField] private InputActionReference camSwitch;
        
        /* ---- Private mutables ---- */
        private int _currentCamera;

        /* ---- Unity overrides ---- */
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
        
        private void Start()
        {
            if (cameras.Length > 0)
            {
                _currentCamera = 0;
                TurnOnCurrent();
            }
        }

        private void OnEnable()
        {
            camSwitch.action.Enable();
        }

        private void OnDisable()
        {
            camSwitch.action.Disable();
        }

        /* ---- Private methods ---- */
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
