using UnityEngine;

namespace Player
{
    public class PlayerControls : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private const float Speed = 3;
        
        #region Keys
        [SerializeField] private KeyCode forward_key = KeyCode.W;
        [SerializeField] private KeyCode backward_key = KeyCode.S;
        [SerializeField] private KeyCode left_key = KeyCode.A;
        [SerializeField] private KeyCode right_key = KeyCode.D;
        #endregion
        
        // Start is called before the first frame update
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKey(forward_key))
            {
                _rigidbody.AddForce(0, 0, Speed);
            }
            if (Input.GetKey(left_key))
            {
                _rigidbody.AddForce(-Speed, 0, 1);
            }
            if (Input.GetKey(right_key))
            {
                _rigidbody.AddForce(Speed, 0, 0);
            }
            if (Input.GetKey(backward_key))
            {
                _rigidbody.AddForce(0, 0, -Speed);
            }
        }
    }
}
