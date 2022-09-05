using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.DM.Script.Puzzle
{
    public class QuestGiver : MonoBehaviour
    {
        public GameObject dialoguePanel;
        public TextMeshProUGUI dialogueText;
        public string[] dialogue;
        private int currentDialogueIndex;

        public float wordSpeed; // typing dialog speed
        public bool playerIsClose; // check player proximity to collider

        public GameObject contButton;
        private bool dialogueInProgress = false;

        // Update is called once per frame
        void Update()
        {
            // Button2 to init the dialog
            // Button1 for next line
            if (ButtonVR.button2 && playerIsClose && !dialogueInProgress)
            {
                if (dialoguePanel.activeInHierarchy)
                {
                    ResetText();
                }
                else
                {
                    dialoguePanel.SetActive(true);
                    dialogueInProgress = true;
                    StartCoroutine(TypeText());
                }
            }

            if (dialogueText.text == dialogue[currentDialogueIndex])
            {
                contButton.SetActive(true);
            }

            // NextLine with key press
            if (ButtonVR.button1 && playerIsClose && dialogueInProgress)
            {
                NextLine();
            }
        }

        public void ResetText()
        {
            dialogueText.text = "";
            currentDialogueIndex = 0;
            dialogueInProgress = false;
            dialoguePanel.SetActive(false);
        }

        IEnumerator TypeText()
        {
            foreach (char letter in dialogue[currentDialogueIndex].ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(wordSpeed);
            }
        }

        public void NextLine()
        {
            //At the beginning the button is disabled
            contButton.SetActive(false);

            if (currentDialogueIndex < dialogue.Length - 1)
            {
                currentDialogueIndex++;
                dialogueText.text = "";
                StartCoroutine(TypeText());
            }
            else
            {
                ResetText();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerIsClose = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerIsClose = false;
                ResetText();
            }
        }
    }
}
