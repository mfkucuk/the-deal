using UnityEngine;
using TMPro;

public class MaiNMenuText : MonoBehaviour
{
    [SerializeField] private EMainMenuTextType mainTextType;
    [SerializeField] private ETextEffects textEffect;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Color color;

    [SerializeField] private AudioSource hoverSound;

    [SerializeField] private Transition transition;

    private void OnMouseEnter()
    {
        if(!transition.TransitionStarted)
        {
            hoverSound.Play();
            text.color = color;
        }
    }

    private void OnMouseExit()
    {
        text.color = Color.white;
    }

    private void OnMouseDown()
    {
        if (!transition.TransitionStarted)
        {
            if (mainTextType == EMainMenuTextType.Start)
            {
                if(GameSceneData.Instance.GetGameSceneTutorial() != 1)
                    StartCoroutine(transition.EndTransition("GameScene"));
                else
                    StartCoroutine(transition.EndTransition("2 Cinematic"));
            }
            else if (mainTextType == EMainMenuTextType.Credits)
                StartCoroutine(transition.EndTransition("Credits"));
            else if (mainTextType == EMainMenuTextType.Settings)
            {
                SettingsData.Instance.SetSceneBeforeSettings("MainMenu");
                StartCoroutine(transition.EndTransition("Settings"));
            }
        }

    }

    private void OnMouseOver()
    {
        if (!transition.TransitionStarted)
        {
            if (textEffect != ETextEffects.None)
                TextEffectsController.Instance.DoTextEffect(text, textEffect); 
        }
    }

}
