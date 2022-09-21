using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (!Input.GetButtonDown("Blockly")) return;
        if(BlocklyIsActive)
            SceneManager.UnloadSceneAsync(BlocklySceneName);
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
