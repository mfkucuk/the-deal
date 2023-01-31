using UnityEngine;
using UnityEngine.Audio;

public class SettingsData : MonoBehaviour
{
    private static SettingsData _instance;
    public static SettingsData Instance { get { return _instance; } }
    
    public AudioMixer AudioMixer;

    private const string SettingsBoneAmount = "BONE_AMOUNT";
    private int initialBoneAmount = 3;

    private const string SceneBeforeSettings = "SCENE_BEFORE_SETTINGS";
    private string initialSceneBeforeSettings = "MainMenu";

    private const string lastMusicBeforeSettings = "LAST_MUSIC_BEFORE_SETTINGS";
    private string initialMusicBeforeSettings = "FightMusic";
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
        
        PlayerPrefs.SetInt(SettingsBoneAmount, PlayerPrefs.GetInt(SettingsBoneAmount, initialBoneAmount));
        PlayerPrefs.SetString(SceneBeforeSettings, PlayerPrefs.GetString(SceneBeforeSettings, initialSceneBeforeSettings));
        
        PlayerPrefs.SetString(lastMusicBeforeSettings, PlayerPrefs.GetString(lastMusicBeforeSettings, initialMusicBeforeSettings));
        
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        AudioMixer.SetFloat("Volume", -10);
        PlayerPrefs.SetInt(SettingsBoneAmount, initialBoneAmount);
    }

    private void OnDestroy()
    {
        ResetData();
        SetSettingsBoneAmount(initialBoneAmount);
    }

    public int GetSettingsBoneAmount()
    {
        return PlayerPrefs.GetInt(SettingsBoneAmount, initialBoneAmount);
    }
    
    public void SetSettingsBoneAmount(int boneAmount)
    { 
        PlayerPrefs.SetInt(SettingsBoneAmount, boneAmount);
    }
    
    public string GetSceneBeforeSettings()
    {
        return PlayerPrefs.GetString(SceneBeforeSettings, initialSceneBeforeSettings);
    }
    
    public void SetSceneBeforeSettings(string sceneName)
    { 
        PlayerPrefs.SetString(SceneBeforeSettings, sceneName);
    }
    
    public string GetMusicBeforeSettings()
    {
        return PlayerPrefs.GetString(lastMusicBeforeSettings, initialMusicBeforeSettings);
    }
    
    public void SetMusicBeforeSettings(string musicName)
    { 
        PlayerPrefs.SetString(lastMusicBeforeSettings, musicName);
    }

    public void ResetData()
    {
        SetMusicBeforeSettings(initialMusicBeforeSettings);
        SetSceneBeforeSettings(initialSceneBeforeSettings);
    }
    
    
}
