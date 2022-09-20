using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.Extras;

public class CloseAllScenes : MonoBehaviour
{   private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        int nScenes = SceneManager.sceneCount;
        for (int i = 0; i< nScenes; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded && scene.name != "Main_Scenario")
                SceneManager.UnloadSceneAsync(scene);
        }
        SteamVR_LaserPointer.isActive = false;
    }
}
