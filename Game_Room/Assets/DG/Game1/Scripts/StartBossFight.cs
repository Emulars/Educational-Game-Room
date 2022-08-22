using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartBossFight : MonoBehaviour
{
    [SerializeField] private GameObject HUD, debugMessage;
    [SerializeField] private Button initialButton;
    [SerializeField] private OpenWorldPlayerController player;

    private bool uscitaLastValue = false, stopExecuteBlocks = false;

    //quando il giocatore si avvicina all'uscita
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag != "Player") return;
        Executor.variabili["sullUscita"] = "true";
        uscitaLastValue = true;
        if(!bool.Parse(Executor.variabili["uscita"]))return;
        other.GetComponent<OpenWorldPlayerController>().LockMovement();
        HUD.SetActive(true);
        initialButton.Select();
        stopExecuteBlocks=true;
    }
    //quando il giocatore si allontana dall'uscita
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Player") return;
        Executor.variabili["sullUscita"] = "false";
        uscitaLastValue = false;
    }

    void Update()
    {
        if(stopExecuteBlocks)return;
        //eseguo blocchi
        LaunchBlocks.launch = true;
        //aspetto la fine dell'esecuzione per leggere
        bool sullUscita = bool.Parse(Executor.variabili["sullUscita"]);
        bool uscita = bool.Parse(Executor.variabili["uscita"]);
        bool collisioni = bool.Parse(Executor.variabili["collisioni"]);
        if (!sullUscita && uscita)
        {
            Debug.Log("ha modificato uscita");
            ShowDebugMessage("hai provato a saltare il livello? non è una cosa bella sai?");
            stopExecuteBlocks = true;
        }
    }
    //chiamata da bottone in canvas
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    
    private void ShowDebugMessage(string message)
    {
        player.LockMovement();
        debugMessage.transform.Find("DebugText").GetComponent<TMP_Text>().SetText(message);
        debugMessage.SetActive(true);
    }
}
