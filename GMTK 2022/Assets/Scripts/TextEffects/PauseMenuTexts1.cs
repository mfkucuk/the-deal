using UnityEngine;
using TMPro;

public class PauseMenuTexts1 : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Color color;
    
    [SerializeField] private PuaseMenu puaseMenu;
    [SerializeField] private Transition transition;
    
    [SerializeField] private PauseMenuTexts2 pauseMenuTexts2;

    private void Start()
    {
        puaseMenu.OnResumed += OnResumed;
    }

    private void OnDestroy()
    {
        puaseMenu.OnResumed -= OnResumed;
    }

    private void OnResumed() => OnMouseExit();

    private void OnMouseEnter()
    {
        if (pauseMenuTexts2.ReadyToClick)
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
        if (pauseMenuTexts2.ReadyToClick)
        {
            pauseMenuTexts2.ReadyToClick = false;

            PuaseMenu.GameIsPaused = false;
            
            Time.timeScale = 1f;
            
            SkullData.Instance.SetCantMove(0);
            
            SettingsData.Instance.ResetData();
            FightSceneData.Instance.ResetData();
        
            DialogueData.Instance.ResetData();
        
            SkullData.Instance.ResetData();
        
            InventoryData.Instance.ResetData();
            InventoryData.Instance.SaveData();
        
            SkillData.Instance.ResetData();
            SkillData.Instance.SaveData();
            
            DialogueData.Instance.ResetData();
            
            InventoryData.Instance.SetLatestSkill(-1);
            
            StartCoroutine(transition.EndTransition("MainMenu"));
        }
        
    }
}
