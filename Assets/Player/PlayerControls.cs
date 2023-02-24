using UnityEngine;

namespace Player
{
    public class PlayerControls : MonoBehaviour
    {
        [SerializeField] private float force = 10;
        [SerializeField] private float rotationSpeed = 15;
        [SerializeField] private Camera[] cameras;
        [SerializeField] private Rigidbody playerRigidbody;
        
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
                var deltaForce = new Vector3();
                var deltaRotate = new Vector3();
                var rotation = playerRigidbody.transform.rotation;
                var rotationX = rotation.x;
                var rotationY = rotation.y;
                var forward = new Vector3(
                    Mathf.Sin(rotationY + rotationX),
                    0,
                    Mathf.Cos(rotationY + rotationX)
                );

                var dPad = _controlActions.gameplay.move.ReadValue<Vector2>();

                deltaForce += forward * dPad.y;
                deltaRotate.y += dPad.x * rotationSpeed;

                deltaForce *= force;

                Debug.DrawRay(playerRigidbody.transform.position, deltaForce, Color.magenta);
                
                playerRigidbody.AddForce(deltaForce);
                playerRigidbody.transform.Rotate(deltaRotate);
            }
        }
    }
}
