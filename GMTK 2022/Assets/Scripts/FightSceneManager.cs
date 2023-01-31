using UnityEngine;

public class FightSceneManager : MonoBehaviour
{
    private static FightSceneManager _instance;
    public static FightSceneManager Instance { get { return _instance; } }

    [SerializeField] private LevelDifficulty[] LevelDifficulties;
    public LevelDifficulty currLevelDifficulty { get; set; }

    [SerializeField] private int[] makeHardBoudnary;

    [SerializeField] private bool difficultyOpen;

    [SerializeField] private GameObject fightScene;
    public static bool openFightScene = false; 
    
    [SerializeField] private PuaseMenu puaseMenu;
    [SerializeField] private PauseMenuTexts2 pauseMenuText;
    
    [SerializeField] private TrailRenderer trailRenderer;

    [SerializeField] private bool isTesting = false;
    [SerializeField] private LevelDifficulty testingDiff;

    [SerializeField] private Transition transition;

    [SerializeField] private LevelDifficulty bossFight;

    [SerializeField] private LevelDifficulty tutorialLevel;

    public bool BossTime = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        
        SceneData.Instance.SetCurrSceneName("FightScene");
    }

    private void Start()
    {
        Enemy.OnAllEnemyDied += OnAllEnemyDied;

        puaseMenu.OnPaused += OnPaused;
        puaseMenu.OnResumed += OnResumed;
        
        if(FightSceneData.Instance.GetBossFightIsReady() == 0)
        {
            AudioManager.Instance.Play("FightMusic");
            SettingsData.Instance.SetMusicBeforeSettings("FightMusic");
            
            if (GameSceneData.Instance.GetGameSceneTutorial() == 2)
            {
                currLevelDifficulty = tutorialLevel;
                FightSceneData.Instance.SetFightSceneDataOpened(0);
            }
            else
            { 
                if (makeHardBoudnary[0] == FightSceneData.Instance.GetTotalFight())
                    FightSceneData.Instance.SetDifficultyLevel(FightSceneData.Instance.GetDifficultyLevel() + 1);
                else if (makeHardBoudnary[1] == FightSceneData.Instance.GetTotalFight())
                    FightSceneData.Instance.SetDifficultyLevel(FightSceneData.Instance.GetDifficultyLevel() + 1);
                else if (makeHardBoudnary[2] == FightSceneData.Instance.GetTotalFight())
                    FightSceneData.Instance.SetDifficultyLevel(FightSceneData.Instance.GetDifficultyLevel() + 1);

                if (isTesting)
                    currLevelDifficulty = testingDiff;
                else if(difficultyOpen)
                    currLevelDifficulty = LevelDifficulties[FightSceneData.Instance.GetDifficultyLevel()];
                else
                    currLevelDifficulty = LevelDifficulties[0];

            }

        }
        else
        {
            AudioManager.Instance.Play("BossFight");
            SettingsData.Instance.SetMusicBeforeSettings("BossFight");
            
            currLevelDifficulty = bossFight;
        }

        StartCoroutine(Grid.Instance.StartFightScene());
    }

    private void Update()
    {
        if (openFightScene)
        {
            
            if (puaseMenu != null)
                puaseMenu.PauseMenuInActive = false;
            
            pauseMenuText.ReadyToClick = true;
            
            openFightScene = false;
                
            fightScene.SetActive(true);
            
            StartCoroutine(transition.StartTransition());
            
            AudioManager.Instance.Play(SettingsData.Instance.GetMusicBeforeSettings());
        }
    }

    private void OnDestroy()
    {
        Enemy.OnAllEnemyDied -= OnAllEnemyDied;
        
        puaseMenu.OnPaused -= OnPaused;
        puaseMenu.OnResumed -= OnResumed;
    }

    private void OnPaused()
    {
        trailRenderer.enabled = false;
    }

    private void OnResumed()
    {
        if(!DialogueManager.Instance.DialogueStopGame)
            trailRenderer.enabled = true;
    }
    

    private void OnAllEnemyDied()
    {
        if(GameSceneData.Instance.GetGameSceneTutorial() == 2)
        {
            FightSceneData.Instance.SaveData();
        }
        
        FightSceneData.Instance.SetTotalFight(FightSceneData.Instance.GetTotalFight()+1);
    }
    
}
