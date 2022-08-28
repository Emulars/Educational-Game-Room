using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class GameManager : MonoBehaviour
{
    private List<string> textOnScreen; 
    private int indexTextOnScreen = 0;
    private string currentTextOnScreen;
    private bool respond = false; //verifico se ho gia risposto
    private bool isGuiOpen = false;

    private TextMeshProUGUI textButton;
    private string nameScene = String.Empty; //nome della scena 

    [SerializeField] private GameObject nextButton;
    [SerializeField] private Player player;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private TextMeshProUGUI factText; //testo che scorre a schermo 
    [SerializeField] private float wordSpeed; //0.06
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        //inizializzo le varie parti del testo
        textOnScreen = new List<string>
        {
            TextOnScreen.p1,
            TextOnScreen.p2,
            TextOnScreen.q1,
            TextOnScreen.q2,
            TextOnScreen.p3,
            TextOnScreen.p4,
            TextOnScreen.q3,
            TextOnScreen.pFinal
        };

        StartCoroutine(Typing());
        currentTextOnScreen = textOnScreen[indexTextOnScreen];

        //prendo il campo di testo del bottone
        textButton = nextButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Blockly") && !isGuiOpen)
        {
            isGuiOpen = true;

            eventSystem.enabled = false;

            SetScene();
            if (nameScene != String.Empty) { 
                SceneManager.LoadScene(nameScene, LoadSceneMode.Additive); 
            }

            void SetScene()
            {
                if (currentTextOnScreen.Equals(TextOnScreen.q1))
                {
                    nameScene = "FMBlockly1";
                }
                else if (currentTextOnScreen.Equals(TextOnScreen.q2))
                {
                    nameScene = "FMBlockly2";
                }
                else if (currentTextOnScreen.Equals(TextOnScreen.q3))
                {
                    nameScene = "FMBlockly3";
                }
                else
                {
                    nameScene = String.Empty;
                }
            }
        }

        else if (Input.GetButtonDown("Blockly") && isGuiOpen)
        {
            isGuiOpen = false;
            
            try { SceneManager.UnloadSceneAsync(nameScene); }
            catch(ArgumentException e) { 
                Debug.Log($"{e.Message}");
                return;
            }

            //StartCoroutine(EventSystemEnableAfterTime());

            if (!respond)
            {
                player.BlockGameUpdate(currentTextOnScreen);
                player.SetValueInTitleBar();

                if (player.IsAnswerNotGiven)
                {
                    IncorrectAnswer(TextOnScreen.answerNotGiven);
                    respond = true; //ho risposto
                    nextButton.SetActive(true); //quando ho risposto riattivo il bottone
                }
                else if (player.AnswerCheat) //se l'utente non bara posso andare avanti 
                {
                    IncorrectAnswer(TextOnScreen.answerCheat);
                }
                else
                {
                    factText.text = currentTextOnScreen;
                    respond = true; //ho risposto
                    nextButton.SetActive(true); //quando ho risposto riattivo il bottone
                }
            }
            else
            {
                Debug.Log("I've already answered ");
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (nextButton.activeSelf) 
            {
                NextQuestion();
                //if (player.AnswerCheat) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                /*var changeLevel = gameObject.GetComponent<ChangeLevel>();
                //changeLevel.gameName = "Game";
                changeLevel.CompareTag("Restart");
                var collider = transform.GetComponent<Collider2D>();
                changeLevel.OnTriggerEnter2D(collider);*/
            }
        }

        void IncorrectAnswer(string alternativeText)
        {
            factText.text = alternativeText;
            textOnScreen.Insert(indexTextOnScreen + 1, alternativeText); //inserisco dopo alla posizione corrente
            indexTextOnScreen++; //mi sposto nella nuova posizione corrente
            StartCoroutine(Typing());
        }
    }

    public void NextQuestion()
    {
        //torno alla home
        if (textButton.text.Equals("End"))
        {
            var changeLevel = gameObject.GetComponent<ChangeLevel>();
            var collider = transform.GetComponent<Collider2D>();
            changeLevel.OnTriggerEnter2D(collider);
        }

        respond = false; //setto che non ho risposto a ogni nuova domanda

        indexTextOnScreen++;

        if (indexTextOnScreen < textOnScreen.Count)
        {
            //stampo il testo a schermo
            StartCoroutine(Typing());
            currentTextOnScreen = textOnScreen[indexTextOnScreen];

            SetIfQuestions();
        }
        else if(indexTextOnScreen == textOnScreen.Count)
        {
            Debug.Log("FINE");
            SetFinalText();

            //prima di passare alla prossima scena cambio nome al bottone
            textButton.text = "End";
        }

        //in base alla domanda scelgo il fontStyle e disattivo il bottone
        void SetIfQuestions()
        {
            if (currentTextOnScreen == TextOnScreen.q1 || currentTextOnScreen == TextOnScreen.q2 || currentTextOnScreen == TextOnScreen.q3)
            {
                factText.fontStyle = FontStyles.Bold;
                nextButton.SetActive(false); //se e' una domanda disattivo il bottone
            }
            else
            {
                factText.fontStyle = FontStyles.SmallCaps;
            }
        }

        //Seleziono l'ultima risposta in base al valore delle barre
        void SetFinalText()
        {
            float happyBarValue = player.GetValueHappyBar();
            float survivalBarValue = player.GetValueSurvivalBar();

            if (happyBarValue <= 50 && survivalBarValue <= 50)
            {
                textOnScreen.Add(TextOnScreen.endBad);
                StartCoroutine(Typing());
            }
            else if (happyBarValue > 50 && survivalBarValue > 50)
            {
                textOnScreen.Add(TextOnScreen.endGood);
                StartCoroutine(Typing());
            }
            else if (happyBarValue >= 50 && survivalBarValue < 50)
            {
                textOnScreen.Add(TextOnScreen.endHealth);
                StartCoroutine(Typing());
            }
            else if (happyBarValue < 50 && survivalBarValue >= 50)
            {
                textOnScreen.Add(TextOnScreen.endSurvival);
                StartCoroutine(Typing());
            }
        }
    }

    //faccio apparire a schermo lettera per lettera con un delay
    private IEnumerator Typing()
    {
        factText.text = "";
        foreach (char letter in textOnScreen[indexTextOnScreen].ToCharArray())
        {
            factText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    private IEnumerator EventSystemEnableAfterTime() //float time
    {
        //utilizzo WaitForSeconds per evitare che contemporaneamente nelle scene ci sia attivo eventSystem
        yield return new WaitForSeconds(0.5f);
        eventSystem.enabled = true;
    }
}
