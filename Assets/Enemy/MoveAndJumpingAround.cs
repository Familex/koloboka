using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
    public class MoveAndJumpingAround : MonoBehaviour
    {
        private enum State
        {
            Idle,
            InJump
        }

        public float jumpForce = 5;
        public Transform goal;

        private readonly Dictionary<State, float> _stateDuration = new();
        // in seconds
        private Rigidbody _rigidbody;
        private NavMeshAgent _agent;
        private bool _isGrounded = true;

        private void Start()
        {
            _stateDuration[State.Idle] = 2;
            _stateDuration[State.InJump] = 2;
            
            _rigidbody = GetComponent<Rigidbody>();
            _agent = GetComponent<NavMeshAgent>();

            StartCoroutine(Jumping());
        }

        private System.Collections.IEnumerator Jumping()
        {
            while (true)
            {
                _isGrounded = false;
                SetAgentEnabled(false);
                _rigidbody.isKinematic = false;
                _rigidbody.useGravity = true;
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                yield return new WaitForSeconds(_stateDuration[State.InJump]);
                yield return new WaitUntil(() => _isGrounded);
                SetAgentEnabled(true);
                yield return new WaitForSeconds(_stateDuration[State.Idle]);
                // some after idle logic
            }
        }

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