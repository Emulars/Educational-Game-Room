using Assets.DM.Script.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.DM.Script.Puzzle
{
    public class CharacterManager : MonoBehaviour
    {
        public CharacterDatabase characterDB;

        public TextMeshProUGUI nameText;                   // UI text, character name
        public SpriteRenderer artworkSprite;    // UI, character's sprite

        private int selectedOption = 0;         // To track currently selected character

        // Start is called before the first frame update
        void Start()
        {
            if (!PlayerPrefs.HasKey("selectedOption"))
                selectedOption = 0;
            else
                Load();

            UpdateCharacter(selectedOption);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                NextOption();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                BackOption();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                ChangeScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        //-------- UI -----------------

        public void NextOption()
        {
            selectedOption++;
    
            if (selectedOption >= characterDB.CharacterCount)
            {
                selectedOption = 0;
            }

            UpdateCharacter(selectedOption);
            Save();
        }

        public void BackOption()
        {
            selectedOption--;

            if (selectedOption < 0)
            {
                selectedOption = characterDB.CharacterCount - 1;
            }

            UpdateCharacter(selectedOption);
            Save();
        }

        private void UpdateCharacter(int selectedOption)
        {
            Character character = characterDB.GetCharacter(selectedOption);
            artworkSprite.sprite = character.characterSprite;
            nameText.text = character.characterName;
        }


        //-------- SAVE CHOSEN CHARACTER -----------------
        private void Load()
        {
            selectedOption = PlayerPrefs.GetInt("selectedOption");
        }

        private void Save()
        {
            PlayerPrefs.SetInt("selectedOption", selectedOption);
        }

        public void ChangeScene(int sceneIndex)
        {
            var changeLevel = gameObject.GetComponent<ChangeLevel>();
            var collider = transform.GetComponent<Collider2D>();
            changeLevel.OnTriggerEnter2D(collider);
        }
    }
}

