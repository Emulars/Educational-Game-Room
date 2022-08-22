using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenBlockly : MonoBehaviour
{
    public string BlocklySceneName = "UBGame1_Lv1";
    // Update is called once per frame


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
                Executor.variabili.Add("uscita", "false");
                break;

            //caso BossFight delegato a BattleSystem
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            SceneManager.LoadScene(BlocklySceneName, LoadSceneMode.Additive);

        if (Input.GetKeyDown(KeyCode.F2))
            SceneManager.UnloadSceneAsync(BlocklySceneName);
    }
}
