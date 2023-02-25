using System;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;    // Rider spam this boilerplate every time –> remove

namespace Player
{
    public class PlayerControls : MonoBehaviour
    {
        /* ---- Private types ---- */
        private enum MoveType
        {
            GlobalAxis,    
            ViewAxisAndRotate
        }
        
        /* ---- Inspector things ---- */
        [SerializeField] private float force = 15;
        [SerializeField] private float rotationSpeed = 15;
        [SerializeField] private Rigidbody playerRigidbody;
        [SerializeField] private Transform firstView;
        
        /* ---- Private mutables ---- */
        private ControlActions _controlActions;
        
        /* ---- Private properties ---- */
        private Vector3 Forward => firstView.forward;
        private Vector3 Up => firstView.up;
        private static MoveType CurrMoveType => MoveType.ViewAxisAndRotate;
        
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
                switch (CurrMoveType)
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
                        Move(dPad.y, null);
                    }
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();    // 😁
                }
            }
        }
        
        /* ---- Private methods ---- */
        private void Move(float y, Vector3? forward)
        {
            var direction =
                forward.HasValue 
                ? Vector3.Project(-playerRigidbody.transform.position, forward.Value).normalized
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
