using Player;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// MonoBehaviour that rotates the compass UI element to match the player's rotation.
    /// </summary>
    public class Compass : MonoBehaviour
    {
        [SerializeField] private PlayerControls controls;
        
        /// <summary>
        /// Move the compass to match the player's rotation.
        /// </summary>
        private void Update()
        {
            transform.eulerAngles = -controls.RelativeY * Vector3.forward;
        }
    }
}
