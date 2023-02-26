using UnityEngine;

namespace Player
{
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
        
        /* ---- Unity overrides ---- */
        private void Awake()
        {
            _controlActions = new ControlActions();
        }

        public void OnEnable()
        {
            _controlActions.Enable();
        }

        public void OnDisable()
        {
            _controlActions.Disable();
        }

        private void FixedUpdate()
        {
            // Move and rotate
            {
                var dPad = _controlActions.gameplay.move.ReadValue<Vector2>();
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
                        Move(dPad.y, Vector3.forward);
                        Move(dPad.x, Vector3.right);
                    }
                        break;
                    
                    case MoveType.ViewAxisAndRotate:
                    {
                        Rotate(dPad.x);
                        MoveIntoView(dPad.y);
                    }
                        break;

                    case MoveType.ViewAxisAndRotateWithUpsideDownMemorize:
                    {
                        Rotate(dPad.x);
                        MoveIntoView(dPad.y, (_isNotUpsideDownMemorize ?? false) != IsNotUpsideDown);
                    }
                        break;
                    
                    default:
                        throw new System.ArgumentOutOfRangeException();    // 😁
                }
            }
        }
        
        /* ---- Private methods ---- */
        private void Move(float y, Vector3 forward)
        {
            var playerPos = playerRigidbody.transform.position; // Rider's fad
            
            var direction = Vector3.Project(-playerPos, forward).normalized;
            
            Debug.DrawRay(
                playerPos,
                direction * 100, 
                Color.magenta
            );
            
            playerRigidbody.AddForce( direction * (y * force) );
        }

        private void MoveIntoView(float y, bool reverse = false)
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
            
            playerRigidbody.AddForce( direction * (y * force) );
        }

        private void Rotate(float x)
        {
            playerRigidbody.transform.Rotate(0, rotationSpeed * x, 0);
        }
    }
}
