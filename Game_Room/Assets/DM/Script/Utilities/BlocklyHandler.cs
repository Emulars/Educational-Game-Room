using System;
using Assets.DM.Script.Metroidvania.Player;
using Assets.DM.Script.Puzzle;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Build index
    
DM/Scenes/Main_Scenario                         0
DM/Scenes/Metroidvania/Metroidvania_Menu        1
DM/Scenes/Metroidvania/Level_1-1                2
DM/Scenes/Metroidvania/Level_1-2                3
DM/Scenes/Metroidvania/Level_1-3                4
DM/Scenes/Metroidvania/End                      5
DM/Scenes/Puzzle/Puzzle_Menu                    6
DM/Scenes/Puzzle/Puzzle_Character_Selection     7
DM/Scenes/Puzzle/Level_2-1                      8

Blockly/Scenese/Metroidvania-1                  9
Blockly/Scenese/Metroidvania-2                  10
Blockly/Scenese/Metroidvania-3                  11

Blockly/Scenese/Puzzle-1                        12
Blockly/Scenese/Puzzle-2                        13
*/


public class BlocklyHandler : MonoBehaviour
{
    [SerializeField] GameObject bindedObject;
    [SerializeField] private int guiLevelIndex;

    private bool isGuiOpen = false; // To check if the Block UI is already open, to be able to close it with the same key
    private int currentLevel; // To store the current level index

    // Start is called before the first frame update
    void Start()
    {
        LevelInizializer();

        // Pick the current scene buildIndex
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    private void LevelInizializer()
    {
        // Clean-up the dictionary at the start
        Executor.variabili.Clear();

        // Add the needed variables for each level
        switch (guiLevelIndex)
        {
            // First Puzzle interaction
            case 12:
                Executor.variabili.Add("word", "sheep");
                Executor.variabili.Add("space", " ");
                Executor.variabili.Add("result", "");
                break;

            default:
                Debug.Log("No GUI variables needed for this level");
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // if the gui is not already open
        if (Input.GetKeyDown(KeyCode.F1) && !isGuiOpen)
        {
            isGuiOpen = true;
            SceneManager.LoadScene(guiLevelIndex, LoadSceneMode.Additive);
        }
        // On GUI closed
        else if (Input.GetKeyDown(KeyCode.F1) && isGuiOpen)
        {
            isGuiOpen = false;

            // Choose which case based on the current level    
            switch (guiLevelIndex)
            {
                // First Metroidvania level
                case 9:
                    MetroidvaniaLevel1();
                    break;

                // Second Metroidvania level
                case 10:
                    MetroidvaniaLevel2();
                    break;

                // Third Metroidvania level
                case 11:
                    MetroidvaniaLevel3();
                    break;

                // First Puzzle interaction
                case 12:
                    PuzzleQuest1();
                    break;

                // Second Puzzle interaction
                case 13:
                    PuzzleQuest1();
                    break;

                default:
                    Debug.Log("No GUI found for this level");
                    throw new NotImplementedException();
            }

            // Unload the UI scene
            SceneManager.UnloadSceneAsync(guiLevelIndex);
        }
    }

    // Create one method for each level to handle
    private void MetroidvaniaLevel1()
    {
        // To move the character the speed should be higher than 0

        if (Executor.variabili.Count != 0)
        {
            if (Executor.variabili["speed"] != null)
            {
                float speed = float.Parse(Executor.variabili["speed"]);
                bindedObject.GetComponent<PlayerMovement>().speed = speed;
                Debug.Log("Metroidvania Level 1: \nSpeed = " + speed);
            }
        }
    }

    private void MetroidvaniaLevel2()
    {
        // To make the jump possible, the speed must be equal to 1 and "enableJump" is set to true

        if (Executor.variabili.Count != 0)
        {
            if (Executor.variabili["enableJump"] != null && Executor.variabili["speed"] != null)
            {
                float speed = float.Parse(Executor.variabili["speed"]);
                bool enableJump = bool.Parse(Executor.variabili["enableJump"]);
                bindedObject.GetComponent<PlayerMovement>().speed = speed;
                bindedObject.GetComponent<PlayerMovement>().enableJump = enableJump;
                Debug.Log("Metroidvania Level 2: \nSpeed = " + speed);
                Debug.Log("Metroidvania Level 2: \nEnable Jump = " + enableJump);
            }
        }
    }

    private void MetroidvaniaLevel3()
    {
        if (Executor.variabili.Count != 0)
        {
            if (Executor.variabili["damage"] != null)
            {
                float damage = float.Parse(Executor.variabili["damage"]);
                bindedObject.transform.Find("AttackTrigger").GetComponent<AttackTrigger>().damage = damage;
                Debug.Log("Metroidvania Level 3: \nDamage = " + damage);
            }
        }
    }

    private void PuzzleQuest1()
    {
        if (Executor.variabili.Count != 0)
        {

            if (Executor.variabili["result"].Equals("1 sheep...2 sheep...3 sheep..."))
            {
                // If the result is correct, check if the player is near the quest giver
                if (bindedObject.GetComponent<QuestGiver>().playerIsClose)
                {
                    // disable the first quest giver 
                    bindedObject.SetActive(false);
                    // enable the second one
                    GameObject.Find("Quest Giver 2").SetActive(true);
                }
            }
            else
            {
            Array.Clear(bindedObject.GetComponent<QuestGiver>().dialogue, 0, bindedObject.GetComponent<QuestGiver>().dialogue.Length);
            bindedObject.GetComponent<QuestGiver>().dialogue[0] = "I need 3 sheep to complete this quest";
            bindedObject.GetComponent<QuestGiver>().dialogue[1] = "Press F1 to open the Block UI";
            }
        
        }
    }
}

