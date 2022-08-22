using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string SceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag !="Player")return;
        SceneManager.LoadScene(SceneName);
    }
}
