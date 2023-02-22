using UnityEngine;

namespace Player
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private float indentFromPlayer;
        [SerializeField] private float height;
        [SerializeField] private float rotationInterpolation = 3;
        
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
            {
                cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation,
                    Quaternion.LookRotation(playerPosition - cameraTransform.position), Time.deltaTime * rotationInterpolation);
            }
            
            _prevPlayerPos = playerPosition;
        }
    }
}
