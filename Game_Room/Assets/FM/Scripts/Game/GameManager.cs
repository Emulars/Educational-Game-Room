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
    private TextMeshProUGUI textButton;
    private bool respond = false; //verifico se ho gia risposto
    private bool isGuiOpen = false;

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
        //setto l'audio di questa parte del gioco con valore preimpostato nel menu iniziale
        //audioSource.volume = PlayerPrefs.GetFloat("VolumeStart");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) && !isGuiOpen)
        {
            Debug.Log("Blockly");
            isGuiOpen = true;

            eventSystem.enabled = false;
            SceneManager.LoadScene("FMBlockly", LoadSceneMode.Additive);
        }

        else if (Input.GetKeyDown(KeyCode.F1) && isGuiOpen)
        {
            Debug.Log("Return game");
            isGuiOpen = false;

            //SceneManager.UnloadSceneAsync("FMBlockly");
            
            try { SceneManager.UnloadSceneAsync("FMBlockly"); }
            catch(ArgumentException e) { 
                Debug.Log($"{e.Message}");
                return;
            }

            //StartCoroutine(EventSystemEnableAfterTime());

            if (!respond)
            {
                //SendMessageUpwards("BlockGameUpdate", currentTextOnScreen);
                player.BlockGameUpdate(currentTextOnScreen);
                //SendMessageUpwards("SetValueInTitleBar");
                player.SetValueInTitleBar();

                respond = true; //ho risposto
                nextButton.SetActive(true); //quando ho risposto riattivo il bottone
            }
            else
            {
                Debug.Log("I've already answered ");
            }
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            if(nextButton.activeSelf)
                NextQuestion();
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
                //factText.color = Color.cyan;
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
