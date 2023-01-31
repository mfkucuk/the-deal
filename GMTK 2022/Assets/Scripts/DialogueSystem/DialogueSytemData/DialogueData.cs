using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
    private static DialogueData _instance;

    public static DialogueData Instance
    {
        get { return _instance; }
    }

    private const string currDialogueIndex = "CURR_DIALOGUE_INDEX";
    
    private const string savedDialogueIndex = "SAVED_DIALOGUE_INDEX";
    
    private int initialDialogueIndex = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        PlayerPrefs.SetInt(currDialogueIndex, PlayerPrefs.GetInt(currDialogueIndex, initialDialogueIndex));
        PlayerPrefs.SetInt(savedDialogueIndex, PlayerPrefs.GetInt(savedDialogueIndex, initialDialogueIndex));

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDestroy()
    {
        ResetData();
    }

    public int GetCurrDialogueIndex()
    {
        return PlayerPrefs.GetInt(currDialogueIndex, initialDialogueIndex);
    }
    
    public void SetCurrDialogueIndex(int currIndex)
    {
        PlayerPrefs.SetInt(currDialogueIndex, currIndex);
    }
    
    public int GetSavedDialogueIndex()
    {
        return PlayerPrefs.GetInt(savedDialogueIndex, initialDialogueIndex);
    }
    
    public void SetSavedDialogueIndex(int savedIndex)
    {
        PlayerPrefs.SetInt(savedDialogueIndex, savedIndex);
    }

    public void ResetData()
    {
        SetCurrDialogueIndex(0);
    }


}
