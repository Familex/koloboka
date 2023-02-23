using UnityEngine;

namespace Player
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private float indentFromPlayer = 10;
        [SerializeField] private float height = 1;
        [SerializeField] private float rotationInterpolation = 100;
        [SerializeField] private bool rotateToPlayer = true;
        
        private Vector3 _prevPlayerPos;

        private void Start()
        {
            _prevPlayerPos = player.transform.position;
        }

        private void FixedUpdate()
        {
            var playerPosition = player.transform.position;
            var playerDirection = (playerPosition - _prevPlayerPos).normalized;
            var cameraTransform = transform;
            
            // Camera move
            {
                var delta = -playerDirection * indentFromPlayer;
                delta.y = height;
                cameraTransform.position = playerPosition + delta;
            }
            // Camera rotate
            if (rotateToPlayer) {
                cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation,
                    Quaternion.LookRotation(playerPosition - cameraTransform.position), Time.deltaTime * rotationInterpolation);
            }
            
            _prevPlayerPos = playerPosition;
        }
    }
}
