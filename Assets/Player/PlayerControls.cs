using UnityEngine;

namespace Player
{
    public class PlayerControls : MonoBehaviour
    {
        [SerializeField] private Camera firstPersonView;
        [SerializeField] private Camera thirdPersonView;
        [SerializeField] private float force = 10;
        
        private Rigidbody _rigidbody;
        
        #region Keys
        [SerializeField] private KeyCode forwardKey = KeyCode.W;
        [SerializeField] private KeyCode backwardKey = KeyCode.S;
        [SerializeField] private KeyCode leftKey = KeyCode.A;
        [SerializeField] private KeyCode rightKey = KeyCode.D;
        #endregion
        
        // Start is called before the first frame update
        private void Start()
        {
            firstPersonView.enabled = true;
            thirdPersonView.enabled = false;
            _rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void Update()
        {
            // Debug.Log(_rigidbody.rotation.eulerAngles);
            
            // Move and rotate
            {
                var resultForce = new Vector3();

                if (Input.GetKey(forwardKey))
                {
                    resultForce += _rigidbody.transform.forward;
                }

                if (Input.GetKey(backwardKey))
                {
                    resultForce -= _rigidbody.transform.forward;
                }

                if (Input.GetKey(leftKey))
                {
                    resultForce -= _rigidbody.transform.right;
                }

                if (Input.GetKey(rightKey))
                {
                    resultForce += _rigidbody.transform.right;
                }

                // Reverse input if object is upside down
                if (Vector3.Dot(transform.up, Vector3.down) > 0)
                {
                    resultForce *= -1;
                }

                resultForce *= force;

                resultForce.y = 0;  // todo maybe should project resultForce to oXZ plane?
                
                _rigidbody.AddForce(resultForce);
            }
            
            if (Input.GetKey(KeyCode.Alpha1))
            {
                firstPersonView.enabled = true;
                thirdPersonView.enabled = false;
            } else if (Input.GetKey(KeyCode.Alpha2))
            {
                firstPersonView.enabled = false;
                thirdPersonView.enabled = true;
            }
        }
    }
}
