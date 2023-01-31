using UnityEngine;

public class GameSceneTutorial : MonoBehaviour
{
    private static GameSceneTutorial _instance;
    public static GameSceneTutorial Instance { get { return _instance; } }
    
    [SerializeField] private GameObject tutorialObjects;
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }
    
    public bool IsGameSceneTutorialOpen { get; set; }

    void Start()
    {
        if (GameSceneData.Instance.GetGameSceneTutorial() != 1)
        {
            tutorialObjects.SetActive(false);
        }
    }
    
}
