using UnityEngine;

public class CreditsController : MonoBehaviour
{
    [SerializeField] private Transition transition;

    [SerializeField] private GameObject finishGame;
    
    void Start()
    {
        if (FightSceneData.Instance.GetBossFightIsReady() == 1)
        {
            finishGame.SetActive(true);
        }
        
        StartCoroutine(transition.StartTransition());
        AudioManager.Instance.Play("CreditsTheme");
    }
    
}
