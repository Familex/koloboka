using UnityEngine;

namespace Player
{
    public class PlayerControls : MonoBehaviour
    {
        /* ---- Inspector things ---- */
        [SerializeField] private float force = 15;
        [SerializeField] private float rotationSpeed = 15;
        [SerializeField] private Rigidbody playerRigidbody;
        [SerializeField] private Transform firstView;
        
        /* ---- Private mutables ---- */
        private ControlActions _controlActions;
        
        /* ---- Private properties ---- */
        private Vector3 LookVec => firstView.forward;

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
                Move(dPad.y);
                Rotate(dPad.x);
            }
        }
        
        /* ---- Private methods ---- */
        private void Move(float y)
        {
            var deltaForce = LookVec * (y * force);
            Debug.DrawRay(playerRigidbody.transform.position, deltaForce, Color.magenta);
            playerRigidbody.AddForce(deltaForce);
        }

        private void Rotate(float x)
        {
            playerRigidbody.transform.Rotate(0, rotationSpeed * x, 0);
        }
    }
}
