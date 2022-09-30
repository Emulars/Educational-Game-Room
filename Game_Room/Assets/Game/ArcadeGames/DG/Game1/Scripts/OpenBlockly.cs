using UEBlockly;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.Extras;

public class OpenBlockly : MonoBehaviour
{
    public string BlocklySceneName = "UBGame1_Lv1";
    private bool BlocklyIsActive = false;


    private void Start()
    {
        BlocklyInitializer();
    }

    private void BlocklyInitializer()
    {

        Executor.variabili.Clear();
        switch (BlocklySceneName)
        {
            case "UBGame1_Lv1":
                Executor.variabili.Add("collisioni", "false");
                break;
            case "UBGame1_Lv2":
                Executor.variabili.Add("collisioni", "true");
                Executor.variabili.Add("sullUscita", "false");
                Executor.variabili.Add("esci", "false");
                break;
            case "UBGame1_Lv3":
                Executor.variabili.Add("playerHP", "");
                Executor.variabili.Add("battleState", "false");
                SceneManager.LoadScene(BlocklySceneName, LoadSceneMode.Additive);
                BlocklyIsActive = true;
                break;
                
        }
        
    }

    private void Update()
    {
        if (BlocklyIsActive && SteamVR_Input.GetStateDown("default", "triggerLaser", SteamVR_Input_Sources.Any))
            SteamVR_LaserPointer.isActive = !SteamVR_LaserPointer.isActive;
        if (!SteamVR_Input.GetStateDown("default", "BlocklyOpen", SteamVR_Input_Sources.LeftHand)) return;
        if (BlocklyIsActive)
        {
            SceneManager.UnloadSceneAsync(BlocklySceneName);
            SteamVR_LaserPointer.isActive = false;
        }
        else
            SceneManager.LoadScene(BlocklySceneName, LoadSceneMode.Additive);
         
        BlocklyIsActive = !BlocklyIsActive;
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag != "Player")return;
        UnloadThisScene();
    }

    public void UnloadThisScene()
    {
        if(BlocklyIsActive)
            SceneManager.UnloadSceneAsync(BlocklySceneName);
    }
}
