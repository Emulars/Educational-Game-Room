using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelAudioManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().buildIndex == 5)
            Destroy(transform.gameObject);
    }
}
