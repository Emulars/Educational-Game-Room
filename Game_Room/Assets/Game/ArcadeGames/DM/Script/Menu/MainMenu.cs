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
    //VR
    private void Update()
    {
        if (ButtonVR.button1)
            PlayGame();
    }
    public void PlayGame()
    {
        var changeLevel = gameObject.GetComponent<ChangeLevel>();
        var collider = transform.GetComponent<Collider2D>();
        changeLevel.OnTriggerEnter2D(collider);
    }

    public void RestartGame()
    {
        var changeLevel = gameObject.GetComponent<ChangeLevel>();
        var x = transform.Find("retry");
        var button = x.GetComponent<Collider2D>();
        Debug.Log("button ha il collider:  " + button.tag +"  menu ha il collider:  "+ gameObject.tag);
        changeLevel.OnTriggerEnter2D(button);
    }
}
