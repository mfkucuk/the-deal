using UnityEngine;

public class ButtonLevelController : MonoBehaviour
{
    [SerializeField] private string sceneName;
    
    [SerializeField] private Transition transition;

    [SerializeField] private Shake shakeController;
    private Vector3 _origPos;
    
    private void Start()
    {
        _origPos = transform.position;
    }
    
    private void OnMouseEnter()
    {
        if (!transition.TransitionStarted)
        {
            AudioManager.Instance.Play("MenuSelect");
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
            StartCoroutine(transition.EndTransition(sceneName));
        }
    }
    
}
