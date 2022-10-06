using UnityEngine;

// Scriptable object

namespace Assets.DM.Script.Puzzle
{
    [System.Serializable]
    public class Character
    {
        public string characterName;
        public Sprite characterSprite;
        public GameObject characterPrefab;
    }
}

