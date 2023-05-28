using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    /// <summary>
    /// MonoBehaviour component for scene switching.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        /// <summary>
        /// Synchronously loads scene with given name.
        /// </summary>
        /// <param name="sceneName">New scene name</param>
        /// <remarks>Invokes Debug.LogError if sceneName is invalid</remarks>
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
