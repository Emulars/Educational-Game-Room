using System;
using Assets.DM.Script.Metroidvania.Player;
using Assets.DM.Script.Puzzle;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

/* Build index
    
DM/Scenes/Main_Scenario                         1
DM/Scenes/Metroidvania/Metroidvania_Menu        2
DM/Scenes/Metroidvania/Level_1-1                3
DM/Scenes/Metroidvania/Level_1-2                4
DM/Scenes/Metroidvania/Level_1-3                5
DM/Scenes/Metroidvania/End                      6
DM/Scenes/Puzzle/Puzzle_Menu                    7
DM/Scenes/Puzzle/Puzzle_Character_Selection     8
DM/Scenes/Puzzle/Level_2-1                      9
DM/Scenes/Puzzle/Puzzle_End                     10

Blockly/Scenese/Metroidvania-1                  11
Blockly/Scenese/Metroidvania-2                  12
Blockly/Scenese/Metroidvania-3                  13

Blockly/Scenese/Puzzle-1                        14
Blockly/Scenese/Puzzle-2                        15
*/


public class BlocklyHandler : MonoBehaviour
{
    [Header("All level")]
    [SerializeField] GameObject bindedObject;
    [SerializeField] private int guiLevelIndex;

    [Header("Quest Giver 1")]
    [SerializeField] GameObject questGiver2;

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
            // Second Metroidvania level
            case 12:
                Executor.variabili.Add("speed", "0");
                break;

            // First Puzzle interaction
            case 14:
                Executor.variabili.Add("word", "sheep");
                Executor.variabili.Add("space", " ");
                Executor.variabili.Add("result", "");
                break;

            // Second Puzzle interaction
            case 15:
                Random rnd = new Random();
                int bullets = rnd.Next(0, 101);  // creates a number between 0 and 100
                int dragons = rnd.Next(1, 101);  // creates a number between 1 and 100

                Executor.variabili.Add("result", "");
                Executor.variabili.Add("numBullets", bullets.ToString());
                Executor.variabili.Add("numDragons", dragons.ToString());
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
        if (Input.GetButtonDown("Blockly") && !isGuiOpen)
        {
            isGuiOpen = true;
            SceneManager.LoadScene(guiLevelIndex, LoadSceneMode.Additive);
        }
        // On GUI closed
        else if (Input.GetButtonDown("Blockly") && isGuiOpen)
        {
            isGuiOpen = false;

            // Choose which case based on the current level    
            switch (guiLevelIndex)
            {
                // First Metroidvania level
                case 11:
                    MetroidvaniaLevel1();
                    break;

                // Second Metroidvania level
                case 12:
                    MetroidvaniaLevel2();
                    break;

                // Third Metroidvania level
                case 13:
                    MetroidvaniaLevel3();
                    break;

                // First Puzzle interaction
                case 14:
                    PuzzleQuest1();
                    break;

                // Second Puzzle interaction
                case 15:
                    PuzzleQuest2();
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

        if (Executor.variabili.Count == 0) return;
        if (!float.TryParse(Executor.variabili["speed"], out float speed)) return;
        
        if (speed is > 0f and <= 10f)
        {
            // Move the player
            bindedObject.GetComponent<PlayerMovement>().speed = speed;
            Debug.Log("Metroidvania Level 1: \nSpeed = " + speed);
        }
    }

    private void MetroidvaniaLevel2()
    {
        // To make the jump possible, the speed must be equal to 1 and "enableJump" is set to true

        if (Executor.variabili.Count == 0) return;
        if (!bool.TryParse(Executor.variabili["enableJump"], out var enableJump) ||
            !float.TryParse(Executor.variabili["speed"], out var speed)) return;
        if (speed == 1f && enableJump)
        {
            // Jump the player
            bindedObject.GetComponent<PlayerMovement>().speed = speed;
            bindedObject.GetComponent<PlayerMovement>().enableJump = enableJump;
            Debug.Log("Metroidvania Level 2: \nSpeed = " + speed);
            Debug.Log("Metroidvania Level 2: \nEnable Jump = " + enableJump);
        }
    }

    private void MetroidvaniaLevel3()
    {
        if (Executor.variabili.Count == 0) return;
        if (!float.TryParse(Executor.variabili["damage"], out var damage)) return;
        // Deal damage to the enemy
        bindedObject.transform.Find("AttackTrigger").GetComponent<AttackTrigger>().damage = damage;
        Debug.Log("Metroidvania Level 3: \nDamage = " + damage);
    }

    private void PuzzleQuest1()
    {
        if (Executor.variabili.Count == 0) return;
        if (Executor.variabili["result"].Equals("1 sheep...2 sheep...3 sheep..."))
        {
            // If the result is correct, check if the player is near the quest giver
            if (bindedObject.GetComponent<QuestGiver>().playerIsClose)
            {
                // disable the first quest giver 
                bindedObject.SetActive(false);
                // enable the second one
                questGiver2.SetActive(true);
            }
        }
        else
        {
            // Change Quest Giver Dialog
            Array.Clear(bindedObject.GetComponent<QuestGiver>().dialogue, 0, bindedObject.GetComponent<QuestGiver>().dialogue.Length);
            bindedObject.GetComponent<QuestGiver>().dialogue[0] = "You need to return the murmur \'1 sheep...2 sheep...3 sheep...\' to complete this quest";
            bindedObject.GetComponent<QuestGiver>().dialogue[1] = "Try to use the available variables to complete the sentence";
            bindedObject.GetComponent<QuestGiver>().dialogue[2] = "Maybe a counting variable should be useful...";
            bindedObject.GetComponent<QuestGiver>().dialogue[3] = "And a while loop can be useful...";
            bindedObject.GetComponent<QuestGiver>().dialogue[4] = "Press F1 to open the Block UI";

            // Reset Variable
            Executor.variabili["word"] = "sheep";
            Executor.variabili["space"] = " ";
            Executor.variabili["result"] = "";
        }
    }

    private void PuzzleQuest2()
    {
        if (Executor.variabili.Count == 0) return;
        if (!bool.TryParse(Executor.variabili["result"], out var result)) return;

        if (!float.TryParse(Executor.variabili["numBullets"], out var nBullet) ||
            !float.TryParse(Executor.variabili["numDragons"], out var nDragon)) return;
        
        Debug.Log("Puzzle Quest 2: \nResult = " + result);
        Debug.Log("Puzzle Quest 2: \nBullets = " + nBullet);
        Debug.Log("Puzzle Quest 2: \nDragons = " + nDragon);
        var operation = ((nBullet / 2) - nDragon >= 0) ? true : false;
        if ( (operation && result) || (!operation && !result))
        {
            if (bindedObject.GetComponent<QuestGiver>().playerIsClose)
                bindedObject.SetActive(false);  // disable quest giver 
        }
    }
}

