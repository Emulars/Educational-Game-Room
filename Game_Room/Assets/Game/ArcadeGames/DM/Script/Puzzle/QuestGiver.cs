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

        private KeyCode DIALOG_KEY = KeyCode.Return;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(DIALOG_KEY) && playerIsClose && !dialogueInProgress)
            {
                if (dialoguePanel.activeInHierarchy)
                {
                    ResetText();
                }
                else
                {
                    dialoguePanel.SetActive(true);
                    StartCoroutine(TypeText());
                }
            }

            if (dialogueText.text == dialogue[currentDialogueIndex])
            {
                contButton.SetActive(true);
            }

            // NextLine with key press
            if (Input.GetKeyDown(DIALOG_KEY) && playerIsClose && dialogueInProgress)
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
            dialogueInProgress = true;
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
