using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using UnityEngine.EventSystems;
using TMPro;
using System;


public class Player : MonoBehaviour
{
    [SerializeField] private GeneralBar happyBar ; //felicita' delle persone
    [SerializeField] private TextMeshProUGUI happyBarValueTitle; //

    [SerializeField] private GeneralBar survivalBar; //sopravvivenza delle persone
    [SerializeField] private TextMeshProUGUI survivalBarValueTitile;

    public bool IsAnswerNotGiven { private set; get; }
    public bool AnswerCheat { private set; get; }

    private void Start()
    {
        SetValueInTitleBar();
    }

    //scelgo la risposta alla domanda in base alla currentQuestion passata
    public void BlockGameUpdate(string currentQuestion)
    {
        //quando premo...
        if (currentQuestion != null)
        {
            IsAnswerNotGiven = false;//per ogni nuova domanda setto la variabile a false
            AnswerCheat = false;

            if (currentQuestion == TextOnScreen.q1)
            {
                Question1();
            }
            else if (currentQuestion == TextOnScreen.q2)
            {
                Question2();
            }
            else if (currentQuestion == TextOnScreen.q3)
            {
                Question3();
            }
            else
            {
                Debug.Log("no Question");
            }
        }
    }

    //QUESTIONS

    private void Question1()
    {
        if(!CheckVariableInput("t"))
        {
            Debug.Log("Answer not given");
            IsAnswerNotGiven = true;
            happyBar.TakeDamage(GetNumberByPercentage(20, happyBar.GetValue()));
            survivalBar.TakeDamage(GetNumberByPercentage(20, survivalBar.GetValue()));
        }
        else
        {
            float percentageOfTerritory;
            try
            {
                percentageOfTerritory = float.Parse(Executor.variabili["t"]);
            }
            catch (FormatException e)
            {
                //controllo l'eccezione ed esco 
                Debug.Log($"{e.Message}");
                AnswerCheat = true;
                return;
            }

            if (percentageOfTerritory == 0)
            {
                happyBar.TakeHealth(GetNumberByPercentage(20, happyBar.GetValue()));
                survivalBar.TakeDamage(GetNumberByPercentage(40, survivalBar.GetValue()));
            }
            else if (percentageOfTerritory > 0 && percentageOfTerritory <= 100 )
            {
                //incremento la barra in base a quante percentageOfTerritory installo sul territorio
                survivalBar.TakeHealth(GetNumberByPercentage(percentageOfTerritory, survivalBar.GetValue()));
                //decremento del 20% se decido di installarle 
                happyBar.TakeDamage(GetNumberByPercentage(20, happyBar.GetValue()));
            }
            else if(percentageOfTerritory > 100 || percentageOfTerritory < 0)
            {
                AnswerCheat = true;
            }
        }
    }

    private void Question2()
    {
        if(!CheckVariableInput("min") || !CheckVariableInput("max"))
        {
            Debug.Log("Answer not given");
            IsAnswerNotGiven = true;
            happyBar.TakeDamage(GetNumberByPercentage(20, happyBar.GetValue()));
            survivalBar.TakeDamage(GetNumberByPercentage(20, survivalBar.GetValue()));
        }
        else
        {
            float min;
            float max;
            try
            {
                min = float.Parse(Executor.variabili["min"]);
                max = float.Parse(Executor.variabili["max"]);
            }
            catch (FormatException e)
            {
                Debug.Log($"{e.Message}");
                AnswerCheat = true;
                return;
            }

            if (min < max)
            {
                if (min < 18)
                {
                    survivalBar.TakeHealth(GetNumberByPercentage(20, survivalBar.GetValue()));
                    happyBar.TakeDamage(GetNumberByPercentage(40, happyBar.GetValue()));
                }
                if (max > 80)
                {
                    survivalBar.TakeHealth(GetNumberByPercentage(10, survivalBar.GetValue()));
                    happyBar.TakeDamage(GetNumberByPercentage(30, happyBar.GetValue()));
                }
                if (max - min < 40)
                {
                    survivalBar.TakeDamage(GetNumberByPercentage(40, survivalBar.GetValue()));
                    happyBar.TakeHealth(GetNumberByPercentage(30, happyBar.GetValue()));
                }
                else if(min < 0 || max > 150)
                {
                    AnswerCheat = true;
                }
                else
                {
                    survivalBar.TakeHealth(GetNumberByPercentage(30, survivalBar.GetValue()));
                    happyBar.TakeDamage(GetNumberByPercentage(10, happyBar.GetValue()));
                }
            }
            else
            {
                Debug.Log("Error: Question2: min > max!");
                AnswerCheat = true;
            }
        }
    }

