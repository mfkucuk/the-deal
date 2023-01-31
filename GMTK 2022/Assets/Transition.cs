using System.Collections;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private GameObject[] setInactiveObjects;
    [SerializeField] private float duration;

    [SerializeField] private Animator animator;
    [SerializeField] private PuaseMenu puaseMenu;

    public bool TransitionStarted { get; set; } = false;

    public IEnumerator StartTransition()
    {
        for(int i = 0; i < setInactiveObjects.Length; i++)
            setInactiveObjects[i].SetActive(false);
    
        animator.Play("StartTransition");
        yield return new WaitForSecondsRealtime(duration);
        
        for(int i = 0; i < setInactiveObjects.Length; i++)
            setInactiveObjects[i].SetActive(true);

        TransitionStarted = false;
    }
    
    public IEnumerator EndTransition(string sceneName)
    {
        TransitionStarted = true;
        
        for (int i = 0; i < setInactiveObjects.Length; i++)
            setInactiveObjects[i].SetActive(false);

        if (puaseMenu != null)
            puaseMenu.PauseMenuInActive = true;
        
        animator.Play("EndTransition");
        yield return new WaitForSecondsRealtime(duration);

        LevelController.Instance.LoadSceneWithName(sceneName);
    }
    
    public IEnumerator EndTransitionAdditive(string sceneName, GameObject gameObject)
    {
        TransitionStarted = true;
        
        for (int i = 0; i < setInactiveObjects.Length; i++)
            setInactiveObjects[i].SetActive(false);
        
        animator.Play("EndTransition");
        
        if (puaseMenu != null)
            puaseMenu.PauseMenuInActive = true;
        
        yield return new WaitForSecondsRealtime(duration);

        LevelController.Instance.LoadSceneAdditive(sceneName);
        
        yield return new WaitForSecondsRealtime(0.2f);
        
        gameObject.SetActive(false);
        
        StartCoroutine(StartTransition());
        
    }
    
    public IEnumerator EndTransitionUnScaledScene(string sceneName, string currScene)
    {
        TransitionStarted = true;
        
        for (int i = 0; i < setInactiveObjects.Length; i++)
            setInactiveObjects[i].SetActive(false);
        
        animator.Play("EndTransition");
        yield return new WaitForSecondsRealtime(duration);

        if (currScene == "GameScene")
        {
            GameSceneController.openGameScene = true;
        }
        else if (currScene == "FightScene")
        {
            FightSceneManager.openFightScene = true;
        }
        
        
        LevelController.Instance.UnLoadScene(sceneName);
    }

    
    
}
