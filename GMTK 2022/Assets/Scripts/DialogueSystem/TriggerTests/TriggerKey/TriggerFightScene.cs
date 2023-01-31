using UnityEngine;

public class TriggerFightScene : MonoBehaviour
{
    [SerializeField] private DialogueHolder dialogueHolderFirst;
    [SerializeField] private DialogueHolder dialogueHolderSecond;
    
    [SerializeField] private Animator animatorMove;
    [SerializeField] private Animator animatorAttack;
    
    [SerializeField] private MoveDice moveDice;
    [SerializeField] private AttackDice attackDice;
    
    private bool wholeDialogueFinished = false;

    private bool canTriggerDialogue = false;
    void Start()
    {
        if (GameSceneData.Instance.GetGameSceneTutorial() != 2)
        {
            Destroy(animatorMove.gameObject);
            Destroy(animatorAttack.gameObject);
            Destroy(this.gameObject);
        }
        else
        {
            Grid.Instance.OnGameStarted += OnGameStarted;

            moveDice.stopMoveDice = true;
            attackDice.stopAttackDice = true;
        }

    }

    private void OnDestroy()
    {
        Grid.Instance.OnGameStarted -= OnGameStarted;
        
        dialogueHolderFirst.HolderOnStartDialogueActions -= HolderOnStartDialogueActions;
        dialogueHolderFirst.HolderOnCustomDialogueActions -= HolderOnCustomDialogueActions;
        dialogueHolderFirst.HolderOnEndDialogueActions -= HolderOnEndDialogueActions;
        
       // dialogueHolderSecond.HolderOnStartDialogueActions -= HolderOnStartDialogueActions2;
        dialogueHolderSecond.HolderOnEndDialogueActions -= HolderOnEndDialogueActions2;
        dialogueHolderSecond.HolderOnCustomDialogueActions -= HolderOnCustomDialogueActions2;
    }

    private void OnGameStarted()
    {
        dialogueHolderFirst.HolderOnStartDialogueActions += HolderOnStartDialogueActions;
        dialogueHolderFirst.HolderOnEndDialogueActions += HolderOnEndDialogueActions;
        dialogueHolderFirst.HolderOnCustomDialogueActions += HolderOnCustomDialogueActions;
        
        DialogueTrigger.Instance.TriggerDialogue();
        canTriggerDialogue = true;
    }
    
    void Update()
    {
        if (Grid.Instance.GameStarted && Grid.Instance.NumberOfEnemies == 0)
        {
            moveDice.stopMoveDice = true;
            attackDice.stopAttackDice = true;
            canTriggerDialogue = true;
            
            DialogueTrigger.Instance.TriggerDialogue();
        }
        
        if (!wholeDialogueFinished && Turn_Controller.Instance.TurnState == Turn_Controller.TURN_STATE.ENEMY_TURN)
        {
            moveDice.stopMoveDice = true;
            attackDice.stopAttackDice = true;
            canTriggerDialogue = true;
            
            DialogueTrigger.Instance.TriggerDialogue();
        }
        
        if(canTriggerDialogue && Input.GetKeyDown(KeyCode.Space))
            DialogueTrigger.Instance.TriggerDialogue();
    }
    
    private void HolderOnStartDialogueActions()
    {
        
    }
    
    private void HolderOnCustomDialogueActions(RealDialogue realDialogue, int index)
    {
        if (index == 5)
        {
            animatorMove.enabled = true;
        }
        else if (index == 6)
        {
            animatorAttack.enabled = true;
        }
    }
    
    private void HolderOnEndDialogueActions()
    {
        //dialogueHolderSecond.HolderOnStartDialogueActions += HolderOnStartDialogueActions2;
        dialogueHolderSecond.HolderOnEndDialogueActions += HolderOnEndDialogueActions2;
        dialogueHolderSecond.HolderOnCustomDialogueActions += HolderOnCustomDialogueActions2;
        
        //dialogueHolderSecond.DialogueStopGame = false;
        
        DialogueTrigger.Instance.TriggerDialogue();
        
        dialogueHolderFirst.HolderOnStartDialogueActions -= HolderOnStartDialogueActions;
        dialogueHolderFirst.HolderOnCustomDialogueActions -= HolderOnCustomDialogueActions;
        dialogueHolderFirst.HolderOnEndDialogueActions -= HolderOnEndDialogueActions;
    }

    private void HolderOnCustomDialogueActions2(RealDialogue realDialogue, int index)
    {
        if (index == 0)
        {
            moveDice.stopMoveDice = false;
            canTriggerDialogue = false;
            animatorMove.Play("Shine");
        }
        else if(index == 1)
        {
            Destroy(animatorMove.gameObject);
        }
        else if (index == 11)
        {
            attackDice.stopAttackDice = false;
            canTriggerDialogue = false;
            animatorAttack.Play("Shine");
        }

    }
    
    private void HolderOnEndDialogueActions2()
    {
        wholeDialogueFinished = true;
        
        Destroy(animatorAttack.gameObject);

        dialogueHolderSecond.gameObject.SetActive(false);
            
        moveDice.stopMoveDice = false;
        attackDice.stopAttackDice = false;
        canTriggerDialogue = false;

        // dialogueHolderSecond.HolderOnStartDialogueActions -= HolderOnStartDialogueActions2;
        dialogueHolderSecond.HolderOnEndDialogueActions -= HolderOnEndDialogueActions2;
        dialogueHolderSecond.HolderOnCustomDialogueActions -= HolderOnCustomDialogueActions2;
        
        Destroy(this.gameObject);
    }
    
    
    
    
}
