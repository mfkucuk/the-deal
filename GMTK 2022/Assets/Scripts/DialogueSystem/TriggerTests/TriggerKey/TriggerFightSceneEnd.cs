using UnityEngine;

public class TriggerFightSceneEnd : MonoBehaviour
{
    [SerializeField] private DialogueHolder dialogueHolderFirst;
    
    private bool canTriggerDialogue = false;

    void Start()
    {
        if (GameSceneData.Instance.GetGameSceneTutorial() != 2)
        {
            Destroy(this.gameObject);
        }

        FightSceneBackground.Instance.OnShopStarted += OnShopStarted;
        
    }

    private void OnDestroy()
    {
        FightSceneBackground.Instance.OnShopStarted -= OnShopStarted;
        
        dialogueHolderFirst.HolderOnStartDialogueActions -= HolderOnStartDialogueActions;
        dialogueHolderFirst.HolderOnCustomDialogueActions -= HolderOnCustomDialogueActions;
        dialogueHolderFirst.HolderOnEndDialogueActions -= HolderOnEndDialogueActions;
    }

    private void OnShopStarted()
    {
        dialogueHolderFirst.HolderOnStartDialogueActions += HolderOnStartDialogueActions;
        dialogueHolderFirst.HolderOnEndDialogueActions += HolderOnEndDialogueActions;
        dialogueHolderFirst.HolderOnCustomDialogueActions += HolderOnCustomDialogueActions;
            
        DialogueTrigger.Instance.TriggerDialogue();
        canTriggerDialogue = true;
    }

    void Update()
    {
        if(canTriggerDialogue && Input.GetKeyDown(KeyCode.Space))
            DialogueTrigger.Instance.TriggerDialogue();
    }
    
    private void HolderOnStartDialogueActions()
    {
        Skill.isInactive = true;
    }
    
    private void HolderOnCustomDialogueActions(RealDialogue realDialogue, int index)
    {

    }
    
    private void HolderOnEndDialogueActions()
    {
        Skill.isInactive = false; 
        
        dialogueHolderFirst.HolderOnStartDialogueActions -= HolderOnStartDialogueActions;
        dialogueHolderFirst.HolderOnCustomDialogueActions -= HolderOnCustomDialogueActions;
        dialogueHolderFirst.HolderOnEndDialogueActions -= HolderOnEndDialogueActions;
        
        Destroy(this.gameObject);
    }
    

}