using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int indexToLoad;

    private void Start()
    {
        transform.Find("PlayButton").GetComponent<Button>().Select();
    }

    public void PlayGame()
    {
        var changeLevel = gameObject.GetComponent<ChangeLevel>();
        var collider = transform.GetComponent<Collider2D>();

        changeLevel.OnTriggerEnter2D(collider);
    }

    // TODO: REMOVE THIS FUNCTION
    public void QuitGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
