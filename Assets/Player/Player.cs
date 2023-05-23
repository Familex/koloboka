using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(SceneController))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private string mainMenuSceneName = "Menu";
        
        public void OnDeath()
        {
            // TODO: make some `you died` message to player
            Debug.Log("Player died; load main menu scene...");
            GetComponent<SceneController>().LoadScene(mainMenuSceneName);
        }
    }
}