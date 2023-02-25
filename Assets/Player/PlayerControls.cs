using UnityEngine;

namespace Player
{
    public class PlayerControls : MonoBehaviour
    {
        [SerializeField] private float force = 10;
        [SerializeField] private float rotationSpeed = 15;
        [SerializeField] private Rigidbody playerRigidbody;
        [SerializeField] private Transform firstView;
        
        [SerializeField] private Camera[] cameras;

        private Vector3 LookVec => firstView.forward;
        
        private ControlActions _controlActions;
        private int _currentCamera;

        private void TurnOnCurrent()
        {
            foreach (var cam in cameras)
            {
                cam.enabled = false;
            }

            cameras[_currentCamera].enabled = true;
        }
        
        private void Awake()
        {
            _controlActions = new ControlActions();

            if (cameras.Length > 0)
            {
                _controlActions.debug.camerachange.performed += ctx =>
                {
                    _currentCamera += Mathf.RoundToInt(ctx.ReadValue<float>());
                    if (_currentCamera < 0) _currentCamera = cameras.Length - 1;
                    if (_currentCamera >= cameras.Length) _currentCamera = 0;
                    TurnOnCurrent();
                };
            }
        }

        public void OnEnable()
        {
            _controlActions.Enable();
        }

        public void OnDisable()
        {
            _controlActions.Disable();
        }

        private void Start()
        {
            if (cameras.Length > 0)
            {
                _currentCamera = 0;
                TurnOnCurrent();
            }
        }

        private void Update()
        {
            // Debug.Log(_rigidbody.rotation.eulerAngles);
            
            // Move and rotate
            {
                var dPad = _controlActions.gameplay.move.ReadValue<Vector2>();
                
                var deltaForce = LookVec * dPad.y * force;
                var deltaRotate = dPad.x * rotationSpeed;

                Debug.DrawRay(playerRigidbody.transform.position, deltaForce, Color.magenta);
                
                playerRigidbody.AddForce(deltaForce);
                playerRigidbody.transform.Rotate(0, deltaRotate, 0);
            }
        }
    }
}
