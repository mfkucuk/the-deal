using UnityEngine;

public class TriggerBossTalk : MonoBehaviour
{
    [SerializeField] private DialogueHolder dialogueHolder;
    [SerializeField] private SkullMovement skullMovement;
    
    private bool canTriggerDialogue = false;

    public void StartActions()
    {
        canTriggerDialogue = true;
        
        dialogueHolder.HolderOnStartDialogueActions += HolderOnStartDialogueActions;
        dialogueHolder.HolderOnCustomDialogueActions += HolderOnCustomDialogueActions;
        dialogueHolder.HolderOnEndDialogueActions += HolderOnEndDialogueActions;
    }

    private void OnDestroy()
    {
        dialogueHolder.HolderOnStartDialogueActions -= HolderOnStartDialogueActions;
        dialogueHolder.HolderOnCustomDialogueActions -= HolderOnCustomDialogueActions;
        dialogueHolder.HolderOnEndDialogueActions -= HolderOnEndDialogueActions;
    }

    private void Update()
    {
        if(canTriggerDialogue && Input.GetKeyDown(KeyCode.Space))
            DialogueTrigger.Instance.TriggerDialogue();
    }
    
    private void HolderOnStartDialogueActions()
    {
        FightSceneData.Instance.SetBossFightIsReady(1);
    }
    
    private void HolderOnCustomDialogueActions(RealDialogue realDialogue, int index)
    {

    }
    
    private void HolderOnEndDialogueActions()
    {
        StartCoroutine(skullMovement.WaitAndFight());
        
        dialogueHolder.HolderOnStartDialogueActions -= HolderOnStartDialogueActions;
        dialogueHolder.HolderOnCustomDialogueActions -= HolderOnCustomDialogueActions;
        dialogueHolder.HolderOnEndDialogueActions -= HolderOnEndDialogueActions;
    }
}
