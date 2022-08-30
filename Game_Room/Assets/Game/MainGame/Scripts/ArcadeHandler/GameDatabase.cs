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
                firstSceneIndex = 2,
                numLevels = 4,
                sceneMaterialPath = "Metroidvania"
            });

            // Labyrinth / Puzzle
            games.Add("Puzzle", new GameInfos
            {
                firstSceneIndex = 7,
                numLevels = 3,
                sceneMaterialPath = "Puzzle"
            });

            //Dystopian Future
            games.Add("DystopianFuture", new GameInfos
            {
                firstSceneIndex = 16,
                numLevels = 2,
                sceneMaterialPath = "DystopianFuture"
            });

            //Legend of Carl
            games.Add("LegendOfCarl", new GameInfos
            {
                firstSceneIndex = 22,
                numLevels = 4,
                sceneMaterialPath = "LegendOfCarl"
            });

        }
    }
}
