using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private static LevelController _instance;
    public static LevelController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
        
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadLevelWithIndex(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadSceneWithName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void LoadCinematics2()
    {
        SceneManager.LoadScene("2 Cinematic");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadFightLevel()
    {
        SceneManager.LoadScene("FightScene");
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void LoadInventory()
    {
        SceneManager.LoadScene("Inventory");
    }
    
    public void LoadSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public int GetTotalAmountOfLevel()
    {
        return SceneManager.sceneCountInBuildSettings;
    }

    public string GetSceneNameWithIndex(int index)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(index);
        return path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
    }

    
    public void LoadSceneAdditive(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void UnLoadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

}
