
namespace Assets.DM.Script.Utilities
{
    public class GameInfos
    {
        public int firstSceneIndex;       // First Scene index of the game
        public int numLevels;           // Number of levels in the game
        public string sceneMaterialPath; // Material Scene path
        public int sceneMaterialIndex = 0;  // Material Scene index
        public bool isCompleted = false;       // True if all the scene are cleared
    }
}
