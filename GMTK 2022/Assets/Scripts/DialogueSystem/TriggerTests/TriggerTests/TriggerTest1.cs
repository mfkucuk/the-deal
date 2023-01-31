using UnityEngine;
using TMPro;
public class TriggerTest1 : MonoBehaviour
{

    [SerializeField] private DialogueHolder dialogueHolder;
    
    [SerializeField] private BoxCollider2D boxCollider;
    
    private bool canStartDialogue = false;
    private bool dialogueStarted = false;

    private bool firstTrigger = true;

    [SerializeField] private GameObject bffSkull;
    
    [SerializeField] private GameObject bones;
    [SerializeField] private GameObject backPack;
    [SerializeField] private GameObject gameSceneDice;

    [SerializeField] private GameObject sword;
    [SerializeField] private TMP_Text fightCallAmountText;
    
    [SerializeField] private TMP_Text currDiceAmount;

    private void Update()
    {
        if (dialogueStarted)
        {
            if(Input.GetKeyDown(KeyCode.Space))
                DialogueTrigger.Instance.TriggerDialogue();
        }

        if (canStartDialogue)
        {
            dialogueStarted = true;
            canStartDialogue = false;
            DialogueTrigger.Instance.TriggerDialogue();
        }


    }

    private void OnDestroy()
    {
        dialogueHolder.HolderOnStartDialogueActions -= HolderOnStartDialogueActions;
        dialogueHolder.HolderOnCustomDialogueActions -= HolderOnCustomDialogueActions;
        dialogueHolder.HolderOnEndDialogueActions -= HolderOnEndDialogueActions;
    }

    private void HolderOnStartDialogueActions()
    {
        
    }

    private void HolderOnCustomDialogueActions(RealDialogue realDialogue, int index)
    {
        if(index == 7)
            Destroy(bffSkull);
        else if(index == 8)
            bones.SetActive(true);
        else if(index == 11)
            backPack.SetActive(true);
        else if(index == 15)
            gameSceneDice.SetActive(true);
        else if (index == 17)
        {
            currDiceAmount.text = "0";
            currDiceAmount.enabled = true;
            DiceAmountController.Instance.cantMove = true;
        }
        else if (index == 18)
        {
            sword.SetActive(true);
            fightCallAmountText.enabled = true;
        }
           
    }

    private void HolderOnEndDialogueActions()
    {
        GameSceneData.Instance.SetGameSceneTutorial(2);
        
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
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        canStartDialogue = false;
    }

}
