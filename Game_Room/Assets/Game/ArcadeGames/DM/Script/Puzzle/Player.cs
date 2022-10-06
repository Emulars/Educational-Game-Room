using Cinemachine;
using UnityEngine;

// Class to handle the sprites of the chosen character

namespace Assets.DM.Script.Puzzle
{
    public class Player : MonoBehaviour
    {
        public CharacterDatabase characterDb;
        private int selectedOption = 0;         // To track currently selected character
        public CinemachineVirtualCamera virtualCamera;
        public static Vector2 lastCheckPointPos = new Vector2(-0.49f, 16.74f);

        private void Awake()
        {
            if (!PlayerPrefs.HasKey("selectedOption"))
                selectedOption = 0;
            else
                Load();

            UpdateCharacter(selectedOption);
        }

        private void UpdateCharacter(int selectedOption)
        {
            //Character character = characterDB.GetCharacter(selectedOption);
            //artworkSprite.sprite = character.characterSprite;

            GameObject player = Instantiate(characterDb.GetCharacterPrefab(selectedOption), lastCheckPointPos, Quaternion.identity);
            virtualCamera.m_Follow = player.transform;
        }


        //-------- SAVE CHOSEN CHARACTER -----------------
        private void Load()
        {
            selectedOption = PlayerPrefs.GetInt("selectedOption");
        }

    }
}
