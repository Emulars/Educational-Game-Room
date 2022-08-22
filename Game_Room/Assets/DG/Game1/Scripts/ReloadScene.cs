using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    public string levelName = "Game1_BossFight";
    public void Reload()
    {
        SceneManager.LoadScene(levelName);
    }
}
