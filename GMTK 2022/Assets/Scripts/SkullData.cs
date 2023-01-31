using System;
using UnityEngine;

public class SkullData : MonoBehaviour
{

    private static SkullData _instance;
    public static SkullData Instance { get { return _instance; } }

    private const string skullHealData = "SKULL_HEALTH";
    private const string skullPositionDataX = "SKULL_POSITION_X";
    private const string skullPositionDataY = "SKULL_POSITION_Y";

    private const string skullCurrDice = "SKULL_CURR_DICE";
    private const string skullTotalDice = "SKULL_TOTAL_DICE";
    
    private const string cantMove = "CANT_MOVE";

    private const string skullSavedPositionX = "SKULL_SAVED_POS_X";
    private const string skullSavedPositionY = "SKULL_SAVED_POS_Y";

    private const string gameOpened = "GAME_Opened";

    [SerializeField] private int MAX_HEALTH;
    [SerializeField] private Vector2 STARTING_POS;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
        
        PlayerPrefs.SetInt(skullHealData, PlayerPrefs.GetInt(skullHealData, MAX_HEALTH));

        PlayerPrefs.SetFloat(skullPositionDataX, PlayerPrefs.GetFloat(skullPositionDataX, STARTING_POS.x));
        PlayerPrefs.SetFloat(skullPositionDataY, PlayerPrefs.GetFloat(skullPositionDataY, STARTING_POS.y));
        
        PlayerPrefs.SetInt(skullCurrDice, PlayerPrefs.GetInt(skullCurrDice, 0));
        PlayerPrefs.SetInt(skullTotalDice, PlayerPrefs.GetInt(skullTotalDice, 3));
        
        PlayerPrefs.SetInt(cantMove, PlayerPrefs.GetInt(cantMove, 0));
        
        
        PlayerPrefs.SetFloat(skullSavedPositionX, PlayerPrefs.GetFloat(skullSavedPositionX, STARTING_POS.x));
        PlayerPrefs.SetFloat(skullSavedPositionY, PlayerPrefs.GetFloat(skullSavedPositionY, STARTING_POS.y));
        
        PlayerPrefs.SetInt(gameOpened, 1);

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDestroy()
    {
        ResetData();
    }

    public void SetHealthDatas(int lastHealth)
    {
        if(lastHealth >= MAX_HEALTH)
            PlayerPrefs.SetInt(skullHealData, MAX_HEALTH);
        else if(lastHealth <= 0)
            PlayerPrefs.SetInt(skullHealData, 0);
        else
            PlayerPrefs.SetInt(skullHealData, lastHealth);

    }

    public int GetHealtDatas()
    {
        return PlayerPrefs.GetInt(skullHealData, MAX_HEALTH);
    }

    public void SetPosDatas(float lastMoveX, float lastMoveY)
    {
        PlayerPrefs.SetFloat(skullPositionDataX, lastMoveX);
        PlayerPrefs.SetFloat(skullPositionDataY, lastMoveY);
    }

    public Vector2 GetPosDatas()
    {
        float x = PlayerPrefs.GetFloat(skullPositionDataX, STARTING_POS.x);
        float y = PlayerPrefs.GetFloat(skullPositionDataY, STARTING_POS.y);

        return new Vector2(x, y);

    }
    
    public void SetSavedPos(float lastMoveX, float lastMoveY)
    {
        PlayerPrefs.SetFloat(skullSavedPositionX, lastMoveX);
        PlayerPrefs.SetFloat(skullSavedPositionY, lastMoveY);
    }

    public Vector2 GetSavedPos()
    {
        float x = PlayerPrefs.GetFloat(skullSavedPositionX, STARTING_POS.x);
        float y = PlayerPrefs.GetFloat(skullSavedPositionY, STARTING_POS.y);

        return new Vector2(x, y);

    }
    
    public void SetGameOpened(int gameOpen)
    {
        PlayerPrefs.SetInt(gameOpened, gameOpen);
    }

    public int GetGameOpened()
    {
        return PlayerPrefs.GetInt(gameOpened, 1);
    }
    
    
    public void SetSkullCurrDice(int currDice)
    {
        PlayerPrefs.SetInt(skullCurrDice, currDice);
    }

    public int GetSkullCurrDice()
    {
        return PlayerPrefs.GetInt(skullCurrDice, 0);
    }

    public void SetSkullTotalDice(int totalDice)
    {
        PlayerPrefs.SetInt(skullTotalDice, totalDice);
    }

    public int GetSkullTotalDice()
    {
        return PlayerPrefs.GetInt(skullTotalDice, 3);
    }
    
    public void SetCantMove(int dontMove)
    {
        PlayerPrefs.SetInt(cantMove, dontMove);
    }

    public int GetCantMove()
    {
        return PlayerPrefs.GetInt(cantMove, 0);
    }
    
    
    public void ResetData()
    {
        SetHealthDatas(MAX_HEALTH);
        SetPosDatas(STARTING_POS.x, STARTING_POS.y);
        SetSkullCurrDice(0);
        SetSkullTotalDice(3);
        SetCantMove(0);
        SetGameOpened(1);
    }


}
