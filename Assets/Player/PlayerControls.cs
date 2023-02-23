using UnityEngine;

namespace Player
{
    public class PlayerControls : MonoBehaviour
    {
        [SerializeField] private float force = 10;
        [SerializeField] private float rotationSpeed = 1;
        
        private Rigidbody _rigidbody;
        
        #region Keys
        [SerializeField] private KeyCode forwardKey = KeyCode.W;
        [SerializeField] private KeyCode backwardKey = KeyCode.S;
        [SerializeField] private KeyCode leftKey = KeyCode.A;
        [SerializeField] private KeyCode rightKey = KeyCode.D;
        #endregion
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Debug.Log(_rigidbody.rotation.eulerAngles);
            
            // Move and rotate
            {
                var deltaForce = new Vector3();
                var deltaRotate = new Vector3();
                var rotation = transform.rotation;
                var rotationX = rotation.x;
                var rotationY = rotation.y;
                var forward = new Vector3(
                    Mathf.Sin(rotationY + rotationX),
                    0,
                    Mathf.Cos(rotationY + rotationX)
                );
                
                if (Input.GetKey(forwardKey))
                {
                    deltaForce += forward;
                }

                if (Input.GetKey(backwardKey))
                {
                    deltaForce -= forward;
                }

                if (Input.GetKey(leftKey))
                {
                    deltaRotate.y -= Time.deltaTime * rotationSpeed;
                }

                if (Input.GetKey(rightKey))
                {
                    deltaRotate.y += Time.deltaTime * rotationSpeed;
                }
                
                deltaForce *= force;

                Debug.DrawRay(transform.position, deltaForce, Color.magenta);
                
                _rigidbody.AddForce(deltaForce);
                _rigidbody.transform.Rotate(deltaRotate);
            }
        }
    }
}
