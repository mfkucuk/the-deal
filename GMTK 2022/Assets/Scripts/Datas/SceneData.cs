using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    private static SceneData _instance;
    public static SceneData Instance { get { return _instance; } }

    private const string currSceneName = "CURR_SCENE_NAME";

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
        
        PlayerPrefs.SetString(currSceneName, PlayerPrefs.GetString(currSceneName, "ManagerScene"));

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDestroy()
    {
        ResetData();
    }

    public void SetCurrSceneName(string sceneName)
    {
        PlayerPrefs.SetString(currSceneName, sceneName);
    }

    public string GetCurrSceneName()
    {
        return PlayerPrefs.GetString(currSceneName, "ManagerScene");
    }

    public void ResetData()
    {
        SetCurrSceneName("ManagerScene");
    }

    
    
}
