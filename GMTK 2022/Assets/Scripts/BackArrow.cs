using UnityEngine;

public class BackArrow : MonoBehaviour
{
    [SerializeField] private Shake shakeController;
    private Vector3 _origPos;

    [SerializeField] private Transition transition;

    private bool dialogueStarted;
    
    private void Start()
    {
        _origPos = transform.position;
    }

    private void OnMouseEnter()
    {
    
        if (DialogueManager.Instance != null)
            dialogueStarted = DialogueManager.Instance.isDialogueStarted;
        else
            dialogueStarted = false;
        
        if (!transition.TransitionStarted && !dialogueStarted)
        {
            shakeController.StartMouseOnShaking(transform);
            AudioManager.Instance.Play("BackSound");
        }
    }

    private void OnMouseExit()
    {
        shakeController.resetShaking();
        transform.position = _origPos;
    }

    private void OnMouseDown()
    {
        if (DialogueManager.Instance != null)
            dialogueStarted = DialogueManager.Instance.isDialogueStarted;
        else
            dialogueStarted = false;
        
        if (!transition.TransitionStarted && !dialogueStarted)
        {
            if (InventoryManager.Instance.CheckFilled())
            {
                StartCoroutine(transition.EndTransition("GameScene"));
            }
            else
            {
                AudioManager.Instance.Play("DiceFaceEmpty");
                InventoryManager.Instance.ShakeEmptyFaces();
            }
        }

    }
}
