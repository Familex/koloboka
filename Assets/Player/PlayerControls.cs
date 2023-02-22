using UnityEngine;

namespace Player
{
    public class PlayerControls : MonoBehaviour
    {
        [SerializeField] private Camera firstPersonView;
        [SerializeField] private Camera thirdPersonView;
        
        private Rigidbody _rigidbody;
        private const float Speed = 3;
        
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
            if (Input.GetKey(forwardKey))
            {
                _rigidbody.AddForce(0, 0, Speed);
            }
            if (Input.GetKey(leftKey))
            {
                _rigidbody.AddForce(-Speed, 0, 1);
            }
            if (Input.GetKey(rightKey))
            {
                _rigidbody.AddForce(Speed, 0, 0);
            }
            if (Input.GetKey(backwardKey))
            {
                _rigidbody.AddForce(0, 0, -Speed);
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
