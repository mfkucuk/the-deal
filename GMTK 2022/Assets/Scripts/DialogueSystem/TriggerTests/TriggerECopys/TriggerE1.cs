using UnityEngine;
using TMPro;

public class TriggerE1 : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    
    [SerializeField] private DialogueHolder dialogueHolder;

    private bool canStartDialogue = false;
    private bool dialogueStarted = false;

    private bool firstTrigger = true;

    [SerializeField] private BoxCollider2D unWalkableWall;
    [SerializeField] private CircleCollider2D selfCollider;

    [SerializeField] private GameObject moveTexts;
    

    private void OnDestroy()
    {
        dialogueHolder.HolderOnStartDialogueActions -= HolderOnStartDialogueActions;
        dialogueHolder.HolderOnCustomDialogueActions -= HolderOnCustomDialogueActions;
        dialogueHolder.HolderOnEndDialogueActions -= HolderOnEndDialogueActions;
    }

    private void Update()
    {
        if (dialogueStarted)
        {
            if(Input.GetKeyDown(KeyCode.Space))
                DialogueTrigger.Instance.TriggerDialogue();
        }

        if (canStartDialogue)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                dialogueStarted = true;
                canStartDialogue = false;
                DialogueTrigger.Instance.TriggerDialogue();
            }
        }


    }
    
    private void HolderOnStartDialogueActions()
    {
        canStartDialogue = false;
        
        text.enabled = false;
        moveTexts.SetActive(false);
    }
    
    private void HolderOnCustomDialogueActions(RealDialogue realDialogue, int index)
    {

    }
    
    private void HolderOnEndDialogueActions()
    {
        dialogueStarted = false;
        
        unWalkableWall.enabled = false;
        selfCollider.enabled = false;
        
        Destroy(this.gameObject);

        dialogueHolder.HolderOnStartDialogueActions -= HolderOnStartDialogueActions;
        dialogueHolder.HolderOnCustomDialogueActions -= HolderOnCustomDialogueActions;
        dialogueHolder.HolderOnEndDialogueActions -= HolderOnEndDialogueActions;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (firstTrigger)
        {
            firstTrigger = false;
            
            dialogueHolder.HolderOnStartDialogueActions += HolderOnStartDialogueActions;
            dialogueHolder.HolderOnCustomDialogueActions += HolderOnCustomDialogueActions;
            dialogueHolder.HolderOnEndDialogueActions += HolderOnEndDialogueActions;
        }

        canStartDialogue = true;
        text.enabled = true;
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        canStartDialogue = false;
        text.enabled = false;
    }
    
}