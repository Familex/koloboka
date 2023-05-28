using UI;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// MonoBehaviour class for player.
    /// </summary>
    [RequireComponent(typeof(SceneController))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private string mainMenuSceneName = "Menu";

        /// <summary>
        /// Util method to load main menu scene.
        /// </summary>
        private void OnDeath()
        {
            // TODO: make some `you died` message to player
            Debug.Log("Player died; load main menu scene...");
            GetComponent<SceneController>().LoadScene(mainMenuSceneName);
        }

        /// <summary>
        /// Check if player is dead when colliding with enemy.
        /// </summary>
        /// <param name="other">Sus enemy trigger</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                OnDeath();
            }
        }
    }
}