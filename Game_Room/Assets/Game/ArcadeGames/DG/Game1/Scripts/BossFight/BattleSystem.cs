using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public BattleState state;
    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> enemies;
    private PlayerBehaviour playerBehaviour;

    [SerializeField] private GameObject victoryTab, lostTab, debugTab;
    
    private bool HPareModified = false;

    private void Start()
    {
        state = BattleState.START;
        SetUpBattle();
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void Update()
    {
        if (playerBehaviour == null || playerBehaviour.currentHP<=0 ) {
            state = BattleState.LOST;
            StartCoroutine(Lost());
        }
    }

    private void SetUpBattle()
    {
        playerBehaviour = player.GetComponent<PlayerBehaviour>();
        Executor.variabili["playerHP"] = playerBehaviour.maxHP.ToString();
    }

    private void PlayerTurn()
    {
        //aggiorno la vita
        Executor.variabili["playerHP"] = playerBehaviour.currentHP.ToString();
    }

    private IEnumerator WaitExecutor()
    {
        while (LaunchBlocks.launch)
        {
            yield return null;
        }

        //i blocchi modificano la vita?
        if(!HPareModified)
        {
            HPareModified = (Executor.variabili["playerHP"] != playerBehaviour.currentHP.ToString());
            playerBehaviour.currentHP = int.Parse(Executor.variabili["playerHP"]);
        }

        //i blocchi modificano lo stato?
        if (Executor.variabili["battleState"] == "WON") FalseVictory();
        if (Executor.variabili["battleState"] == "LOST") FakeLost();
    }

    private void EnemiesTurn()
    {
        IEnumerator waiter()
        {
            bool stillEnemies = false;
            foreach (var badGuy in enemies)
            {
                yield return new WaitForSeconds(1f);
                if (badGuy != null)
                {
                    badGuy.GetComponent<enemyBehaviour>().Act(player);
                    stillEnemies = true;
                }

            }
             
            if (stillEnemies == false)
            {
               
                Victory();
            }
            else
            {
                state = BattleState.PLAYERTURN;
                //eseguo blocchi
                LaunchBlocks.launch = true;
                //aspetto la fine dell'esecuzione per leggere
                StartCoroutine(WaitExecutor());
                PlayerTurn();
            }
        }
        StartCoroutine(waiter());
        
    }

    private IEnumerator Lost()
    {
        Executor.variabili["playerHP"] = playerBehaviour.currentHP.ToString();
       Debug.Log("HP = " + Executor.variabili["playerHP"]);
        LaunchBlocks.launch = true;
        yield return WaitExecutor();
        while (LaunchBlocks.launch){ }
        if (Executor.variabili["battleState"] != "LOST")
        { 
            Debug.Log("il giocatore non ha cambiato la variabile");
            if (state == BattleState.LOST)
            {
                Debug.Log("ma il gioco è finito (non ha blocchi)");
                playerBehaviour.TakeDamage(200);
                lostTab.SetActive(true);
                lostTab.transform.Find("retry").GetComponent<Button>().Select();
                yield break;
            }
            Debug.Log("e il gioco non è finito");
            SetDebug("hai finito gli HP ma non è finito il combattimento...",true);
            state = BattleState.LOST;
            yield break;
        }
        if (HPareModified)
        {
            Debug.Log("hp modificati, sconfitta");
            SetDebug("hai modificato gli hp ma hai perso lo stesso", false);
            lostTab.SetActive(true);
        }
        Debug.Log("fine");
        StartCoroutine(EndGame());
        
        playerBehaviour.TakeDamage(200);
        lostTab.SetActive(true);
        lostTab.transform.Find("retry").gameObject.SetActive(false);
    }

    private void FakeLost()
    {
        SetDebug("hai perso prima che i tuoi hp arrivassero a zero...\n" +
                 "sicuro di aver modificato battle state al momento giusto?", false);

        lostTab.SetActive(true);
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2f);
        var changeLevel = gameObject.GetComponent<ChangeLevel>();
        var collider = transform.GetComponent<Collider2D>();
        changeLevel.OnTriggerEnter2D(collider);
    }

    private void Victory()
    {
        if (HPareModified)
        {
            SetDebug("hai vinto, ma qualcosa non torna...\n hai barato?", true);
        }
        state = BattleState.WON;
        victoryTab.SetActive(true);
    }

    private void FalseVictory()
    {
        Debug.Log("false victory");
        Victory();
        //barato con state
        SetDebug("hai vinto, ma qualcosa non torna...\n hai barato?",true);
    }

    private void SetDebug(string message, bool retryBtt)
    {
        debugTab.SetActive(true);
        debugTab.transform.Find("DebugText").GetComponent<TMP_Text>().SetText(message);
        if (retryBtt)
            debugTab.transform.Find("retry").gameObject.SetActive(true);
        
    }

    public void OnAttackButton(GameObject enemy)
    {
        if(state != BattleState.PLAYERTURN)return;

        enemy.GetComponent<enemyBehaviour>().TakeDamage(playerBehaviour.damage);
        playerBehaviour.Attack(enemy.transform);
        state = BattleState.ENEMYTURN;
        
        EnemiesTurn();
    }

    public void OnMagicButton()
    {
        if(state != BattleState.PLAYERTURN)return;
        playerBehaviour.LaunchFireBall();
        foreach (var badGuy in enemies)
        {
            if(badGuy != null)
                badGuy.GetComponent<enemyBehaviour>().TakeDamage(playerBehaviour.magicDamage);
        }
        state = BattleState.ENEMYTURN;

        EnemiesTurn();
    }

}
