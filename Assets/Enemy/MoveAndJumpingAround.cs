using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    /// <summary>
    /// MonoBehaviour for enemy that can move and jump around.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
    public class MoveAndJumpingAround : MonoBehaviour
    {
        /// <summary>
        /// Enemy states.
        /// </summary>
        private enum State
        {
            Move,
            BeforeJump,
            InJump,
            AfterJump
        }

        public float jumpForce = 5;
        public Transform goal;

        private readonly Dictionary<State, float> _stateDuration = new();

        // in seconds
        private Rigidbody _rigidbody;
        private NavMeshAgent _agent;
        private Animator _animator;
        private bool _isGrounded = true;
        /// <summary>
        /// Current state of enemy.
        /// </summary>
        private State _state = State.Move;
        
        private static readonly int CanJumpAnimId = Animator.StringToHash("canJump");
        private static readonly int LandedAnimId = Animator.StringToHash("landed");
        private static readonly int StartJumpAnimId = Animator.StringToHash("startJump");
        private static readonly int StartMoveAnimId = Animator.StringToHash("startMove");

        /// <summary>
        /// Init cooldown dict, get references to components and start jumping coroutine.
        /// </summary>
        private void Start()
        {
            _stateDuration[State.Move] = 2;
            _stateDuration[State.InJump] = 0;

            _rigidbody = GetComponent<Rigidbody>();
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();

            StartCoroutine(Jumping());
        }

        /// <summary>
        /// Update animator states.
        /// </summary>
        private void Update()
        {
            if (_isGrounded)
            {
                _animator.SetTrigger(LandedAnimId);
            }
        }

        /// <summary>
        /// Coroutine for state machine.
        /// </summary>
        /// <returns>IEnumerator for coroutine</returns>
        private System.Collections.IEnumerator Jumping()
        {
            while (true)
            {
                _state = State.BeforeJump;
                yield return new WaitUntil(() => _animator.GetBool(StartJumpAnimId));
                _animator.ResetTrigger(StartJumpAnimId);
                _state = State.InJump;
                /* jump */ {
                    _isGrounded = false;
                    SetAgentEnabled(false);
                    _rigidbody.isKinematic = false;
                    _rigidbody.useGravity = true;
                    _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
                yield return new WaitUntil(() => _isGrounded);
                _state = State.AfterJump;
                _animator.SetTrigger(LandedAnimId);
                yield return new WaitUntil(() => _animator.GetBool(StartMoveAnimId));
                _animator.ResetTrigger(StartMoveAnimId);
                _state = State.Move;
                SetAgentEnabled(true);
                yield return new WaitForSeconds(_stateDuration[State.Move]);
                _animator.SetTrigger(CanJumpAnimId);
                // some after idle logic
            }
        }

        /// <summary>
        /// Checks "Ground" tag and set _isGrounded to true if it's true.
        /// </summary>
        /// <param name="collision">Object that collided with enemy</param>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider is not null && collision.collider.CompareTag("Ground"))
            {
                if (!_isGrounded)
                {
                    SetAgentEnabled(true);
                    _rigidbody.isKinematic = true;
                    _rigidbody.useGravity = false; // ??
                    _isGrounded = true;
                }
            }
        }

        /// <summary>
        /// Util method for setting agent's properties.
        /// </summary>
        /// <param name="val">Value to set</param>
        private void SetAgentEnabled(bool val)
        {
            _agent.updatePosition = val;
            _agent.updateRotation = val;
            _agent.isStopped = !val;
            if (val && goal is not null)
            {
                _agent.destination = goal.position;
            }
        }
    }
}