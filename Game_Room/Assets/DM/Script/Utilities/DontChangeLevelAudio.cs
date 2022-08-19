using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.DM.Script.Utilities
{
    public class DontChangeLevelAudio : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
        }

        void Update()
        {
            if (SceneManager.GetActiveScene().buildIndex == 4)
                Destroy(transform.gameObject);
        }    
    }
}
