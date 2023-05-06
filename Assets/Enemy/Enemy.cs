using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        public delegate void OnDeath();
        
        public event OnDeath OnDeathEvent;
        // Invoke example: OnDeathEvent?.Invoke();
    }
}