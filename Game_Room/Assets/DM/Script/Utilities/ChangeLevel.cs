using System.Collections;
using Assets.DM.Script.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    //[Header("Scene indexes")]
    public int IndexToLoad
    {
        get => gameInfo.firstSceneIndex + gameInfo.sceneMaterialIndex;
    }
    
    [Header("Canvas")]
    public GameObject canvasImage;

    // Level move zoned enter, if collider is a player
    // Move game to another scene

    [Header("Texture")]
    private static MeshRenderer _meshRenderer;
    private Object[] renderTextures;    // List of all the arcade's render textures
    public string gameName;
    private GameInfos gameInfo;          // GameInfo script

    // 2D Trigger
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Could use other.GetComponent<Player>() to see if the game object has a player component
        // Tags work too, maybe some players have different script components?

        if (collision.tag == "Player")
        {
            LoadNextLevel();
            
            // Unload the current scene
            SceneManager.UnloadSceneAsync(IndexToLoad-1);

            if (gameInfo.isCompleted) return;

            // SendMessage to LoadResources.ChangeRenderTexture() SendMessageUpwards("ChangeRenderTexture", countEnterTrigger2D);
            ChangeRenderTexture(gameInfo.sceneMaterialIndex);

            // Increse the counter for the next level
            UpdateCounter();
        }
    }

    // 3D Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Could use other.GetComponent<Player>() to see if the game object has a player component
        // Tags work too, maybe some players have different script components?

        if (other.tag == "Player")
        {
            _meshRenderer = transform.Find("Screen").GetComponent<MeshRenderer>();

            LoadNextLevel();

            StartCoroutine(ActiveMessage());

            // Increse the counter for the next level
            UpdateCounter();
        }
    }

    private void LoadNextLevel()
    {
        // take the game property
        gameInfo = GameDatabase.games[gameName];

        if (gameInfo.isCompleted) return;
        
        // Get the game's material
        renderTextures = Resources.LoadAll("RenderTextures/" + gameInfo.sceneMaterialPath, typeof(Material));

        // Player entered, so move level
        //indexToLoad = gameInfo.firstSceneIndex + gameInfo.sceneMaterialIndex;
        SceneManager.LoadScene(IndexToLoad, LoadSceneMode.Additive);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {

            //indexToLoad = gameInfo.firstSceneIndex + gameInfo.sceneMaterialIndex;
            SceneManager.UnloadSceneAsync(IndexToLoad - 1);
            gameInfo.sceneMaterialIndex--;
            // reset the counter for the next level
            
            if (gameInfo.isCompleted)
                gameInfo.isCompleted = false;
        }
    }

    private IEnumerator ActiveMessage()
    {
        if(canvasImage != null)
        {
            Transform canvasText = canvasImage.transform.Find("Text");
            canvasText.GetComponent<TextMeshProUGUI>().text = "Press 'F2' to start/stop playing";
            canvasImage.SetActive(true);
            yield return new WaitForSeconds(3);
            canvasImage.SetActive(false);
        }
    }

    public void ChangeRenderTexture(int orderInFolder)
    {
        // Change the plane's Render Texture
        _meshRenderer.material = (Material)renderTextures[orderInFolder];
    }

    public void UpdateCounter()
    {
        if (gameInfo.sceneMaterialIndex >= gameInfo.numLevels)
        {
            gameInfo.isCompleted = true;
        }
        
        gameInfo.sceneMaterialIndex++;
    }
}
