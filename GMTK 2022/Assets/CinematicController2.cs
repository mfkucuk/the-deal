using UnityEngine;

public class CinematicController2 : MonoBehaviour
{
    [SerializeField] private Transition transition;
    void Start()
    {
        SceneData.Instance.SetCurrSceneName("2 Cinematic");
        StartCoroutine(transition.StartTransition());
    }

}
