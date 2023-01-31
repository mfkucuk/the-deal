using UnityEngine;

public class FightSceneData : MonoBehaviour
{
    private static FightSceneData _instance;
    public static FightSceneData Instance { get { return _instance; } }
    
    private const string difficultyLevel = "DIFFICULTY_LEVEL";
    private const string totalFight = "TOTAL_FIGHT";

    private const string savedDiffLevel = "SAVED_DIFF_LEVEL";
    private const string savedTotalFight = "SAVED_TOTAL_FIGHT";

    private const string fightSceneDataOpened = "FIGHTSCENE_OPENED";

    private const string bossFightIsReady = "BOOS_FIGHT_IS_READY";

    private int totalLevelDifficulties = 3;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        PlayerPrefs.SetInt(difficultyLevel, PlayerPrefs.GetInt(difficultyLevel, 0));
        PlayerPrefs.SetInt(totalFight, PlayerPrefs.GetInt(totalFight, 0));
        
        PlayerPrefs.SetInt(savedDiffLevel, PlayerPrefs.GetInt(savedDiffLevel, 0));
        PlayerPrefs.SetInt(savedTotalFight, PlayerPrefs.GetInt(savedTotalFight, 0));
        
        PlayerPrefs.SetInt(fightSceneDataOpened, 1);
        
        PlayerPrefs.SetInt(bossFightIsReady, 0);
        
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDestroy()
    {
        SetDifficultyLevel(0);
        SetTotalFight(0);
        
        PlayerPrefs.SetInt(bossFightIsReady, 0);
        PlayerPrefs.SetInt(fightSceneDataOpened, 0);
    }

    public int GetDifficultyLevel()
    {
        return PlayerPrefs.GetInt(difficultyLevel, 0);
    }
    
    public void SetDifficultyLevel(int diffLevel)
    {
        if(diffLevel < totalLevelDifficulties)
            PlayerPrefs.SetInt(difficultyLevel, diffLevel);
    }
    
    public int GetTotalFight()
    {
        return PlayerPrefs.GetInt(totalFight, 0);
    }
    
    public void SetTotalFight(int totFight)
    {
        PlayerPrefs.SetInt(totalFight, totFight);
    }
    
    public int GetSavedTotalFight()
    {
        return PlayerPrefs.GetInt(savedTotalFight, 0);
    }
    
    public void SetSavedTotalFight(int totFight)
    {
        PlayerPrefs.SetInt(savedTotalFight, totFight);
    }
    
    public int GetSavedDiffFight()
    {
        return PlayerPrefs.GetInt(savedDiffLevel, 0);
    }
    
    public void SetSavedDiffFight(int totFight)
    {
        PlayerPrefs.SetInt(savedDiffLevel, totFight);
    }
    
    
    public int GetFightSceneDataOpened()
    {
        return PlayerPrefs.GetInt(fightSceneDataOpened, 1);
    }
    
    public void SetFightSceneDataOpened(int isOpened)
    {
        PlayerPrefs.SetInt(fightSceneDataOpened, isOpened);
    }
    
    public int GetBossFightIsReady()
    {
        return PlayerPrefs.GetInt(bossFightIsReady, 0);
    }

    public void SetBossFightIsReady(int bossFightReady)
    {
        PlayerPrefs.SetInt(bossFightIsReady, bossFightReady);
    }
    
    public void ResetData()
    {
        SetBossFightIsReady(0);
        SetDifficultyLevel(0);
        SetTotalFight(0);
    }
    
    public void SaveData()
    {
        SetSavedDiffFight(GetDifficultyLevel());
        SetSavedTotalFight(GetTotalFight());
    }
    
    
}
