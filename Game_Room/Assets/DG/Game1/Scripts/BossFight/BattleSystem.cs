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

    void Start()
    {
        state = BattleState.START;
        SetUpBattle();
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void Update()
    {
        if (playerBehaviour == null || playerBehaviour.currentHP<=0 ) {
            state = BattleState.LOST;
            Lost();
        }
    }

    private void SetUpBattle()
    {
        playerBehaviour = player.GetComponent<PlayerBehaviour>();
        //inizializzo il dictionary
        Executor.variabili.Clear();
        Executor.variabili.Add("playerHP", playerBehaviour.maxHP.ToString());
        Executor.variabili.Add("battleState", "PLAYERTURN");
    }

    private void PlayerTurn()
    {
        //aggiorno la vita
        Executor.variabili["playerHP"] = playerBehaviour.currentHP.ToString();
    }

    IEnumerator waitExecutor()
    {
        while (LaunchBlocks.launch)
        {
            yield return null;
        }

        //i blocchi modificano la vita?
        HPareModified = (Executor.variabili["playerHP"] != playerBehaviour.currentHP.ToString());
        playerBehaviour.currentHP = int.Parse(Executor.variabili["playerHP"]);

        //i blocchi modificano lo stato?
        if (Executor.variabili["battleState"] == "WON") FalseVictory();
        if (Executor.variabili["battleState"] == "LOST") Lost();
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
                StartCoroutine("waitExecutor");
                PlayerTurn();
            }
        }
        StartCoroutine(waiter());
        
    }

    private void Lost()
    {
        Debug.Log("lost");
        if(float.Parse(Executor.variabili["playerHP"]) != 0 && playerBehaviour.currentHP != 0)
        {
            SetDebug("hai perso prima che i tuoi hp arrivassero a zero...\n" +
                     "sicuro di aver modificato battle state al momento giusto?",false);

            lostTab.SetActive(true);
        }
        //hai perso prima che i tuoi hp arrivassero a zero...
        //sicuro di aver modificato battle state al momento giusto?
        if (Executor.variabili["battleState"] != "LOST" && state != BattleState.LOST)
        {
            SetDebug("hai finito gli HP ma non è finito il combattimento...",true);
            
            state = BattleState.LOST;
            Destroy(gameObject);
            return;
        }

        if (state == BattleState.LOST && Executor.variabili["battleState"] != "LOST")
        {
            Destroy(gameObject);
            StartCoroutine(EndGame());
            return;
        }
        state = BattleState.LOST;
        playerBehaviour.TakeDamage(200);
        lostTab.SetActive(true);
        lostTab.transform.Find("retry").GetComponent<Button>().Select();
        Destroy(gameObject);
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(4);
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
        {
            debugTab.transform.Find("retry").gameObject.SetActive(true);
        }
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
