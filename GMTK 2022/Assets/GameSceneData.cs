using UnityEngine;

public class GameSceneData : MonoBehaviour
{
    
    private static GameSceneData _instance;
    public static GameSceneData Instance { get { return _instance; } }

    private const string GameSceneTutorial = "GAME_SCENE_TUTORIAL"; //  1 -> open 
    private const string InventoryOpened = "INVENTORY_OPENED";  
    
    private const string savePointOpened = "SAVE_POINT_OPENED";

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
        
        PlayerPrefs.SetInt(GameSceneTutorial, PlayerPrefs.GetInt(GameSceneTutorial, 1));
        PlayerPrefs.SetInt(InventoryOpened, PlayerPrefs.GetInt(InventoryOpened, 0));
        
        PlayerPrefs.SetInt(savePointOpened, PlayerPrefs.GetInt(savePointOpened, 1));

        DontDestroyOnLoad(this.gameObject);
    }

    public int GetGameSceneTutorial()
    {
        return PlayerPrefs.GetInt(GameSceneTutorial, 1);
    }

    public void SetGameSceneTutorial(int value)
    {
        PlayerPrefs.SetInt(GameSceneTutorial, value);
    }
    
    public int GetInventoryOpened()
    {
        return PlayerPrefs.GetInt(InventoryOpened, 0);
    }

    public void SetInventoryOpened(int value)
    {
        PlayerPrefs.SetInt(InventoryOpened, value);
    }
    
    public int GetSavePointOpened()
    {
        return PlayerPrefs.GetInt(savePointOpened, 1);
    }
    
    public void SetSavePointOpened(int isOpened)
    {
        PlayerPrefs.SetInt(savePointOpened, isOpened);
    }

    public void ResetData()
    {
        SetGameSceneTutorial(1);
        SetInventoryOpened(0);
    }
    
    
}
