using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// MonoBehaviour class for Enemy.
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        public delegate void OnDeath();
        
        /// <summary>
        /// Event invoked when enemy dies.
        /// TODO: Add event listeners to this event.
        /// </summary>
        public event OnDeath OnDeathEvent;
        // Invoke example: OnDeathEvent?.Invoke();
    }
}