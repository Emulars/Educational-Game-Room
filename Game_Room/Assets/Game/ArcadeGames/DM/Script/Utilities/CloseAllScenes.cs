using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseAllScenes : TurnOffScreen
{
    private void OnTriggerExit()
    {
        int nScenes = SceneManager.sceneCount;
        for (int i = 0; i< nScenes; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded && scene.name != "Main_Scenario")
                SceneManager.UnloadSceneAsync(scene);
        }

    }
}
