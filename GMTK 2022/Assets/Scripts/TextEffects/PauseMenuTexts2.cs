using UnityEngine;
using TMPro;

public class PauseMenuTexts2 : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Color color;
    
    [SerializeField] private Transition transition;
    [SerializeField] private GameObject gameScene;
    
    [SerializeField] private string currSceneName;
    
    [SerializeField] private PuaseMenu puaseMenu;
    
    [SerializeField] public string musicName;

    public bool ReadyToClick { get; set; } = true;
    
    private void Start()
    {
        SettingsData.Instance.SetMusicBeforeSettings(musicName);
        
        puaseMenu.OnResumed += OnResumed;
    }

    private void OnDestroy()
    {
        puaseMenu.OnResumed -= OnResumed;
    }
    
    private void OnResumed() => OnMouseExit();

    private void OnMouseEnter()
    {
        if (ReadyToClick)
        {
            AudioManager.Instance.Play("MenuSelect");
            text.color = color;
        }
        
    }

    private void OnMouseExit()
    {
        text.color = Color.white;
    }
    
    private void OnMouseDown()
    {
        if (ReadyToClick)
        {
            ReadyToClick = false;
            
            SettingsData.Instance.SetSceneBeforeSettings(currSceneName);

            OnMouseExit();
            StartCoroutine(transition.EndTransitionAdditive("SettingsAdd", gameScene));
        }

    }
}
