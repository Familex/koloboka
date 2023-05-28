using UnityEngine;

namespace Player
{
    /// <summary>
    /// MonoBehaviour for player controls.
    /// </summary>
    public class PlayerControls : MonoBehaviour
    {
        /* ---- Private types ---- */
        private enum MoveType
        {
            // Problems in comments
            GlobalAxis,   // Minimap view only    
            ViewAxisAndRotate,  // Hard to determine top and bottom
            ViewAxisAndRotateWithUpsideDownMemorize  // Hard to start brake instantly
        }
        
        /* ---- Inspector things ---- */
        [SerializeField] private float force = 15;
        [SerializeField] private float rotationSpeed = 15;
        [SerializeField] private Rigidbody playerRigidbody;
        [SerializeField] private Transform firstView;
        [SerializeField] private MoveType currMoveType = MoveType.ViewAxisAndRotateWithUpsideDownMemorize;
        [SerializeField] private float brakeCoefficient = 0.95f; 
        [SerializeField] private float restoreRotationSpeed = 0.05f;
        [SerializeField, Utils.Unstable] private bool iceEffect = false;
        
        /* ---- Private mutables ---- */
        private ControlActions _controlActions;
        /// <summary>
        /// Remembers the rigidbody state at the first triggering of user input. <br/>
        /// Null if is no user input; true if not upside down. <br/>
        /// </summary>
        private bool? _isNotUpsideDownMemorize;
        
        /* ---- Private properties ---- */
        private Vector3 Forward => firstView.forward;
        private Vector3 Up => firstView.up;
        private bool IsNotUpsideDown => Vector3.Dot(Up, Vector3.up) > 0;
        
        /* ---- Public properties ---- */
        public float RelativeY
        {
            get
            {
                var result = playerRigidbody.transform.rotation.eulerAngles;
                var reverse = (_isNotUpsideDownMemorize ?? true) != IsNotUpsideDown;
                return result.y + (reverse ? 180 : 0);
            }
        }

        /* ---- Unity overrides ---- */
        /// <summary>
        /// Create control actions.
        /// </summary>
        private void Awake()
        {
            _controlActions = new ControlActions();
        }

        /// <summary>
        /// Enable control actions.
        /// </summary>
        public void OnEnable()
        {
            _controlActions.Enable();
        }

        /// <summary>
        /// Disable control actions.
        /// </summary>
        public void OnDisable()
        {
            _controlActions.Disable();
        }

        /// <summary>
        /// Make the player move according to the user input and current move strategy (<see cref="currMoveType"/>).
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">If move strategy is not supported</exception>
        private void FixedUpdate()
        {
            var dPad = _controlActions.gameplay.move.ReadValue<Vector2>();
            var inputProvided = dPad.magnitude > 1E-5;
            
            // Move and rotate
            {
                // Variables
                {
                    if (Mathf.Abs(dPad.y) < 1E-5)
                    {
                        _isNotUpsideDownMemorize = null;
                    }
                    else if (_isNotUpsideDownMemorize is null)
                    {
                        _isNotUpsideDownMemorize = IsNotUpsideDown;
                    }
                }
                // Move
                switch (currMoveType)
                {
                    case MoveType.GlobalAxis:
                    {
                        Move(GetMove(dPad.y, Vector3.forward));
                        Move(GetMove(dPad.x, Vector3.right));
                    }
                        break;
                    
                    case MoveType.ViewAxisAndRotate:
                    {
                        Rotate(dPad.x);
                        Move(GetMoveIntoView(dPad.y));
                    }
                        break;

                    case MoveType.ViewAxisAndRotateWithUpsideDownMemorize:
                    {
                        Rotate(dPad.x);
                        Move(GetMoveIntoView(dPad.y, (_isNotUpsideDownMemorize ?? false) != IsNotUpsideDown));
                    }
                        break;
                    
                    default:
                        throw new System.ArgumentOutOfRangeException("😁");
                }
            }
            
            // gradually stop
            if (!inputProvided) {
                playerRigidbody.velocity *= brakeCoefficient;
            }

            // gradually restore rotation
            if (iceEffect || !inputProvided) {
                var rotation = playerRigidbody.transform.rotation;
                playerRigidbody.transform.rotation = Quaternion.Lerp(
                    rotation,
                    Quaternion.Euler(0, rotation.eulerAngles.y, 0),
                    restoreRotationSpeed
                );
            }
            
            // gradually restore angular velocity (prevent infinite rotation)
            if (!inputProvided)
            {
                playerRigidbody.angularVelocity *= brakeCoefficient;
            }
        }
        
        /* ---- Private methods ---- */
        /// <summary>
        /// Get move vector with the given direction and magnitude.
        /// </summary>
        /// <param name="y">Magnitude</param>
        /// <param name="forward">Direction</param>
        /// <returns>Move vector</returns>
        private Vector3 GetMove(float y, Vector3 forward)
        {
            var playerPos = playerRigidbody.transform.position; // Rider's fad
            
            var direction = Vector3.Project(-playerPos, forward).normalized;
            
            Debug.DrawRay(
                playerPos,
                direction * 100, 
                Color.magenta
            );
            
            return direction * (y * force);
        }

        /// <summary>
        /// Get move into view vector. 
        /// </summary>
        /// <param name="y">Magnitude</param>
        /// <param name="reverse">Reverse the direction</param>
        /// <returns>Move vector</returns>
        private Vector3 GetMoveIntoView(float y, bool reverse = false)
        {
            var direction =
                reverse
                    ? -Forward
                    : Forward;
            
            Debug.DrawRay(
                playerRigidbody.transform.position,
                direction * 100, 
                Color.magenta
            );
            
            return direction * (y * force);
        }

        /// <summary>
        /// Project the given vector on the xOy plane and add it to the rigidbody.
        /// </summary>
        /// <param name="move">Force vector</param>
        private void Move(Vector3 move)
        {
            playerRigidbody.AddForce(Vector3.ProjectOnPlane(move, Vector3.up));
        }
        
        /// <summary>
        /// Rotate the player.
        /// </summary>
        /// <param name="x">Magnitude</param>
        private void Rotate(float x)
        {
            playerRigidbody.transform.Rotate(0, rotationSpeed * x, 0);
        }
    }
}