    private void Question3()
    {
        //non giro il volante e non freno
        if(!CheckVariableInput("freno") && !CheckVariableInput("volante"))
        {
            Debug.Log("Answer not given");
            IsAnswerNotGiven = true;
            survivalBar.TakeDamage(GetNumberByPercentage(50, survivalBar.GetValue()));
            happyBar.TakeDamage(GetNumberByPercentage(50, happyBar.GetValue()));
        }
        //freno soltato
        else if (CheckVariableInput("freno"))
        {
            //se freno con la giusta forza diminuisco i possibili danni
            float fereno ;
            try
            {
                fereno = float.Parse(Executor.variabili["freno"]);
            }
            catch (FormatException e)
            {
                Debug.Log($"{e.Message}");
                AnswerCheat = true;
                return;
            }

            float damageSurvival = 20;
            if (fereno > 100 || fereno < 0)
            {
                AnswerCheat = true;
                return;
            }
            else if (fereno < 60 || fereno > 80  )
            {
                damageSurvival = 40;
            }
            
            //freno e giro
            if (CheckVariableInput("volante"))
            {
                string volante;
                try
                {
                    volante = Executor.variabili["volante"];
                }
                catch (FormatException e)
                {
                    Debug.Log($"{e.Message}");
                    AnswerCheat = true;
                    return;
                }

                if (volante == "destra")
                {
                    survivalBar.TakeDamage(GetNumberByPercentage(damageSurvival, survivalBar.GetValue()));
                    happyBar.TakeDamage(GetNumberByPercentage(20, happyBar.GetValue()));
                }
                else if (volante == "sinitra") //svolta fortunata
                {
                    survivalBar.TakeDamage(GetNumberByPercentage(damageSurvival - 15, survivalBar.GetValue()));
                    happyBar.TakeDamage(GetNumberByPercentage(10, happyBar.GetValue()));
                }
                else
                {
                    AnswerCheat = true;
                }
            }
            else
            {
                survivalBar.TakeDamage(GetNumberByPercentage(damageSurvival, survivalBar.GetValue()));
                happyBar.TakeDamage(GetNumberByPercentage(10, happyBar.GetValue()));
            }
        }
    }

    //setto il valore delle percentuali sullo schermo 
    public void SetValueInTitleBar()
    {
        happyBarValueTitle.text = $"Felicità della popolazione: {Math.Round(happyBar.GetValue(), 2)}%";
        survivalBarValueTitile.text = $"Sopravvivenza della specie: {Math.Round(survivalBar.GetValue(), 2)}%";
    }

    //DI CONTROLLO

    //controllo se le variabili cercate esistono 
    private bool CheckVariableInput(string nameVar)
    {
        if (Executor.variabili.Count != 0)
        {
            if (!Executor.variabili.ContainsKey(nameVar))
            {
                Debug.Log(nameVar + " :variabili: variable not found");
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            Debug.Log("variabiliTest: is null");
            return false;
        }
    }

    //ritorna la percentuale passando il numero e il totale
    private float GetPercentage(float n, float totalNumber)
    {
        //percentage = (yourNumber / totalNumber) * 100;
        return ((n / totalNumber) * 100);
    }

    //ritorna il numero data una percentuale
    private float GetNumberByPercentage(float percentage, float totalNumber)
    {
        return (percentage / 100) * totalNumber; //return: number 
    }

    public float GetValueHappyBar()
    {
        return happyBar.GetValue();
    }

    public float GetValueSurvivalBar()
    {
        return survivalBar.GetValue();
    }
}
