using UnityEngine;

public class SettingMenuButton : MonoBehaviour
{
    [SerializeField] private Transition transition;

    [SerializeField] private Shake shakeController;
    private Vector3 _origPos;

    [SerializeField] private AudioSource hoverSound;
    
    private void Start()
    {
        _origPos = transform.position;
    }
    
    private void OnMouseEnter()
    {
        if(!transition.TransitionStarted)
        {
            hoverSound.Play();
            shakeController.StartMouseOnShaking(transform);
        }

    }

    private void OnMouseExit()
    {
        shakeController.resetShaking();
        transform.position = _origPos;
    }

    private void OnMouseDown()
    {
        if (!transition.TransitionStarted)
        {
            if (SettingsData.Instance.GetSceneBeforeSettings() == "GameScene")
            {
                StartCoroutine(transition.EndTransitionUnScaledScene("SettingsAdd", "GameScene"));
            }
            else if (SettingsData.Instance.GetSceneBeforeSettings() == "FightScene")
            {
                StartCoroutine(transition.EndTransitionUnScaledScene("SettingsAdd", "FightScene"));
            }
            else if(SettingsData.Instance.GetSceneBeforeSettings() == "MainMenu")
                StartCoroutine(transition.EndTransition(SettingsData.Instance.GetSceneBeforeSettings()));
        }
        
    }
}
