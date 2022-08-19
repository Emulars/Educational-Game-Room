using System.Collections.Generic;
using UnityEngine;

namespace Assets.DM.Script.Utilities
{
    public class GameDatabase : MonoBehaviour
    {
        public static Dictionary<string, GameInfos> games = new Dictionary<string, GameInfos>();

        void Start()
        {
            // Metroidvania
            games.Add("Metroidvania", new GameInfos
            {
                firstSceneIndex = 1,
                numLevels = 4,
                sceneMaterialPath = "Metroidvania"
            });

            // Labyrinth / Puzzle
            games.Add("Puzzle", new GameInfos
            {
                firstSceneIndex = 6,
                sceneMaterialPath = "Puzzle"
            });

        }
    }
}
