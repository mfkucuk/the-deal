using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameSceneController : MonoBehaviour
{
    [SerializeField] private Transition transition;

    [SerializeField] private GameObject gameScene;
    public static bool openGameScene = false; 
    
    [SerializeField] private TilemapCollider2D tilemapCollider2D;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private PuaseMenu puaseMenu;

    [SerializeField] private PauseMenuTexts2 pauseMenuText;

    private void Awake()
    {
        SceneData.Instance.SetCurrSceneName("GameScene");
    }
    
    void Start()
    {
        puaseMenu.OnPaused += OnPaused;
        puaseMenu.OnResumed += OnResumed;

        StartCoroutine(transition.StartTransition());
        AudioManager.Instance.Play("GameMusic");
    }

    private void OnDestroy()
    {
        puaseMenu.OnPaused -= OnPaused;
        puaseMenu.OnResumed -= OnResumed;
    }
    
    private void OnPaused()
    {
        trailRenderer.enabled = false;
        tilemapCollider2D.enabled = false;
    }
    
    private void OnResumed()
    {
        if (!DialogueManager.Instance.DialogueStopGame)
            trailRenderer.enabled = true;
        

        tilemapCollider2D.enabled = true;
    }

    private void Update()
    {
        if (openGameScene)
        {
            if (puaseMenu != null)
                puaseMenu.PauseMenuInActive = false;

            openGameScene = false;

            pauseMenuText.ReadyToClick = true;

            gameScene.SetActive(true);
            
            StartCoroutine(transition.StartTransition());
        }
    }
}
